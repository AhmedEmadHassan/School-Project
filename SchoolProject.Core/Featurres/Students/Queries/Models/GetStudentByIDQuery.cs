using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Students.Queries.Response;

namespace SchoolProject.Core.Featurres.Students.Queries.Models
{
    public class GetStudentByIDQuery : IRequest<Response<GetStudentResponse>>
    {
        public int Id { get; set; }
        public GetStudentByIDQuery(int id)
        {
            Id = id;
        }
    }
}
