using SchoolProject.Data.Entities;
using SchoolProject.Infrustructure.Abstracts;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolProject.Service.Implementation
{

    public class StudentService : IStudentService
    {
        #region Fields
        private readonly IStudentRepository _studentRepository;
        #endregion
        #region Constructors
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        #endregion
        #region Methods
        public async Task<List<Student>> GetStudentsListAsync()
        {
            return await _studentRepository.GetAllStudentsListAsync();
        }
        #endregion

    }
}
