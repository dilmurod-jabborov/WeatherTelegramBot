using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherForecast.Models;

namespace WeatherForecast.Domain
{
    public class IdGenerateHelper
    {
        public static int GenerateId(string filePath)
        {
            if (File.Exists(filePath))
            {
                var users = JsonConvert.DeserializeObject<List<Users>>(FileHelper.ReadFromFile(filePath));

                int maxId = 0;
                if (users != null)
                {
                    foreach (var user in users)
                    {
                        if (maxId < user.Id)
                        {
                            maxId = user.Id;
                        }
                    }

                    return ++maxId;
                }
                else
                {
                    return 1;
                }

            }
            else
            {
                return 1;
            }
        }
    }
}
