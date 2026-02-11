using AutoMapper;
using SchoolProject.Core.Featurres.Departments.Queries.Response;
using SchoolProject.Data.Entities;

namespace SchoolProject.Core.Mapping.Departments
{
    public partial class DepartmentProfile : Profile
    {
        public void ConfigureGetDepartmentResponseMapping()
        {
            CreateMap<Department, GetDepartmentResponse>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.DID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.GetLocalized(src.DNameEn, src.DNameAr)));
        }
    }
}
