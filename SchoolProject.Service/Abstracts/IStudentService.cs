using SchoolProject.Data.Entities;

namespace SchoolProject.Service.Abstracts
{
    public interface IStudentService
    {
        public Task<List<Student>> GetStudentsListAsync();
        public Task<Student?> GetStudentByIdAsync(int id);
        public Task<bool> AddAsync(Student student);
        public Task<bool> EditAsync(Student student);
        public Task<bool> IsNameExists(string name);
        public Task<bool> IsNameExistsExcludeSelf(string name, int id);
    }
}
