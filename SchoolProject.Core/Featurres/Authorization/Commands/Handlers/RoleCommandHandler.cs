using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Authorization.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Featurres.Authorization.Commands.Handlers
{
    public class RoleCommandHandler : ResponseHandler
        , IRequestHandler<AddRoleCommand, Response<string>>
        , IRequestHandler<EditRoleCommand, Response<string>>
        , IRequestHandler<DeleteRoleCommand, Response<string>>
        , IRequestHandler<ManageUserRolesCommand, Response<string>>
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

        public async Task<Response<string>> Handle(EditRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _authorizationService.GetRoleByIDAsync(request.Id);
            if (role == null)
            {
                return NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFound]);
            }
            var result = await _authorizationService.EditRoleAsync(request.Id, request.Name);
            if (result)
            {
                return Success<string>(_stringLocalizer[SharedResourcesKeys.RoleEditedSuccessfully]);
            }
            else
            {
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToEditRole]);
            }
        }

        public async Task<Response<string>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _authorizationService.GetRoleByIDAsync(request.Id);
            if (role == null || role.Name == null)
            {
                return NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFound]);
            }
            var usersInRole = await _authorizationService.GetUsersInRoleAsync(role.Name);
            if (usersInRole.Any())
            {
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.RoleHasUsers]);
            }
            var result = await _authorizationService.DeleteRoleAsync(request.Id);
            if (result)
            {
                return Success<string>(_stringLocalizer[SharedResourcesKeys.DeletedSuccessfully]);
            }
            else
            {
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToDeleteRole]);
            }
        }

        public async Task<Response<string>> Handle(ManageUserRolesCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.ManageUserRolesAsync(request.UserId, request.RolesList);
            if (result)
            {
                return Success<string>(_stringLocalizer[SharedResourcesKeys.UserRolesUpdatedSuccessfully]);
            }
            else
            {
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToUpdateUserRoles]);
            }
        }
    }
}
