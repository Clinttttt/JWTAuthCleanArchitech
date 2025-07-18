/*using JWTAuthCleanArchitech.Application;
using JWTAuthCleanArchitech.WebUI.Server.Components;
using JWTAuthCleanArchitech.Infrastructure;

using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using JWTAuthCleanArchitech.WebUI.Server.Services;
using JWTAuthCleanArchitech.Infrastructure.Services;
using Microsoft.AspNetCore.Components.Authorization;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<AuthStateProvider>());
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthStateProvider>();
builder.Services.AddScoped<AuthApiService>();
builder.Services.AddHttpClient();




builder.Services.AddHttpClient("WebAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7140");
});

builder.Services.AddHttpClient<BookApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7140");
})
  .AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddScoped<ProtectedLocalStorage>();
builder.Services.AddScoped<TokenProvider>();
builder.Services.AddScoped<AuthHeaderHandler>();

builder.Services.AddHttpClient<AuthApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7140"); 
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();




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

app.Run();
*/








using JWTAuthCleanArchitech.Application;
using JWTAuthCleanArchitech.WebUI.Server.Components;
using JWTAuthCleanArchitech.Infrastructure;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using JWTAuthCleanArchitech.WebUI.Server.Services;
using JWTAuthCleanArchitech.Infrastructure.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthStateProvider>();
builder.Services.AddScoped<AuthApiService>();
builder.Services.AddHttpClient();


// Simple HTTP client setup - no complex handlers
builder.Services.AddHttpClient("WebAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7140");
});

builder.Services.AddHttpClient<BookApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7140");
});

builder.Services.AddScoped<ProtectedLocalStorage>();

builder.Services.AddHttpClient<AuthApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7140");
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

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

app.Run();