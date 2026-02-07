using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WeatherApp.Models;
using WeatherApp.Services.IServices;

namespace WeatherApp.Components.Pages;

public partial class MapPage : IAsyncDisposable
{
    [Inject]
    public IOpenStreetMapService GeoService { get; set; } = default!;

    [Inject]
    public IWeatherService WeatherService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    public IConfiguration Configuration { get; set; } = default!;

    [Inject]
    public ILogger<MapPage> Logger { get; set; } = default!;

    private IJSObjectReference? _module;

    public string CityName { get; set; } = string.Empty;
    public MapState CurrentState { get; private set; } = MapState.Idle;
    public WeatherViewModel? WeatherResult { get; private set; }

    private const double ViewLat = 52.0693;
    private const double ViewLon = 19.4803;
    private const int DefaultZoom = 6;

    private const double MarkerLat = 53.1325;
    private const double MarkerLon = 23.1688;
    private const string DefaultCity = "Białystok";

    private string HeroBackgroundStyle =>
        $"background-image: url('{Configuration["WeatherSettings:BackgroundImageUrl"]}');";

    protected override async Task OnInitializedAsync()
    {
        WeatherResult = await WeatherService.GetWeatherForCityAsync(DefaultCity);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/mapHelper.js");
            await _module.InvokeVoidAsync("initializeMap", "map-container", ViewLat, ViewLon, DefaultZoom, MarkerLat, MarkerLon);
        }
    }

    private async Task SearchLocation()
    {
        if (string.IsNullOrWhiteSpace(CityName))
        {
            return;
        }

        CurrentState = MapState.Searching;
        WeatherResult = null;
        StateHasChanged();

        try
        {
            var mapTask = UpdateMapAsync();
            var weatherTask = UpdateWeatherAsync();

            await Task.WhenAll(mapTask, weatherTask);

            if (mapTask.Result || weatherTask.Result != null)
            {
                CurrentState = MapState.Success;
            }
            else
            {
                CurrentState = MapState.Error;
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error during search");
            CurrentState = MapState.Error;
        }
        finally
        {
            if (CurrentState == MapState.Success)
            {
                CurrentState = MapState.Idle;
            }
            StateHasChanged();
        }
    }

    private async Task<bool> UpdateMapAsync()
    {
        try
        {
            var coords = await GeoService.GetCoordinatesAsync(CityName);
            if (coords != null && _module != null)
            {
                if (double.TryParse(coords.Lat, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double lat) &&
                    double.TryParse(coords.Lon, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double lon))
                {
                    await _module.InvokeVoidAsync("updateMap", lat, lon);
                    return true;
                }
            }
            return false;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Map update failed");
            return false;
        }
    }

    private async Task<WeatherViewModel?> UpdateWeatherAsync()
    {
        try
        {
            var result = await WeatherService.GetWeatherForCityAsync(CityName);
            WeatherResult = result;
            return result;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Weather fetch failed");
            return null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            await _module.DisposeAsync();
        }
    }

    public enum MapState
    {
        Idle,
        Searching,
        Success,
        Error
    }
}