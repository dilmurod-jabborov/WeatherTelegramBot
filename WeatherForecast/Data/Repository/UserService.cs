using Newtonsoft.Json;
using WeatherForecast.Data.Constanta;
using WeatherForecast.Domain;
using WeatherForecast.Models;

namespace WeatherForecast.Data.Repository;

public class UserService
{
    public void Register(string firstName, string lastName, string phoneNumber)
    {
        var users = JsonConvert.DeserializeObject<List<Users>>(FileHelper.ReadFromFile(PathHolder.UsersFilePath)) ?? new List<Users>();

        var exists = users.Find(x => x.PhoneNumber == phoneNumber);
        if (exists != null)
        {
            throw new Exception("User already exists");
        }

        var user = new Users
        {
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber
        };

        users.Add(user);

        FileHelper.WriteToFile(PathHolder.UsersFilePath, JsonConvert.SerializeObject(users, Formatting.Indented)); 
    }
}
    