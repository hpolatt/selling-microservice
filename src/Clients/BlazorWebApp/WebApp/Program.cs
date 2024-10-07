using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using WebApp.Application.Services;
using WebApp.Application.Services.Interfaces;
using WebApp.Components;
using WebApp.Infrastructure;
using WebApp.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddBlazoredLocalStorage();

// Add services to the container.
builder.Services.AddTransient<IIdentityService, IdentityService>();
builder.Services.AddTransient<ICatalogService, CatalogService>();
builder.Services.AddTransient<IBasketService, BasketService>();

// Add AuthStateProvider
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddTransient<AuthTokenHandler>();

builder.Services.AddHttpClient("ApiClient")
    .AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddScoped(sp => {
    var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return clientFactory.CreateClient("ApiGateWayHttpClient");
});

builder.Services.AddHttpClient("ApiGateWayHttpClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5000");
}).AddHttpMessageHandler<AuthTokenHandler>();;

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

app.Run();
