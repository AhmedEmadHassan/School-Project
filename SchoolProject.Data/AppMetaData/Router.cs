namespace SchoolProject.Data.AppMetaData
{
    public static class Router
    {
        public const string root = "api";
        public const string version = "v1";
        public const string baseUrl = root + "/" + version + "/";
        public static class StudentsRouting
        {
            public const string controller = "students";
            public const string prefix = baseUrl + controller + "/";
            #region Routes
            public const string getStudentList = prefix + "list";
            public const string getStudentByID = prefix + "{id}";
            public const string Create = prefix + "create";
            public const string Edit = prefix + "edit";
            public const string Delete = prefix + "delete/{id}";
            public const string Paginated = prefix + "paginated";
            #endregion
        }
        public static class DepartmentsRouting
        {
            public const string controller = "departments";
            public const string prefix = baseUrl + controller + "/";
            public const string getList = prefix + "list";
            public const string getByID = prefix + "{id}";
            public const string Create = prefix + "create";
            public const string Edit = prefix + "edit";
            public const string Delete = prefix + "delete/{id}";
            public const string Paginated = prefix + "paginated";

        }
        public static class UsersRouting
        {
            public const string controller = "users";
            public const string prefix = baseUrl + controller + "/";
            public const string getList = prefix + "list";
            public const string getByID = prefix + "{id}";
            public const string Create = prefix + "create";
            public const string Edit = prefix + "edit";
            public const string Delete = prefix + "delete/{id}";
            public const string Paginated = prefix + "paginated";
            public const string ChangePassword = prefix + "change-password";

        }
        public static class AuthenticationRouting
        {
            public const string controller = "authentication";
            public const string prefix = baseUrl + controller + "/";
            public const string getList = prefix + "list";
            public const string getByID = prefix + "{id}";
            public const string Create = prefix + "create";
            public const string SignIn = prefix + "SignIn";
            public const string Edit = prefix + "edit";
            public const string Delete = prefix + "delete/{id}";
            public const string Paginated = prefix + "paginated";
            public const string RefreshToken = prefix + "refresh-token";
            public const string ValidateToken = prefix + "validate-token";

        }
        public static class AuthorizationRouting
        {
            public const string controller = "authorization";
            public const string prefix = baseUrl + controller + "/";
            public const string getList = prefix + "list";
            public const string getByID = prefix + "{id}";
            public const string Create = prefix + "create";
            public const string SignIn = prefix + "SignIn";
            public const string Edit = prefix + "edit";
            public const string Delete = prefix + "delete/{id}";
            public const string Paginated = prefix + "paginated";
            public const string ManageUserRoles = prefix + "manage-user-roles/{id}";
        }
    }
}
