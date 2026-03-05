using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Authentication.Queries.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;


namespace SchoolProject.Core.Featurres.Authentication.Queries.Handlers
{
    internal class AuthenticationQueryHandler : ResponseHandler, IRequestHandler<AuthorizeUserQuery, Response<string>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IAuthenticationService _authenticationService;
        #endregion
        #region Constructors
        public AuthenticationQueryHandler(IStringLocalizer<SharedResources> localizer, IAuthenticationService authenticationService) : base(localizer)
        {
            _localizer = localizer;
            _authenticationService = authenticationService;
        }
        #endregion

        #region Constructors
        public async Task<Response<string>> Handle(AuthorizeUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.ValidateAccessTokenAsync(request.AccessToken);
            if (result == "NotExpired")
                return Success<string>(result);
            return Unauthorized<string>(_localizer[SharedResourcesKeys.TokenIsExpired]);
        }
        #endregion
    }
}
