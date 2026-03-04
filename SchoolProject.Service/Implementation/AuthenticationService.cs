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

        #region Methods
        public async Task<JwtAuthResult> GetJWTToken(User user)
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
        private string GenerateRefreshToken()
        {
            var randomNumber = new Byte[32];
            var randomNumberGenerate = RandomNumberGenerator.Create();
            randomNumberGenerate.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        public async Task<List<Claim>> GetClaims(User user)
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
        public async Task<JwtAuthResult> GetRefreshToken(User user, JwtSecurityToken jwtToken, DateTime? expiryDate, string refreshToken)
        {
            var (jwtSecurityToken, newToken) = await GenerateAccessToken(user);
            var response = new JwtAuthResult();
            response.AccessToken = newToken;
            var refreshTokenResult = new RefreshToken();
            // prefer using the provided user object for username to avoid missing claim issue
            refreshTokenResult.UserName = user?.UserName ?? jwtToken?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            refreshTokenResult.TokenString = refreshToken;
            // ensure expiryDate has value before casting
            refreshTokenResult.ExpireAt = expiryDate ?? DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpireDate);
            response.refreshToken = refreshTokenResult;
            return response;

        }
        public JwtSecurityToken ReadJWTToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }
            var handler = new JwtSecurityTokenHandler();
            var response = handler.ReadJwtToken(accessToken);
            return response;
        }

        public async Task<string> ValidateToken(string accessToken)
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
        public async Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken)
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
        public async Task<bool> SaveJWTTokenToRefreshToken(string accessToken, string refreshToken)
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
    }
}
