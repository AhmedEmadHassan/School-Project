using AutoMapper;
using SchoolProject.Core.Featurres.Authorization.Queries.Results;
using SchoolProject.Data.Entities.Identity;

namespace SchoolProject.Core.Mapping.Roles
{
    public partial class RoleProfile : Profile
    {
        public void GetRolesByIdMapping()
        {
            CreateMap<Role, GetRoleByIdResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}

