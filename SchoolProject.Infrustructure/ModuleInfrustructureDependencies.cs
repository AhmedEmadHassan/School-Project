using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SchoolProject.Infrustructure.Abstracts;
using SchoolProject.Infrustructure.Context;
using SchoolProject.Infrustructure.Repositories;

namespace SchoolProject.Infrustructure
{
    public static class ModuleInfrustructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<IInstructorRepository, InstructorRepository>();

            // Unit of Work (depends on repositories and ApplicationDbContext)
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
