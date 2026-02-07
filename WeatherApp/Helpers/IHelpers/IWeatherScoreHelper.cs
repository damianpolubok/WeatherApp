using WeatherApp.Models;

namespace WeatherApp.Helpers.IHelpers
{
    public interface IWeatherScoreHelper
    {
        double CalculateScore(WeatherResponse weather);
    }
}