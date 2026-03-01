using MediatR;
using SchoolProject.Core.Wrapper.SchoolProject.Core.Wrappers;

namespace SchoolProject.Core.Featurres.Users.Queries.Models
{
    public class GetUserListQuery : IRequest<PaginatedResult<GetUserListResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        //public StudentOrderingEnum OrderBy { get; set; }
        //public string? Search { get; set; }
    }
}
