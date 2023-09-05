using AirplaneManagementApp.BackEnd.DataLayer.Constants;
using AirplaneManagementApp.BackEnd.DomainLayer.Model;
using AirplaneManagementApp.BackEnd.ServiceLayer.Helpers;
using AirplaneManagementApp.BackEnd.ServiceLayer.Interface;
using Newtonsoft.Json;

namespace AirplaneManagementApp.BackEnd.ServiceLayer.Service;

public class FlightService : IFlightService
{
    string path = FilePath.FLIGHT_PATH;
    string pathTicket = FilePath.TICKET_PATH;
    string pathAirplane = FilePath.AIRPLANE_PATH;

    public FlightService()
    {
        string result = File.ReadAllText(path);
        if (string.IsNullOrEmpty(result))
        {
            File.WriteAllText(path, "[]");
        }
    }

    public bool CheckNumOfSeats(Flight flight)
    {
        string source = File.ReadAllText(pathTicket);
        List<Ticket> tickets = JsonConvert.DeserializeObject<List<Ticket>>(source);
        var count = tickets.Count(x => x.FlightId.Equals(flight.Id));

        string source1 = File.ReadAllText(pathAirplane);
        List<Airplane> airplanes = JsonConvert.DeserializeObject<List<Airplane>>(source1);
        var countOfSeats = airplanes.FirstOrDefault(a => a.Id == flight.AirplaneId).NumOfSeats;
        if (count < countOfSeats)
        {
            return true;
        }
        return false;

    }

    public Response<Flight> Create(Flight flight, int airplaneId)
    {
        string source = File.ReadAllText(path);
        List<Flight> flights = JsonConvert.DeserializeObject<List<Flight>>(source);

        Flight existFlight = flights.FirstOrDefault(u => u.Id.Equals(flight.Id));

        if (existFlight is not null)
        {
            return new Response<Flight>
            {
                StatusCode = 403,
                Message = "This Flight already exist",
            };
        }
        flight.AirplaneId = airplaneId;
        int lastId;
        try
        {
            lastId = flights.LastOrDefault().Id;
        }
        catch
        {
            lastId = 0;
        }
        flight.Id = lastId + 1;
        flights.Add(flight);

        string json = JsonConvert.SerializeObject(flights, Newtonsoft.Json.Formatting.Indented);

        File.WriteAllText(path, json);

        return new Response<Flight>
        {
            StatusCode = 200,
            Message = "Succes",
            Data = flight
        };
    }

    public Response<bool> Delete(int id)
    {
        string source = File.ReadAllText(path);

        List<Flight> flights = JsonConvert.DeserializeObject<List<Flight>>(source);

        Flight existFlight = flights.FirstOrDefault(l => l.Id.Equals(id));

        if (existFlight is null)
        {
            return new Response<bool>
            {
                StatusCode = 404,
                Message = "Not found",
                Data = false
            };
        }

        flights.Remove(existFlight);
        string json = JsonConvert.SerializeObject(flights, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(path, json);

        return new Response<bool>
        {
            StatusCode = 200,
            Message = "Succes",
            Data = true
        };
    }

    public Response<List<Flight>> GetAll()
    {
        string source = File.ReadAllText(path);

        List<Flight> flights = JsonConvert.DeserializeObject<List<Flight>>(source);

        return new Response<List<Flight>>
        {
            Message = "Succes",
            StatusCode = 200,
            Data = flights
        };
    }

    public Response<Flight> GetById(int id)
    {
        return new Response<Flight>
        {
            StatusCode = 200,
            Message = "Succes",
            Data = GetAll().Data.FirstOrDefault(Data => Data.Id == id)
        };
    }

    public Response<Flight> Update(Flight flight)
    {
        string source = File.ReadAllText(path);

        List<Flight> flights = JsonConvert.DeserializeObject<List<Flight>>(source);

        Flight existFlight = flights.FirstOrDefault(l => l.Id.Equals(flight.Id));

        if (existFlight is null)
        {
            return new Response<Flight>
            {
                StatusCode = 404,
                Message = "Not found",
            };
        }

        existFlight.Id = flight.Id;
        existFlight.FlyFrom = flight.FlyFrom;
        existFlight.FlyTo = flight.FlyTo;
        existFlight.TakeOfDate = flight.TakeOfDate;
        existFlight.Price = flight.Price;
        existFlight.BookedUsers = flight.BookedUsers;
        existFlight.AirplaneId = flight.AirplaneId;


        string json = JsonConvert.SerializeObject(flights, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(path, json);

        return new Response<Flight>
        {
            StatusCode = 200,
            Message = "Succes",
            Data = flight
        };
    }
}
