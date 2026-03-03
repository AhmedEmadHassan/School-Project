using Microsoft.EntityFrameworkCore.Storage;
using SchoolProject.Infrustructure.Abstracts;
using SchoolProject.Infrustructure.Context;

namespace SchoolProject.Infrustructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        // Repositories
        public IDepartmentRepository Departments { get; }
        public IStudentRepository Students { get; }
        public ISubjectRepository Subjects { get; }
        public IInstructorRepository Instructors { get; }
        public IUserRefreshTokenRepository userRefreshToken { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            IDepartmentRepository departmentRepository,
            IStudentRepository studentRepository,
            ISubjectRepository subjectRepository,
            IInstructorRepository instructorRepository,
            IUserRefreshTokenRepository userRefreshTokenRepository)
        {
            _context = context;
            Departments = departmentRepository;
            Students = studentRepository;
            Subjects = subjectRepository;
            Instructors = instructorRepository;
            userRefreshToken = userRefreshTokenRepository;
        }

        // Save all tracked changes
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Start a new transaction
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_transaction != null) return _transaction;

            _transaction = await _context.Database.BeginTransactionAsync();
            return _transaction;
        }

        // Commit transaction and save changes
        public async Task CommitTransactionAsync()
        {
            if (_transaction == null) return;

            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        // Rollback transaction
        public async Task RollbackTransactionAsync()
        {
            if (_transaction == null) return;

            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        // Dispose DbContext and transaction
        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
