using Microsoft.Extensions.DependencyInjection;
using TeamCollaboration.Application.Services;

namespace TeamCollaboration.Application.DependencyInjection;

/// <summary>
/// Registers application-layer services with the dependency injection container.
/// </summary>
public static class ApplicationRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<TaskService>();
        services.AddSignalR();

        return services;
    }
}

