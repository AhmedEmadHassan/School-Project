using Microsoft.EntityFrameworkCore;
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
        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            var student = _studentRepository.GetTableNoTracking()
                                            .Include(s => s.Department)
                                            .FirstOrDefaultAsync(s => s.StudID == id);
            return await student;
        }

        public async Task<bool> AddAsync(Student student)
        {
            // Check if name already exists
            var studentResult = _studentRepository.GetTableNoTracking().Where(x => x.Name.Equals(student.Name)).FirstOrDefault();
            if (studentResult != null)
            {
                return false;
            }
            // Add student if not exists
            try
            {
                await _studentRepository.AddAsync(student);
                return true;

            }
            catch (Exception)
            {
                return false;
            }

        }

        #endregion

    }
}
