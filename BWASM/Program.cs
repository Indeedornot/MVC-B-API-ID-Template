using BWASM;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//LIBRARY LoC Library
builder.Services.AddLocalization();

builder.Services.AddHttpClient("BWASM", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

builder.Services.AddScoped(sp => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});

await builder.Build().RunAsync();