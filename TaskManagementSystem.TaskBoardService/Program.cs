using TaskManagementSystem.GrpcLib.Configurations.AspNet;
using TaskManagementSystem.TaskBoardService.Api.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpcLib();
builder.Services.AddMediatR(
    options =>
    {
        options.RegisterServicesFromAssemblyContaining<TaskBoardGrpcService>();
    }
);

var app = builder.Build();

app.UseGrpcLib();
app.MapGrpcService<TaskBoardGrpcService>();
app.Run();
