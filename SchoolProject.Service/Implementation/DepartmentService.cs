using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrustructure.Abstracts;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Service.Implementation
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Department?> GetDepartmentByIdAsync(int id)
        {
            var department = await _unitOfWork.Departments.GetTableNoTracking().Where(x => x.DID.Equals(id))
                                                        .Include(x => x.DepartmentSubjects).ThenInclude(x => x.Subjects)
                                                        .Include(x => x.Instructors)
                                                        .FirstOrDefaultAsync();
            return department;
        }
    }
}
