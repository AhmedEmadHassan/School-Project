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
            #endregion
        }
    }
}
