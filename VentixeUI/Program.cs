using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using VentixeUI;
using VentixeUI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Load configuration from wwwroot/appsettings.json
await LoadConfiguration(builder);

// Register Event service with its HttpClient
builder.Services.AddScoped<IEventService>(sp => 
{
    var eventServiceUrl = builder.Configuration["ApiSettings:EventServiceUrl"];
    var client = new HttpClient { BaseAddress = new Uri(eventServiceUrl ?? "https://ventixeeventservicewilkon-d9e0fqd4bxeye7au.swedencentral-01.azurewebsites.net/") };
    return new EventService(client);
});

// Register Booking service with its HttpClient
builder.Services.AddScoped<IBookingService>(sp => 
{
    var bookingServiceUrl = builder.Configuration["ApiSettings:BookingServiceUrl"];
    var client = new HttpClient { BaseAddress = new Uri(bookingServiceUrl ?? "https://ventixebookingservicewilkon-dggphge9a6b2a6ef.swedencentral-01.azurewebsites.net/") };
    return new BookingService(client);
});

await builder.Build().RunAsync();

// Helper method to load configuration
static async Task LoadConfiguration(WebAssemblyHostBuilder builder)
{
    var client = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
    using var response = await client.GetAsync("appsettings.json");
    using var stream = await response.Content.ReadAsStreamAsync();
    builder.Configuration.AddJsonStream(stream);
}
