using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Api.Base;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Authorization.Commands.Models;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class AuthorizationController : AppControllerBase
    {
        [HttpPost(Router.AuthorizationRouting.Create)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces("Application/json")]
        public async Task<IActionResult> Create([FromForm] AddRoleCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }
    }
}
