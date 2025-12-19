using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Students.Queries.Models;
using SchoolProject.Core.Featurres.Students.Queries.Response;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.Api.Controllers
{
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public StudentsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet(Router.StudentsRouting.getStudentList)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<List<GetStudentListResponse>>))]
        [Produces("application/json")]
        public async Task<IActionResult> GetStudentList()
        {
            var response = await _mediator.Send(new GetStudentListQuery());
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpGet(Router.StudentsRouting.getStudentByID)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<GetStudentResponse>)), ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response<GetStudentResponse>))]
        [Produces("Application/json")]
        public async Task<IActionResult> GetStudentByID([FromRoute]int id)
        {
            var response = await _mediator.Send(new GetStudentByIDQuery(id));
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
