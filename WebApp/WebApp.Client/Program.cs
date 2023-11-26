using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebApp.Client.Services.Implementations;
using WebApp.Client.Services.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddScoped<IAccountClientService, AccountClientService>();

await builder.Build().RunAsync();
