using System.Xml.Linq;

namespace AirplaneManagementApp.BackEnd.DataLayer.Constants;

public static class FilePath
{
    static string currentDirectory = Directory.GetCurrentDirectory();
    static string parentDirectory = Directory.GetParent(currentDirectory).Parent.Parent.ToString();

    public static string USER_PATH = Path.Combine(parentDirectory, "DataLayer", "Files", "users.txt");
    public static string CENCEL_PATH = Path.Combine(parentDirectory, "DataLayer", "Files", "cancellationFly.txt");
    public static string AIRPLANE_PATH = Path.Combine(parentDirectory, "DataLayer", "Files", "airplane.txt");
    public static string TICKET_PATH = Path.Combine(parentDirectory, "DataLayer", "Files", "ticket.txt");
    public static string FLIGHT_PATH = Path.Combine(parentDirectory, "DataLayer", "Files", "flights.txt");

}
