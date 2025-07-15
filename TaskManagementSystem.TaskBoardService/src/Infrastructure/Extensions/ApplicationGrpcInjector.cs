using TaskManagementSystem.GrpcLib.Configurations.AspNet;
using TaskManagementSystem.SharedLib.Api.Grpc.Server.Interceptors;
using TaskManagementSystem.TaskBoardService.Api.Grpc.Services;

namespace TaskManagementSystem.TaskBoardService.Infrastructure.Extensions;


public static class ApplicationGrpcInjector
{
    public static IServiceCollection AddApplicationGrpc(this IServiceCollection services)
    {
        services.AddGrpcLib();
        services.AddGrpc(op =>
            op.Interceptors.Add<ExecutionContextInitializerGrpcServerInterceptor>()
        );
        return services;
    }

    public static IApplicationBuilder UseApplicationGrpc(this WebApplication app)
    {
        app.UseGrpcLib();
        app.MapGrpcService<TaskBoardGrpcService>();
        return app;
    }
}
