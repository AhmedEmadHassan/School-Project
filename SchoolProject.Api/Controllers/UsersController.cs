using MediatR;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Users.Commands.Models;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.Api.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
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
    }
}
