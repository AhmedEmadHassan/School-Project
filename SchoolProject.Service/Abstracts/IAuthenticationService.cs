using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using System.IdentityModel.Tokens.Jwt;

namespace SchoolProject.Service.Abstracts
{
    public interface IAuthenticationService
    {
        public Task<JwtAuthResult> GenerateJwtAuthResultAsync(User user);
        public Task<JwtAuthResult> GenerateNewAccessTokenFromRefreshAsync(User user, JwtSecurityToken jwtToken, DateTime? expiryDate, string refreshToken);
        public Task<string> ValidateAccessTokenAsync(string AccessToken);
        public Task<(string, DateTime?)> ValidateRefreshTokenAsync(JwtSecurityToken jwtToken, string accessToken, string refreshToken);
        public JwtSecurityToken ReadJwtToken(string accessToken);
        public Task<bool> UpdateRefreshTokenWithNewAccessTokenAsync(string accessToken, string refreshToken);
    }
}
