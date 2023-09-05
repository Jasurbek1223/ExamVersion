using AirplaneManagementApp.BackEnd.DomainLayer.Enums;

namespace AirplaneManagementApp.BackEnd.DomainLayer.Model;

public class User
{
    public int Id { get; set; }
    public UserRole Role { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public List<int> BookedFlight { get; set; } = new List<int>();
}
