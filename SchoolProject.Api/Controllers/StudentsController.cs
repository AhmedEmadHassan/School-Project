using Microsoft.AspNetCore.Mvc;
using SchoolProject.Api.Base;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Students.Commands.Models;
using SchoolProject.Core.Featurres.Students.Queries.Models;
using SchoolProject.Core.Featurres.Students.Queries.Response;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.Api.Controllers
{
    [ApiController]
    public class StudentsController : AppControllerBase
    {
        [HttpGet(Router.StudentsRouting.getStudentList)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<List<GetStudentListResponse>>))]
        [Produces("application/json")]
        public async Task<IActionResult> GetStudentList()
        {
            return NewResult(await Mediator.Send(new GetStudentListQuery()));
        }
        [HttpGet(Router.StudentsRouting.getStudentByID)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<GetStudentResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response<GetStudentResponse>))]
        [Produces("Application/json")]
        public async Task<IActionResult> GetStudentByID([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new GetStudentByIDQuery(id)));
        }

        [HttpPost(Router.StudentsRouting.Create)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<string>))]
        [Produces("Application/json")]
        public async Task<IActionResult> Create([FromBody] AddStudentCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }

        [HttpPut(Router.StudentsRouting.Edit)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<string>))]
        [Produces("Application/json")]
        public async Task<IActionResult> Edit([FromBody] EditStudentCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }
    }
}
