using MediatR;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Api.Base;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Users.Commands.Models;
using SchoolProject.Core.Featurres.Users.Queries.Models;
using SchoolProject.Core.Featurres.Users.Queries.Response;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.Api.Controllers
{
    [ApiController]
    public class UsersController : AppControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost(Router.UsersRouting.Create)]
        public async Task<Response<string>> AddNewUser(AddUserCommand addUserCommand)
        {
            return await _mediator.Send(addUserCommand);
        }
        [HttpGet(Router.UsersRouting.Paginated)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<List<GetUserListResponse>>))]
        [Produces("application/json")]
        public async Task<IActionResult> Paginated([FromQuery] GetUserListQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
        [HttpGet(Router.UsersRouting.getByID)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<GetUserListResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response<GetUserListResponse>))]
        [Produces("Application/json")]
        public async Task<IActionResult> GetUserByID([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new GetUserByIdQuery(id)));
        }

        [HttpPut(Router.UsersRouting.Edit)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<GetUserByIdResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<string>))]
        [Produces("Application/json")]
        public async Task<IActionResult> Edit([FromBody] UpdateUserCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }
        [HttpDelete(Router.UsersRouting.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response<string>))]
        [Produces("Application/json")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new DeleteUserCommand(id)));
        }
    }
}
