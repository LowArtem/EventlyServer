using System.Text.RegularExpressions;

namespace EventlyServer.Extensions;

public static class ValidationExtensions
{
    public static bool ValidateAsEmail(this string str)
    {
        var regex = new Regex("^[a-zA-Z\\d_!#$%&'*+/=?`{|}~^.-]+@[a-zA-Z\\d.-]+$");
        return regex.IsMatch(str);
    }
    
    public static bool ValidateAsPhoneNumber(this string str)
    {
        var regex = new Regex("^\\d{11}$");
        return regex.IsMatch(str);
    }
}