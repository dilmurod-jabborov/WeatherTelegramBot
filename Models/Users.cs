using System.Text.Json.Serialization;
using WeatherForecast.Data.Constanta;
using WeatherForecast.Domain;

namespace WeatherForecast.Models;

public class Users
{
    public Users()
    {
        Id = IdGenerateHelper.GenerateId(PathHolder.UsersFilePath);
    }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string LastName { get; set; }

    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; }
}
