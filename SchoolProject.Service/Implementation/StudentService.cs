using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrustructure.Abstracts;
using SchoolProject.Service.Abstracts;

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
        public async Task<Student?> GetStudentByIdWithIncludeAsync(int id)
        {
            var student = _studentRepository.GetTableNoTracking()
                                            .Include(s => s.Department)
                                            .FirstOrDefaultAsync(s => s.StudID == id);
            return await student;
        }
        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            var student = _studentRepository.GetTableNoTracking()
                                            .FirstOrDefaultAsync(s => s.StudID == id);
            return await student;
        }

        public async Task<bool> AddAsync(Student student)
        {
            bool result = false;
            // Add student if not exists
            try
            {
                await _studentRepository.AddAsync(student);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        public async Task<bool> EditAsync(Student student)
        {
            bool result = false;
            // Update student if exists
            try
            {
                await _studentRepository.UpdateAsync(student);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        public async Task<bool> DeleteAsync(Student student)
        {
            bool result = false;
            var trans = _studentRepository.BeginTransaction();
            try
            {
                await _studentRepository.DeleteAsync(student);
                await trans.CommitAsync();
                result = true;
            }
            catch (Exception)
            {
                await trans.RollbackAsync();
                result = false;
            }
            return result;
        }

        public async Task<bool> IsNameExistsAsync(string name)
        {
            return await _studentRepository.GetTableNoTracking().AnyAsync(s => s.Name == name);
        }

        public async Task<bool> IsNameExistsExcludeSelfAsync(string name, int id)
        {
            return await _studentRepository.GetTableNoTracking().AnyAsync(s => s.Name == name && s.StudID != id);
        }
        public async Task<bool> IsIdExistsAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            return student != null;
        }
        #endregion

    }
}
