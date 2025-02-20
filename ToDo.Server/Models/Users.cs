using System.Text.RegularExpressions;

namespace ToDo.Server.Models;

public class Users
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    private string _password;
    public string Password
    {
        get => _password;
        set
        {
            if (value != null)
            {
                if (Regex.IsMatch(value, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{9,}$"))
                {
                    _password = value;
                }
            }
            else
            {
                throw new ArgumentException("Password do not meet complexity requirements.");
            }
        }
    }
    public DateTime CreatedAt { get; set; }
}
