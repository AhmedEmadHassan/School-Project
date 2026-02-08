using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Data.Helpers;
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

        public async Task<bool> IsNameEnExistsAsync(string nameEn)
        {
            return await _studentRepository.GetTableNoTracking().AnyAsync(s => s.NameEn == nameEn);
        }
        public async Task<bool> IsNameArExistsAsync(string nameAr)
        {
            return await _studentRepository.GetTableNoTracking().AnyAsync(s => s.NameAr == nameAr);
        }
        public async Task<bool> IsNameArExistsExcludeSelfAsync(string nameAr, int id)
        {
            return await _studentRepository.GetTableNoTracking().AnyAsync(s => s.NameAr == nameAr && s.StudID != id);
        }
        public async Task<bool> IsNameEnExistsExcludeSelfAsync(string nameEn, int id)
        {
            return await _studentRepository.GetTableNoTracking().AnyAsync(s => s.NameEn == nameEn && s.StudID != id);
        }
        public async Task<bool> IsIdExistsAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            return student != null;
        }

        public IQueryable<Student> GetStudentsQueryable()
        {
            return _studentRepository.GetTableNoTracking().Include(s => s.Department).AsQueryable();
        }

        public IQueryable<Student> FilterStudentPaginatedQuarable(StudentOrderingEnum orderBy, string? search = null)
        {
            var quarable = _studentRepository.GetTableNoTracking().Include(s => s.Department).AsQueryable();
            switch (orderBy)
            {
                case StudentOrderingEnum.StudID:
                    quarable = quarable.OrderBy(s => s.StudID);
                    break;
                case StudentOrderingEnum.Name:
                    quarable = quarable.OrderBy(s => s.GetLocalized(s.NameEn, s.NameAr));
                    break;
                case StudentOrderingEnum.Address:
                    quarable = quarable.OrderBy(s => s.GetLocalized(s.AddressEn, s.AddressAr));
                    break;
                case StudentOrderingEnum.DepartmentName:
                    quarable = quarable.OrderBy(s => s.Department!.GetLocalized(s.Department.DNameEn, s.Department.DNameAr));
                    break;
                default:
                    quarable = quarable.OrderBy(s => s.StudID);
                    break;
            }
            if (search != null)
            {
                quarable = quarable.Where(s => s.GetLocalized(s.NameEn, s.NameAr).Contains(search) || s.GetLocalized(s.AddressEn, s.AddressAr).Contains(search) || s.Phone.Contains(search) || s.Department!.GetLocalized(s.Department.DNameEn, s.Department.DNameAr)!.Contains(search));
            }
            return quarable;
        }



        #endregion

    }
}
