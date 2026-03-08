using SchoolProject.Data.Entities.Identity;

namespace SchoolProject.Service.Abstracts
{
    public interface IAuthorizationService
    {
        public Task<bool> AddRoleAsync(string RoleName);
        public Task<bool> EditRoleAsync(int Id, string RoleName);
        public Task<Role?> GetRoleByIDAsync(int Id);
        public Task<Role?> GetRoleByNameAsync(string Name);
        public Task<bool> IsRoleExist(string RoleName);
        public Task<bool> DeleteRoleAsync(int Id);
        public Task<List<User>> GetUsersInRoleAsync(string RoleName);
        public Task<List<Role>> GetRolesListAsync();
        public Task<Role?> GetRoleByIdAsync(int Id);

    }
}
