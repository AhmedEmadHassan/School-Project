namespace SchoolProject.Core.Featurres.Students.Queries.Response
{
    public class GetStudentResponse
    {

        public int StudID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string? DepartmentName { get; set; }
    }
}
