using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Authorization.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Featurres.Authorization.Commands.Handlers
{
    public class RoleCommandHandler : ResponseHandler,
                                        IRequestHandler<AddRoleCommand, Response<string>>
    {
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IAuthorizationService _authorizationService;
        public RoleCommandHandler(IStringLocalizer<SharedResources> stringLocalizer, IAuthorizationService authorizationService) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _authorizationService = authorizationService;
        }
        public async Task<Response<string>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.AddRoleAsync(request.RoleName);
            if (result)
            {
                return Success<string>(_stringLocalizer[SharedResourcesKeys.RoleAddedSuccessfully]);
            }
            else
            {
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToAddRole]);
            }
        }
    }
}
