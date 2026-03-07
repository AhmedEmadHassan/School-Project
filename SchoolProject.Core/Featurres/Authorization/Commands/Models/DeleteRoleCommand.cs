using MediatR;
using SchoolProject.Core.Bases;

namespace SchoolProject.Core.Featurres.Authorization.Commands.Models
{
    public class DeleteRoleCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeleteRoleCommand(int Id)
        {
            this.Id = Id;
        }
    }
}
