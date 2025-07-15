using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaskManagementSystem.TaskBoardService.Infrastructure.DataAccess.ORM;


public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    private readonly IConfiguration _configuration;

    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var dbConfig = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var dbSettings = dbConfig.GetSection("Database");

        var connectionString =
            $"Host={dbSettings["Host"]};" +
            $"Port={dbSettings["Port"]};" +
            $"Database={dbSettings["Database"]};" +
            $"Username={dbSettings["User"]};" +
            $"Password={dbSettings["Password"]};" +
            $"Include Error Detail=true";

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
