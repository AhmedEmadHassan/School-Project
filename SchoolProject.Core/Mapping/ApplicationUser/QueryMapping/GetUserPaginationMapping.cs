using AutoMapper;
using SchoolProject.Core.Featurres.Users.Queries.Models;
using SchoolProject.Data.Entities.Identity;

namespace SchoolProject.Core.Mapping.ApplicationUser
{
    public partial class ApplicationUserProfile : Profile
    {
        public void GetUserPaginationMapping()
        {
            CreateMap<User, GetUserListResponse>();
        }
    }
}
