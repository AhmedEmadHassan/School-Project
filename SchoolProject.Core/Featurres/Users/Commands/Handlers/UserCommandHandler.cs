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
                                    , IRequestHandler<DeleteUserCommand, Response<string>>
                                    , IRequestHandler<ChangeUserPasswordCommand, Response<string>>
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

        public async Task<Response<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            // Get User
            var User = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.Id);
            // Check if found
            if (User == null)
            {
                return NotFound<string>(_localizer[SharedResourcesKeys.NotFound]);
            }
            // Delete User
            var result = await _userManager.DeleteAsync(User);
            if (!result.Succeeded)
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.FailedToDelete]);
            }
            return Success<string>(_localizer[SharedResourcesKeys.DeletedSuccessfully]);
        }

        public async Task<Response<string>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            // Check that the NewPassword == ConfirmPassword
            if (request.NewPassword != request.ConfirmPassword)
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.ThePasswordAndConfirmPasswordDontMatch]);
            }
            // Get User By ID
            var User = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.Id);
            // Check if User Found
            if (User == null)
            {
                return NotFound<string>(_localizer[SharedResourcesKeys.NotFound]);
            }
            // Change Password Service
            var result = await _userManager.ChangePasswordAsync(User, request.CurrentPassword, request.NewPassword);
            // Check if changed Successfully
            if (!result.Succeeded)
            {
                return BadRequest<string>("Failed to change Password");
            }
            //Return the Response
            return Success<string>("Password Changed Successfully");
        }
        #endregion

    }
}
