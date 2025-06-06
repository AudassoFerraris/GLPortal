using GLPortal.Application.Services;
using GLPortal.Core.Models;
using GLPortal.Infrastructure.DependencyInjection;
using GLPortal.Web.Api;
using GLPortal.Web.Components;

using MudBlazor.Services;
var builder = WebApplication.CreateBuilder(args);

// Add support to run as a Windows Service
builder.Host.UseWindowsService();

builder.AddGitLabServices();

builder.Services.AddScoped<IProjectService, ProjectService>();

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapGroup("/api/issues").ConfigureIssuesApi();

app.Run();
