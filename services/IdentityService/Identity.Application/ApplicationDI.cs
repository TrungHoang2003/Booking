using BuildingBlocks.PipelineBehaviors;
using FluentValidation;
using Identity.Application.Commons;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application;
public static class ApplicationDi
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
            typeof(ApplicationDi).Assembly
        ));
        services.AddValidatorsFromAssembly(typeof(ApplicationDi).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddSingleton<IJwtService, JwtService>();
        services.AddSingleton<IRedisService, RedisService>();
        return services;
    }
}
