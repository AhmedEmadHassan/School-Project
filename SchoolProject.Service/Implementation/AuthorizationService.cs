using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.DTOs;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Infrustructure.Abstracts;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Service.Implementation
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public AuthorizationService(RoleManager<Role> roleManager, UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
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
        public async Task<ManageUserRolesResult?> GetManageUserRolesData(int UserId)
        {
            var userRoles = new List<UserRoleCheck>();
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            if (user == null)
                return null;
            var AllUserRoles = await _userManager.GetRolesAsync(user);
            List<Role> AllRoles = await _roleManager.Roles.ToListAsync();
            foreach (var item in AllRoles)
            {
                var role = new UserRoleCheck();
                role.RoleId = item.Id;
                role.RoleName = item.Name!;
                role.IsSelected = AllUserRoles.Contains(item.Name!);
                userRoles.Add(role);
            }
            var result = new ManageUserRolesResult();
            result.RolesList = userRoles;
            result.UserId = user.Id;
            //TODO: Check the result output
            return result;
        }
        public async Task<bool> ManageUserRolesAsync(int UserId, List<UserRoleCheck> RolesList)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Get User
                var user = await _userManager.FindByIdAsync(UserId.ToString());
                if (user == null)
                    return false;

                // Get User Roles
                var userRoles = await _userManager.GetRolesAsync(user);

                // Remove existing roles
                var result = await _userManager.RemoveFromRolesAsync(user, userRoles);
                if (!result.Succeeded)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return false;
                }

                // Map selected roles
                var selectedRoles = RolesList
                                    .Where(x => x.IsSelected)
                                    .Select(x => x.RoleName)
                                    .ToList();

                // Add new roles
                result = await _userManager.AddToRolesAsync(user, selectedRoles);

                if (!result.Succeeded)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return false;
                }

                // Commit transaction
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
            }
        }
    }
}
