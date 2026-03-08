using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Authorization.Queries.Results;

namespace SchoolProject.Core.Featurres.Authorization.Queries.Models
{
    public class GetRolesListQuery : IRequest<Response<List<GetRolesListResult>>>
    {

    }
}
