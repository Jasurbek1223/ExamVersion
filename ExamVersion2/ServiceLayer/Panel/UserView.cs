using AirplaneManagementApp.BackEnd.DomainLayer.Model;
using AirplaneManagementApp.BackEnd.ServiceLayer.Service;

namespace AirplaneManagementApp.BackEnd.ServiceLayer.Panel;

public class UserView
{
    public UserService userService = new UserService();
    public FlightService flightService = new FlightService();
    public AirplaneService airplaneService = new AirplaneService();
    public TicketService ticketService = new TicketService();

    public void Start()
    {
        Console.WriteLine("Kirish:");

        Console.WriteLine("1. Royhatdan otish");
        Console.WriteLine("2. Kirish");
        Console.Write("Tanlov: ");
        int loginChoice = default;

        try
        {
            loginChoice = Convert.ToInt32(Console.ReadLine());
        }
        catch
        {
            Console.WriteLine("Notog'ri tanlov");
            Start();
        }

        if (loginChoice == 1)
        {
            Registration();
        }
        else if (loginChoice == 2)
        {
            Login();
        }
        else
        {
            Console.WriteLine("Noto'g'ri tanlov!");
            Start();
        }
    }

    public void Registration()
    {
        User user = new User();
        Console.Write("First Name: ");
        user.FirstName = Console.ReadLine();
        Console.Write("Last Name: ");
        user.LastName = Console.ReadLine();
        Console.Write("Phone Number(Exp:+998998887766): ");
        user.PhoneNumber = Console.ReadLine();
        Console.Write("Email Address: ");
        user.Email = Console.ReadLine();
        Console.Write("Password: ");
        user.Password = Console.ReadLine();

        var checkUser = userService.Create(user);
        Console.WriteLine(checkUser.Message);
        Console.Clear();
        Start();
    }

    public void Login()
    {
        try
        {
            Console.Write("Login(email): ");
            string login = Console.ReadLine();

            Console.Write("Parol: ");
            string password = Console.ReadLine();

            var user = userService.GetByEmail(login);

            if (user != null && password == user.Data.Password)
                ShowUserMenu(user.Data.Id);
        }
        catch
        {
            Console.WriteLine("Xato kiritildi");
            Start();
        }
    }

    public void ShowUserMenu(int userId)
    {
        Console.WriteLine("Samolet boshqarish tizimiga xush kelibsiz!");

        while (true)
        {
            Console.WriteLine("\n1. Parvozlar ro'yxatini ko'rish");
            Console.WriteLine("2. Bilet bron qilish");
            Console.WriteLine("3. Olingan biletni bekor qilish");
            Console.WriteLine("4. Chiqish");

            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    var flightList = flightService.GetAll().Data;
                    ShowFlights(flightList);
                    break;
                case 2:
                    var flights = flightService.GetAll().Data;
                    ShowFlights(flights);
                    ReserveTicket(flights.FirstOrDefault().Id, flights.Count, userId);
                    break;
                case 3:
                    CancelTicket(userId);
                    break;
                case 4:
                    Console.WriteLine("Dastur tugadi.");
                    return;
                default:
                    Console.WriteLine("Noto'g'ri tanlov! Iltimos, qayta urinib ko'ring.");
                    break;
            }
        }
    }

    public void ShowFlights(List<Flight> flights)
    {
        foreach (Flight flight in flights)
        {
            Console.WriteLine($"Parvoz raqami: {flight.Id}, FlyFrom: {flight.FlyFrom}, FlyTo: {flight.FlyTo}," +
                $" Take of Date: {flight.TakeOfDate}, Price: {flight.Price}");
        }
    }

    public void ReserveTicket(int firstIndex, int size, int userId)
    {

        Console.WriteLine("\nBilet bron qilish");
        int flightNumber = default;

        Console.Write("Parvoz raqami: ");
        try
        {
            flightNumber = int.Parse(Console.ReadLine());
        }
        catch
        {
            Console.WriteLine("String kiritdingiz");
            ReserveTicket(firstIndex, size, userId);
        }
        var flight = flightService.GetById(flightNumber).Data;

        if (flightNumber >= firstIndex && flightNumber <= size + firstIndex - 1 && flightService.CheckNumOfSeats(flight))
        {
            var users = userService.GetAll();
            var user = users.Data.FirstOrDefault(d => d.Id == userId);
            Ticket ticket = new Ticket();
            ticket.Id = 0;
            ticket.OwnerFullName = user.FirstName + " " + user.LastName;
            ticket.FlightTime = DateTime.Now.AddDays(2);
            ticket.UserId = userId;
            ticket.FlightId = flightNumber;
            ticketService.Create(ticket);
        }
        else
        {
            Console.WriteLine("Mavjud emas");
            ShowUserMenu(userId);
        }

        Console.WriteLine("Bilet muvaffaqiyatli bron qilindi!");
    }

    public void CancelTicket(int userId)
    {
        var mustUserId = userId;
        var ticetServiceA = new TicketService();
        Console.WriteLine("\nOlingan biletni bekor qilish:");
        var tickets = ticetServiceA.GetAllList();
        if (tickets == null || tickets.Count == 0)
        {
            Console.WriteLine("Bu id ga biletlar mavjud emas");
            return;
        }
        tickets.Where(item => item.UserId == userId).ToList().ForEach(item => Console.WriteLine($"Id: {item.Id}, " +
            $"FullName: {item.OwnerFullName}, Flight Time: {item.FlightTime}"));
        int flightNumber = default;

        Console.Write("Bilet raqami: ");
        try
        {
            flightNumber = int.Parse(Console.ReadLine());
        }
        catch
        {
            Console.WriteLine("Noto'g'ri malumot kiritildi");
            CancelTicket(userId);
        }
        ticketService.Delete(flightNumber);
    }

    static void ShowAdminMenu()
    {
        Console.WriteLine("Samolet boshqarish tizimiga xush kelibsiz!");

        while (true)
        {
            Console.WriteLine("1. Yangi samolyot qo'shish");
            Console.WriteLine("2. Samolyotlar ro'yxatini ko'rish");
            Console.WriteLine("3. Chiqish");

            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    AddAirplane();
                    break;
                case 2:
                    ShowAirplanes();
                    break;
                case 3:
                    Console.WriteLine("Dastur tugadi.");
                    return;
                default:
                    Console.WriteLine("Noto'g'ri tanlov! Iltimos, qayta urinib ko'ring.");
                    break;
            }
        }
    }

    static void AddAirplane()
    {
        Console.WriteLine("Samolyot raqamini kiriting:");
        string airplaneNumber = Console.ReadLine();

        Console.WriteLine("Samolyot turi:");
        string airplaneType = Console.ReadLine();

        string airplaneInfo = $"{airplaneNumber},{airplaneType}";

        // Faylga ma'lumotlarni saqlash
        using (StreamWriter writer = File.AppendText("samolyotlar.txt"))
        {
            writer.WriteLine(airplaneInfo);
        }

        Console.WriteLine("Samolyot ma'lumotlari saqlandi!");
    }

    static void ShowAirplanes()
    {
        Console.WriteLine("Samolyotlar ro'yxati:");

        if (File.Exists("samolyotlar.txt"))
        {
            string[] lines = File.ReadAllLines("samolyotlar.txt");
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                string airplaneNumber = parts[0];
                string airplaneType = parts[1];
                Console.WriteLine($"Samolyot raqami: {airplaneNumber}, Samolyot turi: {airplaneType}");
            }
        }
        else
        {
            Console.WriteLine("Hozircha samolyotlar mavjud emas.");
        }
    }
}
