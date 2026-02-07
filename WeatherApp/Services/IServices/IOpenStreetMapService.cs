using WeatherApp.Models;

namespace WeatherApp.Services.IServices;

public interface IOpenStreetMapService
{
    Task<GeoCoordinates?> GetCoordinatesAsync(string cityName);
}