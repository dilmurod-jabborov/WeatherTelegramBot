using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Data.Constanta;

public static class PathHolder
{
    private static readonly string baseRoot =
        Path.Combine(AppContext.BaseDirectory, "..", "..", "..");

    public static readonly string UsersFilePath =
        Path.Combine(baseRoot, "Data", "Database", "users.json");
}
