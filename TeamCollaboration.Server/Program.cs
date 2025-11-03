using TeamCollaboration.Application.DependencyInjection;
using TeamCollaboration.Application.RealTime;
using TeamCollaboration.Server.Components;
using TeamCollaboration.Infrastructure.DependencyInjection;
using Microsoft.EntityFrameworkCore;

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


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapHub<KanbanHub>(KanbanHub.HubPath);

app.Run();