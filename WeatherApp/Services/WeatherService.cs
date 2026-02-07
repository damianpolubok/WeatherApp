using WeatherApp.Helpers.IHelpers;
using WeatherApp.Models;
using WeatherApp.Services.IServices;

namespace WeatherApp.Services;

public class WeatherService : IWeatherService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<WeatherService> _logger;
    private readonly IWeatherHelper _weatherHelper;

    public WeatherService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<WeatherService> logger, IWeatherHelper weatherHelper)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
        _weatherHelper = weatherHelper;
    }

    public async Task<WeatherViewModel?> GetWeatherForCityAsync(string cityName)
    {
        if (string.IsNullOrWhiteSpace(cityName))
        {
            _logger.LogWarning("City name provided is empty.");
            return null;
        }

        var apiKey = _configuration["WeatherSettings:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            _logger.LogError("API Key is missing.");
            return null;
        }

        var client = _httpClientFactory.CreateClient("WeatherApi");

        var url = $"data/2.5/weather?q={cityName}&units=metric&lang=en&appid={apiKey}";

        try
        {
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var dto = await response.Content.ReadFromJsonAsync<WeatherResponse>();

                if (dto != null)
                {
                    return _weatherHelper.MapToViewModel(dto);
                }

                return null;
            }

            _logger.LogWarning("API Error for {City}. Status: {Status}", cityName, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception fetching weather for: {City}", cityName);
            return null;
        }
    }

    public async Task<WeatherResponse?> GetRawWeatherAsync(string cityName)
    {
        if (string.IsNullOrWhiteSpace(cityName)) return null;

        var apiKey = _configuration["WeatherSettings:ApiKey"];
        var client = _httpClientFactory.CreateClient("WeatherApi");
        var url = $"data/2.5/weather?q={cityName}&units=metric&lang=en&appid={apiKey}";

        var response = await client.GetAsync(url);
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<WeatherResponse>()
            : null;
    }
}