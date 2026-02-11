using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Departments.Queries.Models;
using SchoolProject.Core.Featurres.Departments.Queries.Response;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Featurres.Departments.Queries.Handlers
{
    public class DepartmentQueryHandler : ResponseHandler, IRequestHandler<GetDepartmentListQuery, Response<List<GetDepartmentResponse>>>
    {
        IStringLocalizer<SharedResources> _localizer;
        IDepartmentService _departmentService;
        IMapper _mapper;
        public DepartmentQueryHandler(IStringLocalizer<SharedResources> localizer, IDepartmentService departmentService, IMapper mapper) : base(localizer)
        {
            _localizer = localizer;
            _departmentService = departmentService;
            _mapper = mapper;
        }

        public async Task<Response<List<GetDepartmentResponse>>> Handle(GetDepartmentListQuery request, CancellationToken cancellationToken)
        {
            var departments = await _departmentService.GetDepartmentsListWithInstructorsAsync();
            if (departments is null)
                return NotFound<List<GetDepartmentResponse>>(_localizer[SharedResourcesKeys.NotFound]);
            var mappedDepartments = _mapper.Map<List<GetDepartmentResponse>>(departments);
            return Success(mappedDepartments);

        }
    }
}
