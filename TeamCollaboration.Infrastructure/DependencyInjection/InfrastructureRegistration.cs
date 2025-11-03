using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TeamCollaboration.Domain.Repositories;
using TeamCollaboration.Infrastructure.Persistence;
using TeamCollaboration.Infrastructure.Repositories;

namespace TeamCollaboration.Infrastructure.DependencyInjection;

public static class InfrastructureRegistration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder>? configureDbContext = null)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            if (configureDbContext is not null)
            {
                configureDbContext(options);
            }
            else
            {
                options.UseInMemoryDatabase("TeamCollaborationDb");
            }
        });

        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IBoardRepository, BoardRepository>();

        return services;
    }
}

