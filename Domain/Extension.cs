using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WeatherForecast.Models;

namespace WeatherForecast.Domain
{
    public static class Extension
    {
        public static List<Users> ToUser(this string text)
        {
            List<Users> users = new();

            string[] lines = text.Split('\n');

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split(',');

                users.Add(new Users
                {
                    Id = int.Parse(parts[0]),
                    FirstName = parts[1],
                    LastName = parts[2],
                    PhoneNumber = parts[3]
                });
            }

            return users;
        }

        public static List<string> ConvertToString<T>(this List<T> lists)
        {
            var convertedlist = new List<string>();

            foreach (var list in lists)
            {
                convertedlist.Add(list.ToString());
            }

            return convertedlist;
        }
    }
}
