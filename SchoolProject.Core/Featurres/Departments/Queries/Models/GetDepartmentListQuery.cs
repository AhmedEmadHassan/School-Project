using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Departments.Queries.Response;

namespace SchoolProject.Core.Featurres.Departments.Queries.Models
{
    public class GetDepartmentListQuery : IRequest<Response<List<GetDepartmentResponse>>>
    {

    }
}
