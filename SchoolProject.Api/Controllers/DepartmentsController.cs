using MediatR;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Departments.Queries.Models;
using SchoolProject.Core.Featurres.Departments.Queries.Response;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.Api.Controllers
{
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        #region Fields
        private readonly IMediator _mediator;
        #endregion
        #region Constructors
        public DepartmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        #endregion

        [HttpGet(Router.DepartmentsRouting.getList)]
        public async Task<Response<List<GetDepartmentResponse>>> GetAllDepartmentsListAsync()
        {
            return await _mediator.Send(new GetDepartmentListQuery());
        }
    }
}
