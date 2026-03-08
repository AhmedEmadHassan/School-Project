using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Authorization.Queries.Results;

namespace SchoolProject.Core.Featurres.Authorization.Queries.Models
{
    public class GetRoleByIdQuery : IRequest<Response<GetRoleByIdResult>>
    {
        public GetRoleByIdQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
