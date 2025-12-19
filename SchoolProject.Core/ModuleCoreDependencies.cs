using Microsoft.Extensions.DependencyInjection;
using SchoolProject.Infrustructure.Abstracts;
using System.Reflection;
namespace SchoolProject.Core
{
    public static class ModuleCoreDependencies
    {
        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
            // Configuration for MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            // Configuration for AutoMapper
            services.AddAutoMapper(cfg =>
            {
                // Optional: add custom config here if needed
            }, Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
