using System.Text.Json.Serialization;

namespace WeatherApp.Models;

public class WeatherDescription
{
    [JsonPropertyName("main")]
    public string Main { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("icon")]
    public string Icon { get; set; }
}
