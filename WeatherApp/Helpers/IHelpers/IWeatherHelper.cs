using WeatherApp.Models;

namespace WeatherApp.Helpers.IHelpers;

public interface IWeatherHelper
{
    WeatherViewModel MapToViewModel(WeatherResponse dto);
}