using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Interfaces;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.TaskBoardService.Infrastructure.Extensions;
using TaskManagementSystem.TaskBoardService.Infrastructure.DataAccess.ORM;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationDependencies();
builder.Services.AddApplicationDbContext(builder.Configuration);
builder.Services.AddApplicationGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddNumeralRankAlgorithm();
builder.Services.AddLogging(l =>
{
    l.AddConsole();
    l.SetMinimumLevel(LogLevel.Debug);
});

builder.Configuration.AddJsonFile("appsettings.json", optional: true).AddEnvironmentVariables();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();   // ⚡ вот оно
}

app.UseApplicationGrpc();
app.Run();
