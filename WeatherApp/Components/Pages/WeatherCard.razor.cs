using Microsoft.AspNetCore.Components;
using WeatherApp.Helpers.IHelpers;
using WeatherApp.Models;

namespace WeatherApp.Components.Pages;

public partial class WeatherCard
{
    [Inject]
    public IWeatherVisualHelper VisualService { get; set; } = default!;

    [Parameter]
    public WeatherViewModel Weather { get; set; } = default!;

    private string BackgroundStyle => VisualService.GetTemperatureGradient(Weather?.Temperature);
}