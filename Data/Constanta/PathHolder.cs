using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Data.Constanta;

public static class PathHolder
{
    public static readonly string UsersFilePath =
        Path.Combine(AppContext.BaseDirectory, "Data", "Database", "users.json");
}

