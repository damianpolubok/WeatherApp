using System.Text.Json.Serialization;

namespace WeatherApp.Models;

public class Wind
{
    [JsonPropertyName("speed")]
    public double Speed { get; set; }
}
