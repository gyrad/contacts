using System;

namespace Contacts;

public class Contact
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }

    public Contact(string name, string phone, string email)
    {
        Name = name;
        Phone = phone;
        Email = email;
    }

    public override string ToString()
    {
        return $"{Name, -26}"
            + $"{(Phone == string.Empty ? '-' : Phone), -26}"
            + $"{(Email == string.Empty ? '-' : Email), -26}";
    }
}
