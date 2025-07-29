using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using WeatherForecast.Data;
using WeatherForecast.Models;
using System.Reflection;
using System.Text;

namespace WeatherForecast
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            string token = Environment.GetEnvironmentVariable("BOT_TOKEN");

            MainMenu mainMenu = new MainMenu(token);

            await mainMenu.StartAsync();
        }
    }
}
