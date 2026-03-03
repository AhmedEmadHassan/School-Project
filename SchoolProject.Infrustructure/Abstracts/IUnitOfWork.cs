using Microsoft.EntityFrameworkCore.Storage;

namespace SchoolProject.Infrustructure.Abstracts
{
    public interface IUnitOfWork : IDisposable
    {
        // Your repositories
        IDepartmentRepository Departments { get; }
        IStudentRepository Students { get; }
        ISubjectRepository Subjects { get; }
        IInstructorRepository Instructors { get; }
        IUserRefreshTokenRepository userRefreshToken { get; }

        // Save all changes
        Task<int> SaveChangesAsync();

        // Transaction control
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
