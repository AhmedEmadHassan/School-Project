using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Data.Entities.Identity;
using System.Reflection;

namespace SchoolProject.Infrustructure.Context
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, IdentityUserRole<int>, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> users { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<Department> departments { get; set; }
        public DbSet<Student> students { get; set; }
        public DbSet<Instructor> instructors { get; set; }
        public DbSet<DepartmentSubject> departmentSubjects { get; set; }
        public DbSet<Subject> subjects { get; set; }
        public DbSet<Ins_Subject> instructorSubjects { get; set; }
        public DbSet<UserRefreshToken> userRefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
