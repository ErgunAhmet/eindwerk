using Application.CommandHandlers;
using Common;
using JobCandidateUI.Components;
using JobCandidateUI.Services;
using JobCandidateUI.StartupTasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Logging;
using MudBlazor;
using MudBlazor.Services;
using Shared.Extensions;
using Shared.Startup;

var builder = WebApplication.CreateBuilder(args);

// Load settings from config file
AppSettings.InitAppSettings(builder.Configuration);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));

builder.Services.AddAuthorization(options =>
{
    //options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddMediatR(typeof(CreateCandidateAccountHandler).Assembly);

builder.Services.AddStartupTask<MongoDBStartupTask>();

new Repository.Module().Execute(builder.Services);
// not needed for now
//new DomainService.Module().Execute(builder.Services);
new Application.Module().Execute(builder.Services);
new Infrastructure.Module().Execute(builder.Services);
new JobCandidateUI.Module().Execute(builder.Services);
// not needed for now
//new ServiceAgent.Module().Execute(builder.Services);

builder.Services.AddScoped<IdentityService>();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = true;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 5000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
    config.SnackbarConfiguration.ClearAfterNavigation = false;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
} 
else
{
    IdentityModelEventSource.ShowPII = true;
}
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();



app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
