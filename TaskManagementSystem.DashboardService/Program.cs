using TaskManagementSystem.GrpcLib.Configurations.AspNet;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpcLib();

var app = builder.Build();

app.UseGrpcLib();

app.Run();
