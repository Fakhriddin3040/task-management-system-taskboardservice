using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.TaskBoardService.Infrastructure.DataAccess.ORM;

namespace TaskManagementSystem.TaskBoardService.Infrastructure.Extensions;


public static class ApplicationDbContextExtension
{
    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var dbSettings = configuration.GetSection("Database");

        var connectionString =
            $"Host={dbSettings["Host"]};" +
            $"Port={dbSettings["Port"]};" +
            $"Database={dbSettings["Database"]};" +
            $"Username={dbSettings["User"]};" +
            $"Password={dbSettings["Password"]};" +
            $"Include Error Detail=true";

        services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
        });

        return services;
    }
}
