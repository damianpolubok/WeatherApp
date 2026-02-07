using System.Text.Json.Serialization;

namespace WeatherApp.Models;

public class WeatherResponse
{
    [JsonPropertyName("name")]
    public string CityName { get; set; }

    [JsonPropertyName("main")]
    public WeatherMain Main { get; set; }

    [JsonPropertyName("weather")]
    public List<WeatherDescription> Weather { get; set; }

    [JsonPropertyName("wind")]
    public Wind Wind { get; set; }
}
