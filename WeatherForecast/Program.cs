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
            MainMenu mainMenu = new MainMenu("8395303487:AAGrBXtmyyrWxQk6LjIF3NxgxSe1v5A_eHo");
            await mainMenu.StartAsync();
        }
    }
}
