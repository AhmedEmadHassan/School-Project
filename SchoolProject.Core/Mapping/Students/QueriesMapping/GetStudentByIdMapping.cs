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
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DName));
        }
    }
}
