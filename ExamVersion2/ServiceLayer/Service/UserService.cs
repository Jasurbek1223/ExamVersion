using AirplaneManagementApp.BackEnd.DataLayer.Constants;
using AirplaneManagementApp.BackEnd.DomainLayer.Model;
using AirplaneManagementApp.BackEnd.ServiceLayer.Helpers;
using AirplaneManagementApp.BackEnd.ServiceLayer.Interface;
using Newtonsoft.Json;

namespace AirplaneManagementApp.BackEnd.ServiceLayer.Service;

public class UserService : IUserService
{
    string path = FilePath.USER_PATH;

    public UserService()
    {
        string result = File.ReadAllText(path);
        if (string.IsNullOrEmpty(result))
        {
            File.WriteAllText(path, "[]");
        }
    }
    public Response<User> Create(User user)
    {
        string source = File.ReadAllText(path);
        List<User> users = JsonConvert.DeserializeObject<List<User>>(source);

        if (!user.FirstName.CheckName() ||
            !user.LastName.CheckName() ||
            !user.Email.CheckEmail() ||
            !user.PhoneNumber.CheckNumber())
        {
            return new Response<User>
            {
                StatusCode = 403,
                Message = "Invalid information",
            };
        }

        User existUser = users.FirstOrDefault(u => u.Email.Equals(user.Email));

        if (existUser is not null)
        {
            return new Response<User>
            {
                StatusCode = 403,
                Message = "This user already exist",
            };
        }
        int lastId;
        try
        {
            lastId = users.LastOrDefault().Id;
        }
        catch
        {
            lastId = 0;
        }
        user.Id = lastId + 1;
        users.Add(user);

        string json = JsonConvert.SerializeObject(users, Newtonsoft.Json.Formatting.Indented);

        File.WriteAllText(path, json);

        return new Response<User>
        {
            StatusCode = 200,
            Message = "Succes",
            Data = user
        };
    }

    public Response<bool> Delete(int id)
    {
        string source = File.ReadAllText(path);

        List<User> users = JsonConvert.DeserializeObject<List<User>>(source);

        User existUser = users.FirstOrDefault(l => l.Id.Equals(id));

        if (existUser is null)
        {
            return new Response<bool>
            {
                StatusCode = 404,
                Message = "Not found",
                Data = false
            };
        }

        users.Remove(existUser);
        string json = JsonConvert.SerializeObject(users, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(path, json);

        return new Response<bool>
        {
            StatusCode = 200,
            Message = "Succes",
            Data = true
        };
    }

    public Response<List<User>> GetAll()
    {
        string source = File.ReadAllText(path);

        List<User> users = JsonConvert.DeserializeObject<List<User>>(source);

        return new Response<List<User>>
        {
            Message = "Succes",
            StatusCode = 200,
            Data = users
        };
    }

    public Response<User> GetByEmail(string email)
    {
        return new Response<User>
        {
            StatusCode = 200,
            Message = "Succes",
            Data = GetAll().Data.FirstOrDefault(Data => Data.Email == email.Trim())
        };
    }

    public Response<User> Update(User user)
    {
        string source = File.ReadAllText(path);

        List<User> users = JsonConvert.DeserializeObject<List<User>>(source);

        User existUser = users.FirstOrDefault(l => l.Email.Equals(user.Email));

        if (existUser is null)
        {
            return new Response<User>
            {
                StatusCode = 404,
                Message = "Not found",
            };
        }

        existUser.Id = user.Id;
        existUser.PhoneNumber = user.PhoneNumber;
        existUser.FirstName = user.FirstName;
        existUser.LastName = user.LastName;
        existUser.Email = user.Email;
        existUser.Password = user.Password;

        string json = JsonConvert.SerializeObject(users, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(path, json);

        return new Response<User>
        {
            StatusCode = 200,
            Message = "Succes",
            Data = user
        };
    }
}
