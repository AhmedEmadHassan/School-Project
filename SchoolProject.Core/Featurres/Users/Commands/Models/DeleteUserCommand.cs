using MediatR;
using SchoolProject.Core.Bases;

namespace SchoolProject.Core.Featurres.Users.Commands.Models
{
    public class DeleteUserCommand : IRequest<Response<string>>
    {
        public DeleteUserCommand(int Id)
        {
            this.Id = Id;
        }
        public int Id { get; set; }
    }
}
