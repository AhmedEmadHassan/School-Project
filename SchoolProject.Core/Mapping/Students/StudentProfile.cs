using AutoMapper;
using SchoolProject.Core.Featurres.Students.Queries.Response;
using SchoolProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolProject.Core.Mapping.Students
{
    public partial class StudentProfile : Profile
    {
        public StudentProfile() 
        {
            ConfigureStudentResponseMapping();
        }
    }
}
