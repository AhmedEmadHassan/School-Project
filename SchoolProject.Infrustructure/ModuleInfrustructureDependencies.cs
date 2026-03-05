using Microsoft.Extensions.DependencyInjection;
using SchoolProject.Infrustructure.Abstracts;
using SchoolProject.Infrustructure.Repositories;

namespace SchoolProject.Infrustructure
{
    public static class ModuleInfrustructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            IncludeRepositoriesInDependencyInjection(services);
            IncludeUnitOfWorkInDependencyInjection(services);
            return services;
        }
        private static void IncludeRepositoriesInDependencyInjection(IServiceCollection services)
        {
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<IInstructorRepository, InstructorRepository>();
            services.AddScoped<IUserRefreshTokenRepository, UserRefreshTokenRepository>();
        }
        private static void IncludeUnitOfWorkInDependencyInjection(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
