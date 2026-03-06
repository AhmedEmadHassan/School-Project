namespace SchoolProject.Service.Abstracts
{
    public interface IAuthorizationService
    {
        public Task<bool> AddRoleAsync(string RoleName);
        public Task<bool> IsRoleExist(string RoleName);
    }
}
