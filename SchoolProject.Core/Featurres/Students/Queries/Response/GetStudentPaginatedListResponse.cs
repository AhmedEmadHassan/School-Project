namespace SchoolProject.Core.Featurres.Students.Queries.Response
{
    public class GetStudentPaginatedListResponse
    {
        public int StudID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string? DepartmentName { get; set; }
        public GetStudentPaginatedListResponse(int StudID, string Name, string Address, string? DepartmentName = null)
        {
            this.StudID = StudID;
            this.Name = Name;
            this.Address = Address;
            this.DepartmentName = DepartmentName;
        }
    }
}
