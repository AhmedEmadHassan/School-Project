using AutoMapper;
using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Students.Queries.Models;
using SchoolProject.Core.Featurres.Students.Queries.Response;
using SchoolProject.Core.Wrapper;
using SchoolProject.Core.Wrapper.SchoolProject.Core.Wrappers;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Abstracts;
using System.Linq.Expressions;

namespace SchoolProject.Core.Featurres.Students.Queries.Handlers
{
    public class StudentQueryHandler : ResponseHandler,
                                       IRequestHandler<GetStudentListQuery, Response<List<GetStudentListResponse>>>,
                                       IRequestHandler<GetStudentByIDQuery, Response<GetStudentResponse>>,
                                       IRequestHandler<GetStudentPaginatedListQuery, PaginatedResult<GetStudentPaginatedListResponse>>
    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        #endregion
        #region Constructors
        public StudentQueryHandler(IStudentService studentService, IMapper mapper)
        {
            _studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        #endregion

        #region Methods
        public async Task<Response<List<GetStudentListResponse>>> Handle(GetStudentListQuery request, CancellationToken cancellationToken)
        {
            var studentList = await _studentService.GetStudentsListAsync();
            var mappedStudentList = _mapper.Map<List<GetStudentListResponse>>(studentList);
            return Success(mappedStudentList);
        }

        public async Task<Response<GetStudentResponse>> Handle(GetStudentByIDQuery request, CancellationToken cancellationToken)
        {
            var student = await _studentService.GetStudentByIdWithIncludeAsync(request.Id);
            if (student == null)
            {
                return NotFound<GetStudentResponse>("Student not found");
            }
            var result = _mapper.Map<GetStudentResponse>(student);
            return Success(result);
        }

        public async Task<PaginatedResult<GetStudentPaginatedListResponse>> Handle(GetStudentPaginatedListQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Student, GetStudentPaginatedListResponse>> expression = s => new GetStudentPaginatedListResponse(s.StudID, s.Name, s.Address, s.Department.DName);
            var FilterQuerable = _studentService.FilterStudentPaginatedQuarable(request.OrderBy);
            var PaginatedList = await FilterQuerable.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return PaginatedList;
        }
        #endregion
    }
}
