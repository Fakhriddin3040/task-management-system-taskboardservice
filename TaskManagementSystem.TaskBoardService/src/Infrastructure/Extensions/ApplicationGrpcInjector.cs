using TaskManagementSystem.GrpcLib.Configurations.AspNet;
using TaskManagementSystem.SharedLib.Api.Grpc.Server.Interceptors;
using TaskManagementSystem.TaskBoardService.Api.Grpc.Services;
using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank;
using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Interfaces;
using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Strategies;
using TaskManagementSystem.TaskBoardService.Core.Algorithms.NumeralRank.Strategies.Validations;

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

    public static IServiceCollection AddNumeralRankAlgorithm(this IServiceCollection services)
    {
        services.AddScoped<INumeralRankValidationStrategy, FirstNumeralRankValidationStrategy>();
        services.AddScoped<INumeralRankValidationStrategy, TopNumeralRankValidationStrategy>();
        services.AddScoped<INumeralRankValidationStrategy, EndNumeralRankValidationStrategy>();
        services.AddScoped<INumeralRankValidationStrategy, BetweenNumeralRankValidationStrategy>();
        services.AddScoped<INumeralRankValidationStrategySelector, NumeralRankValidationStrategySelector>();

        services.AddScoped<INumeralRankStrategy, FirstNumeralRankStrategy>();
        services.AddScoped<INumeralRankStrategy, TopNumeralRankStrategy>();
        services.AddScoped<INumeralRankStrategy, EndNumeralRankStrategy>();
        services.AddScoped<INumeralRankStrategy, BetweenNumeralRankStrategy>();
        services.AddScoped<INumeralRankStrategySelector, NumeralRankStrategySelector>();

        return services;
    }

    public static IApplicationBuilder UseApplicationGrpc(this WebApplication app)
    {
        app.UseGrpcLib();
        app.MapGrpcService<TaskBoardGrpcService>();
        return app;
    }
}
