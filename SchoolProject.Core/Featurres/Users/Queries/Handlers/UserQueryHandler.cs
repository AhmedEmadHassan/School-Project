using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Users.Queries.Models;
using SchoolProject.Core.Featurres.Users.Queries.Response;
using SchoolProject.Core.Resources;
using SchoolProject.Core.Wrapper;
using SchoolProject.Core.Wrapper.SchoolProject.Core.Wrappers;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Featurres.Users.Queries.Handlers
{
    public class UserQueryHandler : ResponseHandler
                                    , IRequestHandler<GetUserListQuery, PaginatedResult<GetUserListResponse>>
                                    , IRequestHandler<GetUserByIdQuery, Response<GetUserByIdResponse>>
    {

        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly UserManager<User> _userManager;

        public UserQueryHandler(IStudentService studentService, IMapper mapper, IStringLocalizer<SharedResources> localizer, UserManager<User> userManager) : base(localizer)
        {
            _studentService = studentService;
            _mapper = mapper;
            _localizer = localizer;
            _userManager = userManager;
        }

        public async Task<PaginatedResult<GetUserListResponse>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var UsersQuery = _userManager.Users.AsQueryable();
            var paginatedUsers = await _mapper.ProjectTo<GetUserListResponse>(UsersQuery)
                                              .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedUsers;
        }

        public async Task<Response<GetUserByIdResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.Id);
            if (user == null)
            {
                return NotFound<GetUserByIdResponse>(_localizer[SharedResourcesKeys.NotFound]);
            }
            var result = _mapper.Map<GetUserByIdResponse>(user);
            return Success(result);
        }
    }
}