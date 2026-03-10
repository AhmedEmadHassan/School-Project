using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Api.Base;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Authorization.Commands.Models;
using SchoolProject.Core.Featurres.Authorization.Queries.Models;
using SchoolProject.Core.Featurres.Authorization.Queries.Results;
using SchoolProject.Data.AppMetaData;
using SchoolProject.Data.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace SchoolProject.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AuthorizationController : AppControllerBase
    {
        [HttpPost(Router.AuthorizationRouting.Create)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Produces("Application/json")]
        public async Task<IActionResult> Create([FromForm] AddRoleCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }
        [HttpPost(Router.AuthorizationRouting.Edit)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Produces("Application/json")]
        public async Task<IActionResult> Edit([FromForm] EditRoleCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }
        [HttpDelete(Router.AuthorizationRouting.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Produces("Application/json")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new DeleteRoleCommand(id)));
        }
        [HttpGet(Router.AuthorizationRouting.getList)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<List<GetRolesListResult>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Produces("Application/json")]
        public async Task<IActionResult> GetList()
        {
            return NewResult(await Mediator.Send(new GetRolesListQuery()));
        }
        [HttpGet(Router.AuthorizationRouting.getByID)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<GetRoleByIdResult>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [SwaggerOperation(Summary = "Get Role By Id", Description = "Get Role By Id")]
        [Produces("Application/json")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new GetRoleByIdQuery(id)));
        }
        [HttpGet(Router.AuthorizationRouting.ManageUserRoles)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<ManageUserRolesResult>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [SwaggerOperation(Description = "Manage user Roles Result")]
        [Produces("Application/json")]
        public async Task<IActionResult> ManageUserRolesResult([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new ManageUserRolesQuery(id)));
        }
        [HttpPost(Router.AuthorizationRouting.SaveUserRoles)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [SwaggerOperation(Description = "Manage user Roles Result")]
        [Produces("Application/json")]
        public async Task<IActionResult> ManageUserRolesResult([FromBody] ManageUserRolesCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }
    }
}
