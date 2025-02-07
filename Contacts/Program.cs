﻿using System.Text;
using System.Text.RegularExpressions;
using Contacts;

Console.Clear();
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("****************");
Console.WriteLine("**  CONTACTS  **");
Console.WriteLine("****************\n");
Console.ResetColor();
DisplayMainMenu();

int selectedOption;
List<Contact> contacts = new List<Contact>()
{
    new Contact("John Doe", "123-456-7890", "john@gmail.com"),
    new Contact("Jane Doe", "987-654-3210", "jane@hotmail.com"),
};

do
{
    Console.Write("\nEnter an option: ");
    selectedOption = int.TryParse(Console.ReadLine(), out int option) ? option : 0;

    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    switch (selectedOption)
    {
        case 1:
            ListContacts(contacts);
            break;
        case 2:
            SearchContacts(contacts);
            break;
        case 3:
            AddContact(contacts);
            break;
        case 4:
            EditContact(contacts);
            break;
        case 5:
            DeleteContact(contacts);
            break;
        case 6:
            ListContacts(contacts, true);
            break;
        case 9:
            break;
        default:
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("-- Invalid option --");
            break;
    }
    Console.ResetColor();
    Console.WriteLine();

    if (selectedOption != 9)
    {
        DisplayMainMenu();
    }
} while (selectedOption != 9);

Console.WriteLine("\nGoodbye!\n");

void DisplayMainMenu()
{
    Console.WriteLine("What would you like to do?");
    Console.WriteLine("1. List contacts");
    Console.WriteLine("2. Search Contacts");
    Console.WriteLine("3. Add a contact");
    Console.WriteLine("4. Edit a contact");
    Console.WriteLine("5. Delete a contact");
    Console.WriteLine("6. Export contacts to file");
    Console.WriteLine("9. Exit");
}

void ListContacts(List<Contact> contacts, bool saveToFile = false)
{
    var stringBuilder = new StringBuilder();
    string header = string.Format("{0,-26}{1,-26}{2,-26}", "Name", "Phone", "Email");
    string separator = new string('-', 78);
    stringBuilder.AppendLine(header);
    stringBuilder.AppendLine(separator);

    foreach (var contact in contacts)
    {
        stringBuilder.AppendLine(contact.ToString());
    }

    Console.WriteLine(stringBuilder.ToString());

    if (saveToFile)
    {
        string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "contacts.txt");
        File.WriteAllText(fileName, stringBuilder.ToString());
        Console.WriteLine($"Contacts saved to {fileName}");
    }
}

void SearchContacts(List<Contact> contacts)
{
    string? searchName;
    do
    {
        Console.Write("Enter a name to search for: ");
        searchName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(searchName))
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Search term cannot be empty");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
        }
    } while (string.IsNullOrWhiteSpace(searchName));

    Console.WriteLine();
    var foundContacts = contacts
        .Where(c => c.Name.Contains(searchName, StringComparison.CurrentCultureIgnoreCase))
        .ToList();
    if (foundContacts.Count == 0)
    {
        Console.WriteLine("No contacts found");
    }
    else
    {
        Console.WriteLine(
            $"{foundContacts.Count} contact{(foundContacts.Count > 1 ? "s" : "")} found\n"
        );
        ListContacts(foundContacts);
    }
}

void AddContact(List<Contact> contacts)
{
    string? name;
    bool hasValidationError;
    do
    {
        hasValidationError = false;
        Console.Write("Enter a name: ");
        name = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(name))
        {
            hasValidationError = true;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Name cannot be empty");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
        }
        else if (contacts.Any(c => c.Name == name))
        {
            hasValidationError = true;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Name already exists. Please enter a different name.");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            name = string.Empty;
        }
        else
        {
            hasValidationError = false;
        }
    } while (hasValidationError);

    string? phone;
    do
    {
        hasValidationError = false;
        Console.Write("Enter a phone number: ");
        phone = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(phone))
        {
            phone = string.Empty;
        }
        else
        {
            if (Regex.IsMatch(phone, @"^\d{10}$"))
            {
                phone = $"{phone.Substring(0, 3)}-{phone.Substring(3, 3)}-{phone.Substring(6)}";
            }

            if (!Regex.IsMatch(phone, @"^\d{3}-\d{3}-\d{4}$"))
            {
                hasValidationError = true;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Phone number must be in the format: 123-456-7890");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                phone = string.Empty;
            }
            else if (contacts.Any(c => c.Phone == phone))
            {
                hasValidationError = true;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(
                    "Phone number already exists. Please enter a different phone number."
                );
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                phone = string.Empty;
            }
            else
            {
                hasValidationError = false;
            }
        }
    } while (hasValidationError);

    string? email;
    do
    {
        hasValidationError = false;
        Console.Write("Enter an email address: ");
        email = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(email))
        {
            email = string.Empty;
        }
        else
        {
            if (!Regex.IsMatch(email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                hasValidationError = true;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Invalid email address");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                email = string.Empty;
            }
            else if (contacts.Any(c => c.Email == email))
            {
                hasValidationError = true;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(
                    "Email address already exists. Please enter a different email address."
                );
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                email = string.Empty;
            }
            else
            {
                hasValidationError = false;
            }
        }
    } while (hasValidationError);

    contacts.Add(new Contact(name, phone, email));
    Console.WriteLine("\nContact added successfully\n");
}

void EditContact(List<Contact> contacts)
{
    Console.WriteLine("\nEdit a contact\n");

    bool hasValidationError = false;
    string name;
    do
    {
        Console.Write("Enter the name of the contact to edit: ");
        name = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(name))
        {
            hasValidationError = true;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Name is required");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
        }
        else
        {
            hasValidationError = false;
        }
    } while (hasValidationError);

    Contact? contact = contacts.FirstOrDefault(c =>
        c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
    );
    if (contact is null)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("Contact not found");
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        return;
    }

    Console.WriteLine("\nEnter new information (Leave empty to use existing value) \n");

    string newName;
    do
    {
        newName = ReadLine.Read($"Name ({contact.Name}): ", contact.Name);
        if (contacts.Any(c => c != contact && c.Name == newName))
        {
            hasValidationError = true;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Name already exists. Please enter a different name.");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
        }
        else
        {
            hasValidationError = false;
            contact.Name = newName;
        }
    } while (hasValidationError);

    string newEmail;
    do
    {
        newEmail = ReadLine.Read($"Email ({contact.Email}): ", contact.Email);
        if (!string.IsNullOrWhiteSpace(newEmail))
        {
            if (!Regex.IsMatch(newEmail, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                hasValidationError = true;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Invalid email address");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                newEmail = string.Empty;
            }
            else if (contacts.Any(c => c != contact && c.Email == newEmail))
            {
                hasValidationError = true;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(
                    "Email address already exists. Please enter a different email address."
                );
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                newEmail = string.Empty;
            }
            else
            {
                hasValidationError = false;
                contact.Email = newEmail;
            }
        }
        else
        {
            hasValidationError = false;
            contact.Email = newEmail;
        }
    } while (hasValidationError);

    string newPhone;
    do
    {
        newPhone = ReadLine.Read($"Phone ({contact.Phone}): ", contact.Phone);
        if (!string.IsNullOrWhiteSpace(newPhone))
        {
            if (Regex.IsMatch(newPhone, @"^\d{10}$"))
            {
                hasValidationError = false;
                newPhone = $"{newPhone[..3]}-{newPhone[3..6]}-{newPhone[6..]}";
                contact.Phone = newPhone;
            }
            else if (!Regex.IsMatch(newPhone, @"^\d{3}-\d{3}-\d{4}$"))
            {
                hasValidationError = true;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Phone is invalid. Use the format: 123-456-7890");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            }
            else if (contacts.Any(c => c != contact && c.Phone == newPhone))
            {
                hasValidationError = true;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(
                    "Phone number already exists. Please enter a different phone number."
                );
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                newPhone = string.Empty;
            }
            else
            {
                hasValidationError = false;
                contact.Phone = newPhone;
            }
        }
        else
        {
            hasValidationError = false;
            contact.Phone = newPhone;
        }
    } while (hasValidationError);
}

void DeleteContact(List<Contact> contacts)
{
    Console.WriteLine("\nDelete a contact\n");

    Console.Write("Enter the name of the contact to delete: ");
    string name = Console.ReadLine() ?? string.Empty;

    Contact? contact = contacts.FirstOrDefault(c =>
        c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
    );
    if (contact is null)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("Contact not found");
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        return;
    }

    contacts.Remove(contact);
    Console.ForegroundColor = ConsoleColor.DarkRed;
    Console.WriteLine($"{contact.Name} has been deleted from your contacts");
    Console.ForegroundColor = ConsoleColor.DarkGreen;

    Console.WriteLine("\nUpdated contacts list\n");
    ListContacts(contacts);
}
