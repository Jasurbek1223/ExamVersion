using AirplaneManagementApp.BackEnd.DataLayer.Constants;
using AirplaneManagementApp.BackEnd.DomainLayer.Model;
using AirplaneManagementApp.BackEnd.ServiceLayer.Helpers;
using AirplaneManagementApp.BackEnd.ServiceLayer.Interface;
using Newtonsoft.Json;

namespace AirplaneManagementApp.BackEnd.ServiceLayer.Service;

public class TicketService : ITicketService
{
    string path = FilePath.TICKET_PATH;
    string path_cancel = FilePath.CENCEL_PATH;

    public TicketService()
    {
        string result = File.ReadAllText(path);
        string resultCancel = File.ReadAllText(path_cancel);
        if (string.IsNullOrEmpty(result))
        {
            File.WriteAllText(path, "[]");
        }
        if (string.IsNullOrEmpty(resultCancel))
        {
            File.WriteAllText(path_cancel, "[]");
        }
    }
    public Response<Ticket> Create(Ticket ticket)
    {
        string source = File.ReadAllText(path);
        List<Ticket> tickets = JsonConvert.DeserializeObject<List<Ticket>>(source);

        Ticket existTicked = tickets.FirstOrDefault(u => u.Id.Equals(ticket.Id));

        if (existTicked is not null)
        {
            return new Response<Ticket>
            {
                StatusCode = 403,
                Message = "This Flight already exist",
            };
        }
        int lastId;
        try
        {
            lastId = tickets.LastOrDefault().Id;
        }
        catch
        {
            lastId = 0;
        }
        ticket.Id = lastId + 1;
        tickets.Add(ticket);

        string json = JsonConvert.SerializeObject(tickets, Newtonsoft.Json.Formatting.Indented);

        File.WriteAllText(path, json);

        return new Response<Ticket>
        {
            StatusCode = 200,
            Message = "Succes",
            Data = ticket
        };
    }

    public Response<bool> Delete(int id)
    {
        string source = File.ReadAllText(path);

        List<Ticket> tickets = JsonConvert.DeserializeObject<List<Ticket>>(source);

        Ticket? existTicket = tickets.FirstOrDefault(l => l.Id == id);
        if (existTicket != null)
        {
            SaveDelete(existTicket);
        }

        if (existTicket is null)
        {
            return new Response<bool>
            {
                StatusCode = 404,
                Message = "Not found",
                Data = false
            };
        }

        tickets.Remove(existTicket);
        string json = JsonConvert.SerializeObject(tickets, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(path, json);

        return new Response<bool>
        {
            StatusCode = 200,
            Message = "Succes",
            Data = true
        };
    }

    public Response<List<Ticket>> GetAll()
    {
        string source = File.ReadAllText(path);

        List<Ticket> tickets = JsonConvert.DeserializeObject<List<Ticket>>(source);


        return new Response<List<Ticket>>
        {
            Message = "Succes",
            StatusCode = 200,
            Data = tickets
        };
    }

    public List<Ticket> GetAllList()
    {
        string source = File.ReadAllText(path);

        List<Ticket> tickets = JsonConvert.DeserializeObject<List<Ticket>>(source);

        return tickets.ToList();
    }




    public Response<Ticket> GetById(int id)
    {
        return new Response<Ticket>
        {
            StatusCode = 200,
            Message = "Succes",
            Data = GetAll().Data.FirstOrDefault(Data => Data.Id == id)
        };
    }

    public Response<Ticket> Update(Ticket ticket)
    {
        string source = File.ReadAllText(path);

        List<Ticket> tickets = JsonConvert.DeserializeObject<List<Ticket>>(source);

        Ticket existTicket = tickets.FirstOrDefault(l => l.Id.Equals(ticket.Id));

        if (existTicket is null)
        {
            return new Response<Ticket>
            {
                StatusCode = 404,
                Message = "Not found",
            };
        }

        existTicket.Id = ticket.Id;
        existTicket.OwnerFullName = ticket.OwnerFullName;
        existTicket.UserId = ticket.UserId;
        existTicket.FlightTime = ticket.FlightTime;
        existTicket.FlightId = ticket.FlightId;

        string json = JsonConvert.SerializeObject(tickets, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(path, json);

        return new Response<Ticket>
        {
            StatusCode = 200,
            Message = "Succes",
            Data = ticket
        };
    }

    public void SaveDelete(Ticket ticket)
    {
        string source = File.ReadAllText(path_cancel);
        List<Ticket> tickets = new();
        if (string.IsNullOrEmpty(source))
        {
            tickets = JsonConvert.DeserializeObject<List<Ticket>>(source);
        }

        tickets.Add(ticket);
        string json = JsonConvert.SerializeObject(tickets, Newtonsoft.Json.Formatting.Indented);

        File.WriteAllText(path_cancel, json);

    }
}
