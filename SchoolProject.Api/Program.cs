

using Microsoft.EntityFrameworkCore;
using SchoolProject.Core;
using SchoolProject.Core.Middleware;
using SchoolProject.Infrustructure;
using SchoolProject.Infrustructure.Context;
using SchoolProject.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
#region Dependency Injection
builder.Services.AddInfrastructureDependencies();
builder.Services.AddServiceDependencies();
builder.Services.AddCoreDependencies();
#endregion
#region AddSwaggerServices
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion
var app = builder.Build();
app.UseMiddleware<ErrorHandlerMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || true)
{
    #region AddSwaggerMiddlewares
    app.UseSwagger();
    app.UseSwaggerUI();
    #endregion
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();
