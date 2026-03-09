using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Data.DTOs;

namespace SchoolProject.Core.Featurres.Authorization.Queries.Models
{
    public class ManageUserRolesQuery : IRequest<Response<ManageUserRolesResult>>
    {
        public ManageUserRolesQuery(int id)
        {
            UserId = id;
        }
        public int UserId { get; set; }

    }

}
