using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Authorization.Queries.Models;
using SchoolProject.Core.Featurres.Authorization.Queries.Results;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Featurres.Authorization.Queries.Handlers
{
    public class RoleQueryHandler : ResponseHandler
                                    , IRequestHandler<GetRolesListQuery, Response<List<GetRolesListResult>>>
                                    , IRequestHandler<GetRoleByIdQuery, Response<GetRoleByIdResult>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;
        #endregion
        #region Constructors
        public RoleQueryHandler(IStringLocalizer<SharedResources> stringLocalizer, IAuthorizationService authorizationService, IMapper mapper) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _authorizationService = authorizationService;
            _mapper = mapper;
        }
        #endregion
        #region Handle Methods
        public async Task<Response<List<GetRolesListResult>>> Handle(GetRolesListQuery request, CancellationToken cancellationToken)
        {
            var roles = await _authorizationService.GetRolesListAsync();
            var response = _mapper.Map<List<GetRolesListResult>>(roles);
            return Success(response);
        }

        public async Task<Response<GetRoleByIdResult>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _authorizationService.GetRoleByIdAsync(request.Id);
            if (role == null)
            {
                return NotFound<GetRoleByIdResult>(_stringLocalizer[SharedResourcesKeys.RoleNotFound]);
            }
            var response = _mapper.Map<GetRoleByIdResult>(role);
            return Success(response);
        }
        #endregion

    }
}
