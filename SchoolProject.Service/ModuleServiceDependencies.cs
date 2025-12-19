using Microsoft.Extensions.DependencyInjection;
using SchoolProject.Infrustructure.Abstracts;
using SchoolProject.Service.Abstracts;
using SchoolProject.Service.Implementation;

namespace SchoolProject.Service
{
    public static class ModuleServiceDependencies
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
        {
            services.AddTransient<IStudentService, StudentService>();
            return services;
        }
    }
}
