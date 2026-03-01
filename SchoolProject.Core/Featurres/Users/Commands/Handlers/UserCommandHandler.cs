using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Users.Commands.Models;
using SchoolProject.Core.Featurres.Users.Queries.Response;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities.Identity;

namespace SchoolProject.Core.Featurres.Users.Commands.Handlers
{
    public class UserCommandHandler : ResponseHandler
                                    , IRequestHandler<AddUserCommand, Response<string>>
                                    , IRequestHandler<UpdateUserCommand, Response<GetUserByIdResponse>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        #endregion

        #region Constructors
        public UserCommandHandler(IStringLocalizer<SharedResources> localizer, IMapper mapper, UserManager<User> userManager) : base(localizer)
        {
            _localizer = localizer;
            _mapper = mapper;
            _userManager = userManager;
        }
        #endregion
        #region Handlers
        public async Task<Response<string>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.EmailExists]);
            }
            user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null)
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.UsernameAlreadyExists]);
            }
            var CreatedUser = _mapper.Map<User>(request);
            var result = await _userManager.CreateAsync(CreatedUser, request.Password);
            if (result.Succeeded)
            {
                return Created<string>(_localizer[SharedResourcesKeys.CreatedSuccessfully]);
            }
            else
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.BadRequest]);
            }
        }

        public async Task<Response<GetUserByIdResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            // Check if User Exists
            var User = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.Id);
            if (User == null)
            {
                return NotFound<GetUserByIdResponse>("User Not Found");
            }
            // Mapping
            var newUser = _mapper.Map(request, User);
            // Update
            var result = await _userManager.UpdateAsync(newUser);
            // Result 
            if (!result.Succeeded)
            {
                return BadRequest<GetUserByIdResponse>(_localizer[SharedResourcesKeys.FailedToEdit]);
            }

            return Success<GetUserByIdResponse>(_mapper.Map<GetUserByIdResponse>(newUser));
        }
        #endregion

    }
}
