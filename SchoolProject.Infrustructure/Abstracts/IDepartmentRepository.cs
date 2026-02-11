using SchoolProject.Data.Entities;
using SchoolProject.Infrustructure.Infrastructure_Bases;

namespace SchoolProject.Infrustructure.Abstracts
{
    public interface IDepartmentRepository : IGenericRepositoryAsync<Department>
    {
        public Task<List<Department>> GetAllDepartmentsListAsync();
        public Task<List<Department>> GetAllDepartmentsListWithIncludeInstructorsAsync();
    }
}
