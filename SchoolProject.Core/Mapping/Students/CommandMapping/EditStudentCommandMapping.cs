using AutoMapper;
using SchoolProject.Core.Featurres.Students.Commands.Models;
using SchoolProject.Data.Entities;

namespace SchoolProject.Core.Mapping.Students
{
    public partial class StudentProfile : Profile
    {
        public void ConfigureEditStudentCommandMapping()
        {
            CreateMap<EditStudentCommand, Student>()
                .ForMember(dest => dest.DID, opt => opt.MapFrom(src => src.DepartmentId))
                .ForMember(dest => dest.StudID, opt => opt.MapFrom(src => src.Id));
        }
    }
}