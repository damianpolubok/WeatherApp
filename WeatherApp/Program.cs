using WeatherApp.Components;
using WeatherApp.Helpers;
using WeatherApp.Helpers.IHelpers;
using WeatherApp.Services;
using WeatherApp.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient("WeatherApi", client =>
{
    client.BaseAddress = new Uri("https://api.openweathermap.org/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpClient("OpenStreetMap", client =>
{
    client.BaseAddress = new Uri("https://nominatim.openstreetmap.org/");
    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
    client.DefaultRequestHeaders.Add("Referer", "https://nominatim.openstreetmap.org/");
});

builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IOpenStreetMapService, OpenStreetMapService>();

builder.Services.AddScoped<IWeatherVisualHelper, WeatherVisualHelper>();
builder.Services.AddScoped<IWeatherHelper, WeatherHelper>();
builder.Services.AddScoped<IWeatherScoreHelper, WeatherScoreHelper>();

var app = builder.Build();

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
