using Microsoft.AspNetCore.Mvc;
using SchoolProject.Api.Base;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Authentication.Commands.Models;
using SchoolProject.Core.Featurres.Authentication.Queries.Models;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.Api.Controllers
{
    [ApiController]
    public class AuthenticationController : AppControllerBase
    {
        [HttpPost(Router.AuthenticationRouting.SignIn)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<string>))]
        [Produces("Application/json")]
        public async Task<IActionResult> Create([FromForm] SignInCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }
        [HttpPost(Router.AuthenticationRouting.RefreshToken)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<string>))]
        [Produces("Application/json")]
        public async Task<IActionResult> RefreshToken([FromForm] RefreshTokenCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }
        [HttpGet(Router.AuthenticationRouting.ValidateToken)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Response<string>))]
        [Produces("Application/json")]
        public async Task<IActionResult> ValidateToken([FromQuery] AuthorizeUserQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }
    }
}
