using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CineTerra;
using CineTerra.Services.Implementations;
using CineTerra.Services.Interfaces;
using CineTerra.State;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient configured with base address
builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Storage and Domain Services
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped<IProductionService, ProductionService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IPassportService, PassportService>();

// Global State Containers
builder.Services.AddScoped<PilgrimageState>();

await builder.Build().RunAsync();
