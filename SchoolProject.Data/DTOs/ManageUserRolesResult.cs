namespace SchoolProject.Data.DTOs
{
    public class ManageUserRolesResult
    {
        public int UserId { get; set; }
        public List<UserRole> RolesList { get; set; }
    }
    public class UserRole
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
