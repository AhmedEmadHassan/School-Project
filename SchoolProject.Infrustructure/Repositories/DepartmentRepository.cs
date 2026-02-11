using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrustructure.Abstracts;
using SchoolProject.Infrustructure.Context;
using SchoolProject.Infrustructure.Infrastructure_Bases;

namespace SchoolProject.Infrustructure.Repositories
{
    public class DepartmentRepository : GenericRepositoryAsync<Department>, IDepartmentRepository
    {
        #region Fields
        private readonly DbSet<Department> _departments;
        #endregion
        #region Constructors
        public DepartmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _departments = dbContext.Set<Department>();
        }

        public Task<List<Department>> GetAllDepartmentsListWithIncludeInstructorsAsync()
        {
            return _departments.Include(d => d.Instructors)
                               .ToListAsync();
        }
        public Task<List<Department>> GetAllDepartmentsListWithIncludeAllAsync()
        {
            return _departments.Include(x => x.DepartmentSubjects)
                                            .ThenInclude(x => x.Subjects)
                                            .Include(x => x.Instructors)
                                            .ToListAsync();
        }
        public Task<List<Department>> GetAllDepartmentsListAsync()
        {
            return _departments.ToListAsync();
        }

        public async Task<Department?> GetDepartmentByIdAsync(int id)
        {
            return await _departments.FindAsync(id);
        }
        #endregion
        #region Methods

        #endregion
    }
}
