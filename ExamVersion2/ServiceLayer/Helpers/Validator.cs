using System.Text.RegularExpressions;

namespace AirplaneManagementApp.BackEnd.ServiceLayer.Helpers;

public static class Validator
{
    public static bool CheckEmail(this string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        var emailAdressRegex = new Regex(pattern);

        return emailAdressRegex.IsMatch(email) ? true : false;
    }
   
    public static bool CheckName(this string name)
    {
        string pattern = "^[A-Za-z][A-Za-z0-9]*$";
        var emailAdressRegex = new Regex(pattern);

        return emailAdressRegex.IsMatch(name) ? true : false;
    }

    public static bool CheckNumber(this string number)
    {
        string pattern = @"^\+998\d{9}$";
        var emailAdressRegex = new Regex(pattern);

        return emailAdressRegex.IsMatch(number) ? true : false;
    }
}
