using Microsoft.Extensions.DependencyInjection;
using TeamCollaboration.Application.Services;

namespace TeamCollaboration.Application.DependencyInjection;

public static class ApplicationRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITaskService, TaskService>();
        services.AddSignalR();

        return services;
    }
}

