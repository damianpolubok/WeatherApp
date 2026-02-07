using Microsoft.AspNetCore.Components;
using WeatherApp.Models;
using WeatherApp.Services.IServices;

namespace WeatherApp.Components.Pages;

public partial class Home
{
    [Inject]
    public IWeatherService WeatherService { get; set; } = default!;

    [Inject]
    public ILogger<Home> Logger { get; set; } = default!;

    [Inject]
    public IConfiguration Configuration { get; set; } = default!;

    public string CityName { get; set; } = string.Empty;

    public WeatherViewModel? SearchResult { get; private set; }

    public SearchState CurrentState { get; private set; } = SearchState.Idle;

    private string HeroBackgroundStyle =>
        $"background-image: url('{Configuration["WeatherSettings:BackgroundImageUrl"]}');";

    private async Task SearchWeather()
    {
        CurrentState = SearchState.Loading;
        SearchResult = null;

        SearchResult = await WeatherService.GetWeatherForCityAsync(CityName);

        if (SearchResult != null)
        {
            CurrentState = SearchState.Success;
            Logger.LogInformation("Weather found for: {City}", SearchResult.CityName);
        }
        else
        {
            CurrentState = SearchState.Error;
            Logger.LogWarning("Search failed for input: {CityName}", CityName);
        }
    }

    public enum SearchState
    {
        Idle,
        Loading,
        Success,
        Error
    }
}