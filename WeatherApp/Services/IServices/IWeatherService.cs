using WeatherApp.Models;

namespace WeatherApp.Services.IServices;

public interface IWeatherService
{
    Task<WeatherResponse?> GetRawWeatherAsync(string cityName);
    Task<WeatherViewModel?> GetWeatherForCityAsync(string cityName);
}