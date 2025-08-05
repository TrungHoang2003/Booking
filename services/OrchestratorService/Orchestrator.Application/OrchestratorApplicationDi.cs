using BuildingBlocks.Interfaces;
using BuildingBlocks.PipelineBehaviors;
using BuildingBlocks.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Orchestrator.Application.Interfaces;
using Orchestrator.Application.Services;

namespace Orchestrator.Application;

public static class OrchestratorApplicationDi
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IBecomeHostDraftService, BecomeHostDraftService>();
        services.AddSingleton<IRedisService, RedisService>();
        return services;
    }
}