using TaskManagementSystem.SharedLib.Abstractions.Interfaces;
using TaskManagementSystem.SharedLib.Api.Grpc.Server.Interceptors;
using TaskManagementSystem.SharedLib.Providers;
using TaskManagementSystem.SharedLib.Providers.Interfaces;
using TaskManagementSystem.SharedLib.Services;
using TaskManagementSystem.TaskBoardService.Api.Grpc.Services;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Policies;
using TaskManagementSystem.TaskBoardService.Core.Interfaces.Repository;
using TaskManagementSystem.TaskBoardService.Infrastructure.DataAccess.Repositories;
using TaskManagementSystem.TaskBoardService.Infrastructure.Policies;

namespace TaskManagementSystem.TaskBoardService.Infrastructure.Extensions;


public static class ApplicationDependencyInjector
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddScoped<IValidBoardNamePolicy, ValidBoardNamePolicy>();
        services.AddScoped<IUniqueTaskBoardNamePolicy, UniqueBoardNamePolicy>();
        services.AddScoped<ITaskBoardRepository, TaskBoardRepository>();
        services.AddSingleton<IDateTimeService, DateTimeService>();
        services.AddScoped<IExecutionContextProvider, GrpcExecutionContextProvider>();
        services.AddScoped<ExecutionContextInitializerGrpcServerInterceptor>();
        services.AddScoped<IValidColumnNamePolicy, ValidColumnNamePolicy>();
        services.AddScoped<IUniqueColumnNamePolicy, UniqueColumnNamePolicy>();
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining<TaskBoardGrpcService>();
        });

        return services;
    }
}
