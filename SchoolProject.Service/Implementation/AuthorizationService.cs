

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Service.Implementation
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        public AuthorizationService(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
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
        public async Task<bool> EditRoleAsync(int Id, string RoleName)
        {
            var role = await _roleManager.FindByIdAsync(Id.ToString());
            if (role == null)
            {
                return false;
            }
            role.Name = RoleName;
            role.NormalizedName = RoleName.ToUpper();
            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }
        public async Task<Role?> GetRoleByIDAsync(int Id)
        {
            return await _roleManager.FindByIdAsync(Id.ToString());
        }
        public async Task<Role?> GetRoleByNameAsync(string Name)
        {
            return await _roleManager.FindByNameAsync(Name);
        }
        public async Task<bool> DeleteRoleAsync(int Id)
        {
            var role = await _roleManager.FindByIdAsync(Id.ToString());
            if (role == null)
            {
                return false;
            }
            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }
        public async Task<List<User>> GetUsersInRoleAsync(string RoleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(RoleName);
            return users.ToList();
        }
        public async Task<List<Role>> GetRolesListAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }
        public async Task<Role?> GetRoleByIdAsync(int Id)
        {
            return await _roleManager.FindByIdAsync(Id.ToString());

        }
    }
}
