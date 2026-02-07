using WeatherApp.Models;
using WeatherApp.Services.IServices;

namespace WeatherApp.Services;

public class OpenStreetMapService : IOpenStreetMapService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<OpenStreetMapService> _logger;

    public OpenStreetMapService(IHttpClientFactory httpClientFactory, ILogger<OpenStreetMapService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<GeoCoordinates?> GetCoordinatesAsync(string cityName)
    {
        if (string.IsNullOrWhiteSpace(cityName))
        {
            return null;
        }

        var client = _httpClientFactory.CreateClient("OpenStreetMap");

        try
        {
            var url = $"search?q={Uri.EscapeDataString(cityName)}&format=json&limit=1";

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var results = await response.Content.ReadFromJsonAsync<List<GeoCoordinates>>();

                if (results != null)
                {
                    return results.FirstOrDefault();
                }

                return null;
            }

            _logger.LogWarning("OSM API Error for {City}. Status: {Status}", cityName, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching coordinates for {City}", cityName);
            return null;
        }
    }
}