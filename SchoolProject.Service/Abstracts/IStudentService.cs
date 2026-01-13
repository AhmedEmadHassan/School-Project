using SchoolProject.Data.Entities;

namespace SchoolProject.Service.Abstracts
{
    public interface IStudentService
    {
        public Task<List<Student>> GetStudentsListAsync();
        public Task<Student?> GetStudentByIdAsync(int id);
        public Task<Student?> GetStudentByIdWithIncludeAsync(int id);
        public Task<bool> AddAsync(Student student);
        public Task<bool> EditAsync(Student student);
        public Task<bool> IsNameExistsAsync(string name);
        public Task<bool> IsIdExistsAsync(int id);
        public Task<bool> IsNameExistsExcludeSelfAsync(string name, int id);
        public Task<bool> DeleteAsync(Student student);
        public IQueryable<Student> GetStudentsQueryable();
        public IQueryable<Student> FilterStudentPaginatedQuarable(string? search);
    }
}
