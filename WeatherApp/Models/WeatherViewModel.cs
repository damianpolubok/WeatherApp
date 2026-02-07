namespace WeatherApp.Models;

public class WeatherViewModel
{
    public string CityName { get; set; } = string.Empty;
    public string Temperature { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
    public string FeelsLike { get; set; } = string.Empty;
    public string Humidity { get; set; } = string.Empty;
    public string WindSpeed { get; set; } = string.Empty;
}