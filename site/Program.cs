using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using site.Utils;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<site.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Read base URL from appsettings.json (the .NET equivalent of .env)
var baseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "https://api.github.com";

// Register HttpClient
builder.Services.AddScoped(sp =>
{
    var client = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
    client.DefaultRequestHeaders.Add("User-Agent", "SharpIB-Site");
    return client;
});

// Register the Api utility class
builder.Services.AddScoped(sp =>
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    return new Api(httpClient, baseUrl);
});

await builder.Build().RunAsync();

