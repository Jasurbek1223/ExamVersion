namespace AirplaneManagementApp.BackEnd.DomainLayer.Model;

public class Flight
{
    public int Id { get; set; }
    public string FlyFrom { get; set; }
    public string FlyTo { get; set; }
    public DateTime TakeOfDate { get; set; }
    public int Price { get; set; }
    public List<int> BookedUsers { get; set; } = new List<int>();
    public int AirplaneId { get; set; }
}

