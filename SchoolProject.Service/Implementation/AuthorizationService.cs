

using Microsoft.AspNetCore.Identity;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Service.Implementation
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly RoleManager<Role> _roleManager;
        public AuthorizationService(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<bool> AddRoleAsync(string RoleName)
        {
            var role = new Role { Name = RoleName, NormalizedName = RoleName.ToUpper() };
            var result = await _roleManager.CreateAsync(role);
            return result.Succeeded;
        }
        public async Task<bool> IsRoleExist(string RoleName)
        {
            return await _roleManager.RoleExistsAsync(RoleName);
        }
    }
}
