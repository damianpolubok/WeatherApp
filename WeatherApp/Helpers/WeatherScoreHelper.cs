using WeatherApp.Helpers.IHelpers;
using WeatherApp.Models;

namespace WeatherApp.Helpers;

public class WeatherScoreHelper : IWeatherScoreHelper
{
    private const double OptimalTemp = 22.0;
    private const double TempScale = 12.0;
    private const double OptimalHumidity = 45.0;
    private const double HumidityScale = 30.0;
    private const double WindScale = 8.0;

    public double CalculateScore(WeatherResponse weather)
    {
        double tempFactor = GetTemperatureFactor(weather.Main.Temp);
        double humidityFactor = GetHumidityFactor(weather.Main.Humidity);
        double windFactor = GetWindFactor(weather.Wind.Speed);

        double baseScore = tempFactor * 100.0;

        double finalScore = baseScore * (0.7 + 0.3 * humidityFactor) * (0.7 + 0.3 * windFactor);

        return Math.Round(Math.Clamp(finalScore, 0, 100), 1);
    }

    private double GetTemperatureFactor(double currentTemp)
    {
        double diff = Math.Abs(currentTemp - OptimalTemp);
        return 1.0 / (1.0 + Math.Pow(diff / TempScale, 2.0));
    }

    private double GetHumidityFactor(int currentHumidity)
    {
        double diff = Math.Abs(currentHumidity - OptimalHumidity);
        return 1.0 / (1.0 + Math.Pow(diff / HumidityScale, 2.0));
    }

    private double GetWindFactor(double currentWind)
    {
        return 1.0 / (1.0 + Math.Pow(currentWind / WindScale, 2.0));
    }
}