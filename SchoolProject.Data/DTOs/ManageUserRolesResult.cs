namespace SchoolProject.Data.DTOs
{
    public class ManageUserRolesResult
    {
        public int UserId { get; set; }
        public List<UserRoleCheck> RolesList { get; set; }
    }
}
