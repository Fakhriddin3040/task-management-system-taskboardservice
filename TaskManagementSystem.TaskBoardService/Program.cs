using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Interfaces;
using TaskManagementSystem.TaskBoardService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationDependencies();
builder.Services.AddApplicationDbContext(builder.Configuration);
builder.Services.AddApplicationGrpc();
builder.Services.AddNumeralRankAlgorithm();
builder.Services.AddLogging(l =>
{
    l.AddConsole();
    l.SetMinimumLevel(LogLevel.Debug);
});

var app = builder.Build();

app.UseApplicationGrpc();
app.Run();
