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
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IStudentRepository _studentRepository;
        #endregion
        #region Constructors
        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion
        #region Methods
        #region ReadMethods
        public async Task<List<Student>> GetStudentsListAsync()
        {
            return await _unitOfWork.Students.GetAllStudentsListAsync();
        }
        public async Task<Student?> GetStudentByIdWithIncludeAsync(int id)
        {
            var student = _unitOfWork.Students.GetTableNoTracking()
                                            .Include(s => s.Department)
                                            .FirstOrDefaultAsync(s => s.StudID == id);
            return await student;
        }
        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            var student = _unitOfWork.Students.GetTableNoTracking()
                                            .FirstOrDefaultAsync(s => s.StudID == id);
            return await student;
        }
        public async Task<bool> IsNameArExistsAsync(string nameAr)
        {
            return await _unitOfWork.Students.GetTableNoTracking().AnyAsync(s => s.NameAr == nameAr);
        }
        public async Task<bool> IsNameArExistsExcludeSelfAsync(string nameAr, int id)
        {
            return await _unitOfWork.Students.GetTableNoTracking().AnyAsync(s => s.NameAr == nameAr && s.StudID != id);
        }
        public async Task<bool> IsNameEnExistsExcludeSelfAsync(string nameEn, int id)
        {
            return await _unitOfWork.Students.GetTableNoTracking().AnyAsync(s => s.NameEn == nameEn && s.StudID != id);
        }
        public async Task<bool> IsIdExistsAsync(int id)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);
            return student != null;
        }
        public IQueryable<Student> GetStudentsQueryable()
        {
            return _unitOfWork.Students.GetTableNoTracking().Include(s => s.Department).AsQueryable();
        }
        public IQueryable<Student> FilterStudentPaginatedQuarable(StudentOrderingEnum orderBy, string? search = null)
        {
            var quarable = _unitOfWork.Students.GetTableNoTracking().Include(s => s.Department).AsQueryable();
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
        public async Task<bool> IsNameEnExistsAsync(string nameEn)
        {
            return await _unitOfWork.Students.GetTableNoTracking().AnyAsync(s => s.NameEn == nameEn);
        }
        #endregion
        #region WriteMethod
        public async Task<Student?> AddAsync(Student student)
        {
            bool result = false;
            Student? newStudent = null;
            // Add student if not exists
            try
            {
                newStudent = await _unitOfWork.Students.AddAsync(student);
                result = await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                result = false;
            }
            if (result && newStudent != null)
            {
                // Reload the inserted student including related data to ensure mapped response has populated fields
                var created = await _unitOfWork.Students.GetTableNoTracking()
                                    .Include(s => s.Department)
                                    .FirstOrDefaultAsync(s => s.StudID == newStudent.StudID);
                return created ?? newStudent;
            }
            return newStudent;
        }

        public async Task<bool> EditAsync(Student student)
        {
            bool result = false;
            // Update student if exists
            try
            {
                await _unitOfWork.Students.UpdateAsync(student);
                result = await _unitOfWork.SaveChangesAsync() > 0;
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
            var trans = _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.Students.DeleteAsync(student);
                var saveResult = await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                result = saveResult > 0;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                result = false;
            }
            return result;
        }


        #endregion

        #endregion

    }
}
