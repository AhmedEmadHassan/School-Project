using MediatR;
using SchoolProject.Core.Bases;

namespace SchoolProject.Core.Featurres.Students.Commands.Models
{
    public class AddStudentCommand : IRequest<Response<string>>
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string AddressEn { get; set; }
        public string AddressAr { get; set; }
        public string Phone { get; set; }
        public int DepartmentId { get; set; } // Map this to DID
    }
}
