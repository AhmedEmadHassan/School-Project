using AutoMapper;
using SchoolProject.Core.Featurres.Students.Commands.Models;
using SchoolProject.Core.Featurres.Students.Queries.Response;
using SchoolProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolProject.Core.Mapping.Students
{
    public partial class StudentProfile : Profile
    {
        public void ConfigureAddStudentCommandMapping()
        {
            CreateMap<AddStudentCommand,Student>()
                .ForMember(dest => dest.DID, opt => opt.MapFrom(src => src.DepartmentId));
        }
    }
}
