using System.Globalization;
using WeatherApp.Helpers.IHelpers;

namespace WeatherApp.Helpers;

public class WeatherVisualHelper : IWeatherVisualHelper
{
    private const string DefaultGradient = "background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);";

    public string GetTemperatureGradient(string? temperatureString)
    {
        if (string.IsNullOrWhiteSpace(temperatureString))
        {
            return DefaultGradient;
        }

        var cleanTemp = temperatureString.Replace("°C", "", StringComparison.OrdinalIgnoreCase).Trim();

        if (!double.TryParse(cleanTemp, NumberStyles.Any, CultureInfo.InvariantCulture, out double temperature))
        {
            return DefaultGradient;
        }

        return CalculateHslGradient(temperature);
    }

    private static string CalculateHslGradient(double t)
    {
        double hue;
        double saturation;
        double lightness1;
        double lightness2;

        if (t <= 0)
        {
            double ratio = Math.Clamp((t + 40) / 40.0, 0, 1);

            hue = 270 - ratio * 70;

            lightness1 = 20 + ratio * 30;
            lightness2 = lightness1 + 15;
            saturation = 60 + ratio * 20;
        }
        else
        {
            double ratio = Math.Clamp(t / 35.0, 0, 1);

            hue = 200 - ratio * 200;

            lightness1 = 50;
            lightness2 = 60;
            saturation = 80;
        }

        string color1 = $"hsl({hue:0}, {saturation:0}%, {lightness1:0}%)";
        string color2 = $"hsl({hue - 15:0}, {saturation:0}%, {lightness2:0}%)";

        return $"background: linear-gradient(135deg, {color1} 0%, {color2} 100%);";
    }
}