using AutoMapper;
using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Authentication.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Featurres.Authentication.Commands.Handlers
{
    public class AuthenticationCommandHandler : ResponseHandler
                                                , IRequestHandler<SignInCommand, Response<JwtAuthResult>>
    {
        #region Fields
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IAuthenticationService _authenticationService;
        #endregion
        #region Constructors
        public AuthenticationCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IStringLocalizer<SharedResources> localizer, IAuthenticationService authenticationService) : base(localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _localizer = localizer;
            _authenticationService = authenticationService;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<JwtAuthResult>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            // Check if User Exist
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);
            if (user == null)
            {
                return BadRequest<JwtAuthResult>(_localizer[SharedResourcesKeys.UserNameOrPasswordIsIncorrect]);
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            // Check if Login Failed
            if (!result.Succeeded)
            {
                return BadRequest<JwtAuthResult>(_localizer[SharedResourcesKeys.UserNameOrPasswordIsIncorrect]);
            }
            // Generate Token
            var token = await _authenticationService.GetJWTToken(user);
            if (token == null)
            {
                return BadRequest<JwtAuthResult>(_localizer[SharedResourcesKeys.FailedToGenerateToken]);
            }
            return Success(token);
        }

        #endregion
    }
}
