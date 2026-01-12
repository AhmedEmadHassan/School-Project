using MediatR;
using SchoolProject.Core.Bases;

namespace SchoolProject.Core.Featurres.Students.Commands.Models
{
    public class DeleteStudentCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeleteStudentCommand(int id)
        {
            Id = id;
        }

    }
}
