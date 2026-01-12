using MediatR;
using SchoolProject.Core.Bases;

namespace SchoolProject.Core.Featurres.Students.Commands.Models
{
    public class EditStudentCommand : IRequest<Response<string>>
    {
        public int Id { get; set; } //Map this to StudID
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int DepartmentId { get; set; } // Map this to DID

    }
}
