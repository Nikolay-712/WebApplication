using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebApp.Client.Services.Implementations;
using WebApp.Client.Services.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccountClientService, AccountClientService>();
builder.Services.AddScoped<AuthenticationStateProvider, ClientAuthenticationStateProvider>();

await builder.Build().RunAsync();
