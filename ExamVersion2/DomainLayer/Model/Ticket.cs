namespace AirplaneManagementApp.BackEnd.DomainLayer.Model;

public class Ticket
{
    public int Id { get; set; }
    public string OwnerFullName { get; set; }
    public int UserId { get; set; }
    public DateTime FlightTime { get; set; }
    public int FlightId { get; set; }
}
