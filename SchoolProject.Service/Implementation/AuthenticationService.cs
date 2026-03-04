using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Infrustructure.Abstracts;
using SchoolProject.Service.Abstracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace SchoolProject.Service.Implementation
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Fields
        private readonly JwtSettings _jwtSettings;
        //private readonly ConcurrentDictionary<string, RefreshToken> _userRefreshToken;
        private readonly IUserRefreshTokenRepository _userRefreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        #endregion
        #region Constructors
        public AuthenticationService(JwtSettings jwtSettings, IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _jwtSettings = jwtSettings;
            //_userRefreshToken = new ConcurrentDictionary<string, RefreshToken>();
            _userRefreshTokenRepository = unitOfWork.userRefreshToken;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Generates a new JWT access token and refresh token for the specified user.
        /// The refresh token is persisted in the database and linked to the generated access token.
        /// Returns both tokens wrapped inside a JwtAuthResult.
        /// </summary>
        /// <param name="user">The authenticated user.</param>
        /// <returns>
        /// JwtAuthResult containing the access token and refresh token.
        /// </returns>
        public async Task<JwtAuthResult> GenerateJwtAuthResultAsync(User user)
        {
            var (securityToken, accessToken) = await GenerateAccessToken(user);
            var refreshToken = GetRefreshToken(user.UserName);
            var userRefreshToken = new UserRefreshToken
            {
                AddedTime = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpireDate),
                IsUsed = true,
                IsRevoked = false,
                JwtId = securityToken.Id,
                RefreshToken = refreshToken.TokenString,
                Token = accessToken,
                UserId = user.Id
            };
            await _unitOfWork.userRefreshToken.AddAsync(userRefreshToken);
            var result = await _unitOfWork.SaveChangesAsync();
            return new JwtAuthResult { AccessToken = accessToken, refreshToken = refreshToken };
        }
        /// <summary>
        /// Generates a new access token using an existing valid refresh token.
        /// Does not create a new refresh token, but reuses the existing one.
        /// </summary>
        /// <param name="user">The user associated with the refresh token.</param>
        /// <param name="ExpiredJwtToken">The expired access token.</param>
        /// <param name="expiryDate">The refresh token expiration date.</param>
        /// <param name="ValidRefreshToken">The existing refresh token string.</param>
        /// <returns>
        /// JwtAuthResult containing the newly generated access token and the existing refresh token.
        /// </returns>
        public async Task<JwtAuthResult> GenerateNewAccessTokenFromRefreshAsync(User user, JwtSecurityToken ExpiredJwtToken, DateTime? expiryDate, string ValidRefreshToken)
        {
            var (jwtSecurityToken, newToken) = await GenerateAccessToken(user);
            var response = new JwtAuthResult();
            response.AccessToken = newToken;
            var refreshTokenResult = new RefreshToken();
            // prefer using the provided user object for username to avoid missing claim issue
            refreshTokenResult.UserName = user?.UserName ?? ExpiredJwtToken?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            refreshTokenResult.TokenString = ValidRefreshToken;
            // ensure expiryDate has value before casting
            refreshTokenResult.ExpireAt = expiryDate ?? DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpireDate);
            response.refreshToken = refreshTokenResult;
            return response;

        }
        /// <summary>
        /// Reads and parses a JWT access token without validating it.
        /// </summary>
        /// <param name="accessToken">The JWT token string.</param>
        /// <returns>
        /// JwtSecurityToken containing decoded header, payload, and claims.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if accessToken is null or empty.
        /// </exception>
        public JwtSecurityToken ReadJwtToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }
            var handler = new JwtSecurityTokenHandler();
            var response = handler.ReadJwtToken(accessToken);
            return response;
        }
        /// <summary>
        /// Validates a JWT access token against configured issuer, audience,
        /// signing key, and lifetime validation rules.
        /// </summary>
        /// <param name="accessToken">The JWT token string.</param>
        /// <returns>
        /// A string representing validation result:
        /// "NotExpired" if valid,
        /// "InvalidToken" if invalid,
        /// or an exception message if validation fails.
        /// </returns>
        public async Task<string> ValidateAccessTokenAsync(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = _jwtSettings.ValidateIssuer,
                ValidIssuers = new[] { _jwtSettings.Issuer },
                ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
                ValidAudience = _jwtSettings.Audience,
                ValidateAudience = _jwtSettings.ValidateAudience,
                ValidateLifetime = _jwtSettings.ValidateLifeTime
                // ,ClockSkew = TimeSpan.Zero
            };
            try
            {
                var validator = handler.ValidateToken(accessToken, parameters, out SecurityToken validatedToken);

                if (validator == null)
                {
                    return "InvalidToken";
                }

                return "NotExpired";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// Validates the refresh token against the expired access token and database records.
        /// Ensures:
        /// - The token algorithm is correct.
        /// - The access token is expired.
        /// - The refresh token exists in the database.
        /// - The refresh token has not expired.
        /// 
        /// If valid, returns the user ID and refresh token expiry date.
        /// Otherwise, returns an error string and null expiry date.
        /// </summary>
        /// <param name="jwtToken">The expired access token.</param>
        /// <param name="accessToken">The expired access token string.</param>
        /// <param name="refreshToken">The refresh token string.</param>
        /// <returns>
        /// Tuple containing:
        /// - string: UserId if valid, otherwise error message.
        /// - DateTime?: Expiry date if valid, otherwise null.
        /// </returns>
        public async Task<(string, DateTime?)> ValidateRefreshTokenAsync(JwtSecurityToken jwtToken, string accessToken, string refreshToken)
        {
            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
            {
                return ("AlgorithmIsWrong", null);
            }
            if (jwtToken.ValidTo > DateTime.UtcNow)
            {
                return ("TokenIsNotExpired", null);
            }

            //Get User

            var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimModel.Id)).Value;
            var userRefreshToken = await _userRefreshTokenRepository.GetTableNoTracking()
                                             .FirstOrDefaultAsync(x => x.Token == accessToken &&
                                                                     x.RefreshToken == refreshToken &&
                                                                     x.UserId == int.Parse(userId));
            if (userRefreshToken == null)
            {
                return ("RefreshTokenIsNotFound", null);
            }

            if (userRefreshToken.ExpiryDate < DateTime.UtcNow)
            {
                userRefreshToken.IsRevoked = true;
                userRefreshToken.IsUsed = false;
                await _userRefreshTokenRepository.UpdateAsync(userRefreshToken);
                await _unitOfWork.SaveChangesAsync();
                return ("RefreshTokenIsExpired", null);
            }
            var expirydate = userRefreshToken.ExpiryDate;
            return (userId, expirydate);
        }
        /// <summary>
        /// Updates the existing refresh token record in the database
        /// by attaching a newly generated access token and marking it as used.
        /// </summary>
        /// <param name="accessToken">The newly generated access token.</param>
        /// <param name="refreshToken">The refresh token string.</param>
        /// <returns>
        /// True if the update was successful; otherwise false.
        /// </returns>
        public async Task<bool> UpdateRefreshTokenWithNewAccessTokenAsync(string accessToken, string refreshToken)
        {
            var existingToken = await _userRefreshTokenRepository.GetTableNoTracking()
                                        .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
            if (existingToken == null)
            {
                return false;
            }
            existingToken.Token = accessToken;
            existingToken.IsUsed = true;
            await _userRefreshTokenRepository.UpdateAsync(existingToken);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }
        #endregion
        #region Helper Methods
        /// <summary>
        /// Builds a list of claims for the specified user,
        /// including identity claims, roles, and custom user claims.
        /// </summary>
        /// <param name="user">The user entity.</param>
        /// <returns>
        /// List of claims to be embedded in the JWT.
        /// </returns>
        private async Task<List<Claim>> GetClaims(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(nameof(UserClaimModel.PhoneNumber), user.PhoneNumber),
                new Claim(nameof(UserClaimModel.Id), user.Id.ToString())
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);
            return claims;
        }
        /// <summary>
        /// Generates a signed JWT access token for the specified user.
        /// Includes user claims and roles, and applies expiration based on configuration.
        /// </summary>
        /// <param name="user">The user entity.</param>
        /// <returns>
        /// Tuple containing:
        /// - JwtSecurityToken (raw token object)
        /// - string (serialized access token)
        /// </returns>
        private async Task<(JwtSecurityToken, string)> GenerateAccessToken(User user)
        {
            List<Claim> claims = await GetClaims(user);
            DateTime expires = DateTime.UtcNow.AddDays(_jwtSettings.AccessTokenExpireDate);
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));
            SigningCredentials? signingCredentials = new SigningCredentials(key: securityKey, algorithm: SecurityAlgorithms.HmacSha256);

            // Pass Paramenter to Generate JWT Token
            var securityToken = new JwtSecurityToken(issuer: _jwtSettings.Issuer, audience: _jwtSettings.Audience, claims: claims
                                                    , notBefore: null, expires: expires, signingCredentials: signingCredentials);
            // Generate Access Token and RefreshToken then return them as a JwtAuthResult
            var accessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return (securityToken, accessToken);
        }
        /// <summary>
        /// Creates a new refresh token model for the specified username.
        /// The token string is generated securely using cryptographic randomness.
        /// </summary>
        /// <param name="userName">The username associated with the token.</param>
        /// <returns>
        /// A new RefreshToken instance.
        /// </returns>
        private RefreshToken GetRefreshToken(string userName)
        {
            var refreshToken = new RefreshToken
            {
                UserName = userName,
                ExpireAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpireDate),
                TokenString = GenerateRefreshToken()
            };
            //_userRefreshToken.AddOrUpdate(refreshToken.TokenString, refreshToken, (s, t) => refreshToken);
            return refreshToken;
        }
        /// <summary>
        /// Generates a secure random refresh token string using a cryptographically
        /// secure random number generator.
        /// </summary>
        /// <returns>
        /// A Base64 encoded random token string.
        /// </returns>
        private string GenerateRefreshToken()
        {
            var randomNumber = new Byte[32];
            var randomNumberGenerate = RandomNumberGenerator.Create();
            randomNumberGenerate.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        #endregion
    }
}
