using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Data.DTOs;

namespace SchoolProject.Core.Featurres.Authorization.Commands.Models
{
    public class ManageUserRolesCommand : IRequest<Response<string>>
    {
        public int UserId { get; set; }
        public List<UserRoleCheck> RolesList { get; set; }
    }
}
