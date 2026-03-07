using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Api.Base;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Authorization.Commands.Models;
using SchoolProject.Data.AppMetaData;

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
    }
}
