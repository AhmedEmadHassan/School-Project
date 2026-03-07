using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Infrustructure.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
namespace SchoolProject.Infrustructure
{
    public static class ServiceRegistration
    {
        private static void AddJWTSettings(IServiceCollection services, IConfiguration configuration)
        {
            // Add JWT Settings
            var jwtSettings = new JwtSettings();
            configuration.GetSection(nameof(jwtSettings)).Bind(jwtSettings);
            services.AddSingleton(jwtSettings);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(x =>
           {
               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = jwtSettings.ValidateIssuer,
                   ValidIssuers = new[] { jwtSettings.Issuer },
                   ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                   ValidAudience = jwtSettings.Audience,
                   ValidateAudience = jwtSettings.ValidateAudience,
                   ValidateLifetime = jwtSettings.ValidateLifeTime,
                   NameClaimType = JwtRegisteredClaimNames.Name,   // optional custom name
                   RoleClaimType = "Role"        // use your custom claim key instead of the URI
               };
           });
        }
        private static void AddIdentitySettings(IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<User, Role>(option =>
            {
                // Password settings.
                option.Password.RequireDigit = true;
                option.Password.RequireLowercase = true;
                option.Password.RequireNonAlphanumeric = true;
                option.Password.RequireUppercase = true;
                option.Password.RequiredLength = 6;
                option.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                option.Lockout.MaxFailedAccessAttempts = 5;
                option.Lockout.AllowedForNewUsers = true;

                // User settings.
                option.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                option.User.RequireUniqueEmail = false;
                option.SignIn.RequireConfirmedEmail = false;

            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

        }
        private static void ConfigureSwaggerAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            // Add Swagger Gen Authorization
            services.AddSwaggerGen(options =>
            {

                options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("bearer", document)] = []
                });
            });
        }
        public static IServiceCollection AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {

            AddIdentitySettings(services, configuration);

            AddJWTSettings(services, configuration);

            ConfigureSwaggerAuthentication(services, configuration);

            return services;
        }
    }
}
