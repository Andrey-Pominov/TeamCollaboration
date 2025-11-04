using TeamCollaboration.Application.DependencyInjection;
using TeamCollaboration.Application.RealTime;
using TeamCollaboration.Server.Components;
using TeamCollaboration.Infrastructure.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TeamCollaboration.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services
    .AddApplication()
    .AddInfrastructure(options =>
    {
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            options.UseNpgsql(connectionString);
        }
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// Apply migrations on startup to persist schema
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
      await db.Database.MigrateAsync();
    }
    catch
    {
        // If migration fails (e.g., DB unavailable), continue; the in-memory fallback may be in use
    }
}


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapHub<KanbanHub>(KanbanHub.HubPath);

app.Run();