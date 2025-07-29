using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using WeatherForecast.Data;
using WeatherForecast.Models;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            string token = "8395303487:AAGrBXtmyyrWxQk6LjIF3NxgxSe1v5A_eHo";             //Environment.GetEnvironmentVariable("BOT_TOKEN");

            MainMenu mainMenu = new MainMenu(token);

            await mainMenu.StartAsync();
        }
    }
}
