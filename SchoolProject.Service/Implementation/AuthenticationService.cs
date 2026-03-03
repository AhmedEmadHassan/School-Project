using Microsoft.IdentityModel.Tokens;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Service.Abstracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SchoolProject.Service.Implementation
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Fields
        private readonly JwtSettings _jwtSettings;
        #endregion
        #region Constructors
        public AuthenticationService(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }
        #endregion

        #region Methods
        public async Task<string> GetJWTToken(User user)
        {
            // Defining Parameters 
            string issuer = _jwtSettings.Issuer;
            string audience = _jwtSettings.Audience;
            List<Claim> claims = new List<Claim>()
            {
                new Claim(nameof(UserClaimModel.UserName),user.UserName),
                new Claim(nameof(UserClaimModel.Email),user.Email),
                new Claim(nameof(UserClaimModel.PhoneNumber),user.PhoneNumber)

            };
            DateTime? notBefore = null;
            DateTime? expires = DateTime.UtcNow.AddMinutes(30);

            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));
            string algorism = SecurityAlgorithms.HmacSha256;
            SigningCredentials? signingCredentials = new SigningCredentials(securityKey, algorism);

            // Pass Paramenter to Generate JWT Token
            var securityToken = new JwtSecurityToken(issuer, audience, claims, notBefore, expires, signingCredentials);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return accessToken;
        }
        #endregion
    }
}
