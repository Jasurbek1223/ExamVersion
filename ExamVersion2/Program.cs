using AirplaneManagementApp.BackEnd.ServiceLayer.Panel;

namespace ExamVersion2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UserView userView = new UserView();

            userView.Start();
        }
    }
}