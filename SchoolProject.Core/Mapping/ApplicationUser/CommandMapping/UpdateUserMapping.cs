using AutoMapper;
using SchoolProject.Core.Featurres.Users.Commands.Models;
using SchoolProject.Data.Entities.Identity;

namespace SchoolProject.Core.Mapping.ApplicationUser
{
    public partial class ApplicationUserProfile : Profile
    {
        public void UpdateUserMapping()
        {
            // Map AddUserCommand to Identity User entity
            CreateMap<UpdateUserCommand, User>();
        }

    }
}