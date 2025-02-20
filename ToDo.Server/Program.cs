using ToDo.Server.Configurations;
using ToDo.Server.Constants;
using ToDo.Server.Middlewares;
using ToDo.Server.Repositories;
using ToDo.Server.Repositories.Interfaces;
using ToDo.Server.Services;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddCustomApiSecurity(configuration);
builder.Services.AddCustomDatabaseConfiguration(configuration);

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ITasksRepository, TasksRepository>();

builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<TasksService>();

builder.Services.AddAuthorizationBuilder()
                .AddPolicy(Constants.PolicyUser, policy => policy
                .RequireRole(Constants.PolicyUser));

builder.Services.AddCors(options =>
{
    options.AddPolicy(Constants.AllowLocalhost, builder =>
    {
        builder.WithOrigins(Constants.OriginReact)
               .AllowAnyHeader()
               .AllowAnyMethod()
               .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
    });
});

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseCors(Constants.AllowLocalhost);
app.UseDefaultFiles();
app.MapStaticAssets();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("/index.html");
app.Run();
