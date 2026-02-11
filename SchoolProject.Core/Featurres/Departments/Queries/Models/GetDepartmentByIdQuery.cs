using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Departments.Queries.Response;

namespace SchoolProject.Core.Featurres.Departments.Queries.Models
{
    public class GetDepartmentByIdQuery : IRequest<Response<GetDepartmentResponse>>
    {
        public int Id { get; set; }
        public GetDepartmentByIdQuery(int id)
        {
            Id = id;
        }
    }
}


