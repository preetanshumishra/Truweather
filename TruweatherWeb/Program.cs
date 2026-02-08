using Microsoft.AspNetCore.Components.Authorization;
using TruweatherCore.Http;
using TruweatherWeb.Components;
using TruweatherWeb.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Authentication & Authorization
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

// Token storage — scoped per Blazor circuit
builder.Services.AddScoped<ITokenStorage, ServerTokenStorage>();
builder.Services.AddScoped<TruweatherAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<TruweatherAuthStateProvider>());

// HTTP client for API communication
var apiBaseUrl = builder.Configuration.GetValue<string>("ApiBaseUrl") ?? "http://localhost:5000";
builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped(sp =>
    new HttpClientWrapper(sp.GetRequiredService<HttpClient>(), apiBaseUrl));

// Application services
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
