using AutoMapper;
using SchoolProject.Core.Featurres.Students.Queries.Response;
using SchoolProject.Data.Entities;

namespace SchoolProject.Core.Mapping.Students
{
    public partial class StudentProfile : Profile
    {
        public void ConfigureGetStudentResponseMapping()
        {
            CreateMap<Student, GetStudentResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.GetLocalized(src.NameEn, src.NameAr)))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.GetLocalized(src.AddressEn, src.AddressAr)))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.GetLocalized(src.Department.DNameEn, src.Department.DNameAr)));
        }
    }
}
