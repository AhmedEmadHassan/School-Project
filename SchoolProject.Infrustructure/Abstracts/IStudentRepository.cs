using SchoolProject.Data.Entities;
using SchoolProject.Infrustructure.Infrastructure_Bases;

namespace SchoolProject.Infrustructure.Abstracts
{
    public interface IStudentRepository : IGenericRepositoryAsync<Student>
    {
        public Task<List<Student>> GetAllStudentsListAsync();
        public Task<Student?> GetStudentByIdAsync(int id);

    }
}
