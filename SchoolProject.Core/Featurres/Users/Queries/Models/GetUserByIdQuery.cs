using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Users.Queries.Response;

namespace SchoolProject.Core.Featurres.Users.Queries.Models
{
    public class GetUserByIdQuery : IRequest<Response<GetUserByIdResponse>>
    {
        public int Id { get; set; }
        public GetUserByIdQuery(int id)
        {
            Id = id;
        }
    }
}
