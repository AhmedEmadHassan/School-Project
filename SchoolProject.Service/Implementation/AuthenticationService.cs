using Microsoft.IdentityModel.Tokens;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Infrustructure.Abstracts;
using SchoolProject.Service.Abstracts;
using System.Collections.Concurrent;
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
        private readonly ConcurrentDictionary<string, RefreshToken> _userRefreshToken;
        private readonly IUserRefreshTokenRepository _userRefreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        #endregion
        #region Constructors
        public AuthenticationService(JwtSettings jwtSettings, IUnitOfWork unitOfWork)
        {
            _jwtSettings = jwtSettings;
            _userRefreshToken = new ConcurrentDictionary<string, RefreshToken>();
            _userRefreshTokenRepository = unitOfWork.userRefreshToken;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods
        public async Task<JwtAuthResult> GetJWTToken(User user)
        {
            List<Claim> claims = GetClaims(user);
            DateTime expires = DateTime.UtcNow.AddDays(_jwtSettings.AccessTokenExpireDate);
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));
            SigningCredentials? signingCredentials = new SigningCredentials(key: securityKey, algorithm: SecurityAlgorithms.HmacSha256);

            // Pass Paramenter to Generate JWT Token
            var securityToken = new JwtSecurityToken(issuer: _jwtSettings.Issuer, audience: _jwtSettings.Audience, claims: claims
                                                    , notBefore: null, expires: expires, signingCredentials: signingCredentials);
            // Generate Access Token and RefreshToken then return them as a JwtAuthResult
            var accessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
            var refreshToken = GetRefreshToken(user.UserName);
            var userRefreshToken = new UserRefreshToken
            {
                AddedTime = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpireDate),
                IsUsed = false,
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
        private RefreshToken GetRefreshToken(string userName)
        {
            var refreshToken = new RefreshToken
            {
                UserName = userName,
                ExpireAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpireDate),
                TokenString = GenerateRefreshToken()
            };
            _userRefreshToken.AddOrUpdate(refreshToken.TokenString, refreshToken, (s, t) => refreshToken);
            return refreshToken;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new Byte[32];
            var randomNumberGenerate = RandomNumberGenerator.Create();
            randomNumberGenerate.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        private List<Claim> GetClaims(User user)
        {
            var result = new List<Claim>();
            if (user == null)
            {
                return result;
            }
            if (user.UserName != null)
            {
                result.Add(new Claim(nameof(UserClaimModel.UserName), user.UserName));
            }
            if (user.Email != null)
            {
                result.Add(new Claim(nameof(UserClaimModel.UserName), user.Email));
            }
            if (user.PhoneNumber != null)
            {
                result.Add(new Claim(nameof(UserClaimModel.UserName), user.PhoneNumber));
            }
            return result;
        }
        #endregion
    }
}
