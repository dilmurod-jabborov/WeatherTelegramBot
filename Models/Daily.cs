using System.Collections.Generic;

namespace WeatherForecast.Models;

public class Daily
{
    public List<string> Time { get; set; }
    public List<double> Temperature_2m_Min { get; set; }
    public List<double> Temperature_2m_Max { get; set; }
    public List<double> Precipitation_Sum { get; set; }
    public List<double> Wind_Speed_10m_Max { get; set; }
}