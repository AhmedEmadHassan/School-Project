using MediatR;
using SchoolProject.Core.Featurres.Students.Queries.Response;
using SchoolProject.Core.Wrapper.SchoolProject.Core.Wrappers;

namespace SchoolProject.Core.Featurres.Students.Queries.Models
{
    public class GetStudentPaginatedListQuery : IRequest<PaginatedResult<GetStudentPaginatedListResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string[]? OrderBy { get; set; }
        public string? Search { get; set; }
    }
}
