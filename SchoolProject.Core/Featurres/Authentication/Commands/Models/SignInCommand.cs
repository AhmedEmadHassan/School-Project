using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Data.Helpers;

namespace SchoolProject.Core.Featurres.Authentication.Commands.Models
{
    public class SignInCommand : IRequest<Response<JwtAuthResult>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}
