using BuildingBlocks.PipelineBehaviors;
using BuildingBlocks.Services;
using CloudinaryDotNet;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Property.Application;

public static class PropertyApplicationDi
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(typeof(PropertyApplicationDi).Assembly);
        });
        services.AddScoped<CloudinaryService>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
}