using WeatherApp.Helpers.IHelpers;
using WeatherApp.Models;

namespace WeatherApp.Helpers;

public class WeatherHelper : IWeatherHelper
{
    public WeatherViewModel MapToViewModel(WeatherResponse dto)
    {
        var description = "-";
        var iconUrl = string.Empty;

        var weatherItem = dto.Weather.FirstOrDefault();

        if (weatherItem != null)
        {
            if (!string.IsNullOrEmpty(weatherItem.Description))
            {
                description = weatherItem.Description;
            }

            iconUrl = $"https://openweathermap.org/img/wn/{weatherItem.Icon}@2x.png";
        }

        return new WeatherViewModel
        {
            CityName = dto.CityName,
            Temperature = FormatTemperature(dto.Main.Temp),
            FeelsLike = FormatTemperature(dto.Main.FeelsLike),
            Humidity = $"{dto.Main.Humidity}%",
            WindSpeed = $"{dto.Wind.Speed} m/s",
            Description = description,
            IconUrl = iconUrl
        };
    }

    private static string FormatTemperature(double value)
    {
        var rounded = Math.Round(value, 0);

        if (rounded == 0)
        {
            return "0°C";
        }

        return $"{rounded:0}°C";
    }
}