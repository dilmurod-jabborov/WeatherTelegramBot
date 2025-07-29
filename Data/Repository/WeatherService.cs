using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherForecast.Data.Constanta;
using WeatherForecast.Models;

namespace WeatherForecast.Data.Repository;

public class WeatherService
{
    private readonly HttpClient httpClient;

    public WeatherService()
    {
        httpClient = new HttpClient();
    }

    public async Task<WeatherModel> GetDailyWeatherAsync(double lat, double lon)
    {
        string today = DateTime.UtcNow.ToString("yyyy-MM-dd");

        string latStr = lat.ToString(CultureInfo.InvariantCulture);
        string lonStr = lon.ToString(CultureInfo.InvariantCulture);

        string url = $"https://api.open-meteo.com/v1/forecast?latitude={latStr}" +
            $"&longitude={lonStr}&start_date={today}&end_date={today}&daily=temperature_2m_min,temperature_2m_max," +
            $"precipitation_sum,wind_speed_10m_max&timezone=auto";


        var response = await httpClient.GetStringAsync(url);
        return JsonConvert.DeserializeObject<WeatherModel>(response)!;
    }

    public async Task<WeatherModel> GetWeeklyWeatherAsync(double lat, double lon)
    {
        var start = DateTime.Today.ToString("yyyy-MM-dd");
        var end = DateTime.Today.AddDays(6).ToString("yyyy-MM-dd");

        string latStr = lat.ToString(CultureInfo.InvariantCulture);
        string lonStr = lon.ToString(CultureInfo.InvariantCulture);

        string url = $"https://api.open-meteo.com/v1/forecast?latitude={latStr}" +
            $"&longitude={lonStr}&start_date={start}&end_date={end}&daily=temperature_2m_min," +
            $"temperature_2m_max,precipitation_sum,wind_speed_10m_max&timezone=auto";

        var response = await httpClient.GetStringAsync(url);
        return JsonConvert.DeserializeObject<WeatherModel>(response);
    }

    public async Task<WeatherModel> GetMonthlyWeatherAsync(double lat, double lon)
    {
        string start = DateTime.Today.ToString("yyyy-MM-dd");
        string end = DateTime.Today.AddDays(15).ToString("yyyy-MM-dd");

        string latStr = lat.ToString(CultureInfo.InvariantCulture);
        string lonStr = lon.ToString(CultureInfo.InvariantCulture);

        string url = $"https://api.open-meteo.com/v1/forecast?latitude={latStr}" +
            $"&longitude={lonStr}&start_date={start}&end_date={end}&daily=temperature_2m_min," +
            $"temperature_2m_max,precipitation_sum,wind_speed_10m_max&timezone=auto";

        var response = await httpClient.GetStringAsync(url);
        return JsonConvert.DeserializeObject<WeatherModel>(response)!;
    }
}

