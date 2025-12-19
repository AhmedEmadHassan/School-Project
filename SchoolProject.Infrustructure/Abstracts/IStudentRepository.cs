using SchoolProject.Data.Entities;
using SchoolProject.Infrustructure.Infrastructure_Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolProject.Infrustructure.Abstracts
{
    public interface IStudentRepository : IGenericRepositoryAsync<Student>
    {   
        public Task<List<Student>> GetAllStudentsListAsync();
        
    }
}
