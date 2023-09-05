using AirplaneManagementApp.BackEnd.DataLayer.Constants;
using AirplaneManagementApp.BackEnd.DomainLayer.Model;
using AirplaneManagementApp.BackEnd.ServiceLayer.Helpers;
using AirplaneManagementApp.BackEnd.ServiceLayer.Interface;
using Newtonsoft.Json;

namespace AirplaneManagementApp.BackEnd.ServiceLayer.Service;

public class AirplaneService : IAirplaneService
{
    string path = FilePath.AIRPLANE_PATH;

    public AirplaneService()
    {
        string result = File.ReadAllText(path);
        if (string.IsNullOrEmpty(result))
        {
            File.WriteAllText(path, "[]");
        }
    }
    public Response<Airplane> Create(Airplane airplane)
    {
        string source = File.ReadAllText(path);
        List<Airplane> airplenes = JsonConvert.DeserializeObject<List<Airplane>>(source);

        Airplane existAirplene = airplenes.FirstOrDefault(u => u.Id.Equals(airplane.Id));

        if (existAirplene is not null)
        {
            return new Response<Airplane>
            {
                StatusCode = 403,
                Message = "This Airplene already exist",
            };
        }
        int lastId = airplenes.LastOrDefault().Id;

        airplane.Id = lastId + 1;

        airplenes.Add(airplane);

        string json = JsonConvert.SerializeObject(airplenes, Newtonsoft.Json.Formatting.Indented);

        File.WriteAllText(path, json);

        return new Response<Airplane>
        {
            StatusCode = 200,
            Message = "Succes",
            Data = airplane
        };
    }

    public Response<bool> Delete(int id)
    {
        string source = File.ReadAllText(path);

        List<Airplane> airplanes = JsonConvert.DeserializeObject<List<Airplane>>(source);

        Airplane existAirplene = airplanes.FirstOrDefault(l => l.Id.Equals(id));

        if (existAirplene is null)
        {
            return new Response<bool>
            {
                StatusCode = 404,
                Message = "Not found",
                Data = false
            };
        }

        airplanes.Remove(existAirplene);
        string json = JsonConvert.SerializeObject(airplanes, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(path, json);

        return new Response<bool>
        {
            StatusCode = 200,
            Message = "Succes",
            Data = true
        };
    }

    public Response<List<Airplane>> GetAll()
    {
        string source = File.ReadAllText(path);

        List<Airplane> airplenes = JsonConvert.DeserializeObject<List<Airplane>>(source);

        return new Response<List<Airplane>>
        {
            Message = "Succes",
            StatusCode = 200,
            Data = airplenes
        };
    }

    public Response<Airplane> GetById(int id)
    {
        return new Response<Airplane>
        {
            StatusCode = 200,
            Message = "Succes",
            Data = GetAll().Data.FirstOrDefault(Data => Data.Id == id)
        };
    }

    public Response<Airplane> Update(Airplane airplane)
    {
        string source = File.ReadAllText(path);

        List<Airplane> airplanes = JsonConvert.DeserializeObject<List<Airplane>>(source);

        Airplane existAirplane = airplanes.FirstOrDefault(l => l.Id.Equals(airplane.Id));

        if (existAirplane is null)
        {
            return new Response<Airplane>
            {
                StatusCode = 404,
                Message = "Not found",
            };
        }

        existAirplane.Id = airplane.Id;
        existAirplane.Model = airplane.Model;
        existAirplane.ModelCount = airplane.ModelCount;
        existAirplane.NumOfSeats = airplane.NumOfSeats;

        string json = JsonConvert.SerializeObject(airplanes, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(path, json);

        return new Response<Airplane>
        {
            StatusCode = 200,
            Message = "Succes",
            Data = airplane
        };
    }
}
