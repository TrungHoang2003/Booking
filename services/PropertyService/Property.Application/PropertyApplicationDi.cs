using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Property.Application;

public static class PropertyApplicationDi
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg=>
        {
            cfg.RegisterServicesFromAssemblies(typeof(PropertyApplicationDi).Assembly);
        });
        services.AddValidatorsFromAssembly(typeof(PropertyApplicationDi).Assembly);
    }
}