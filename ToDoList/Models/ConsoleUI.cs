using ToDoList.Enums;
using ToDoList.Interfaces;

namespace ToDoList.Models;

public class ConsoleUi : IUserInterface
{
    public void PrintMessage(string message) => 
        Console.WriteLine(message);

    public void PrintSeparator() =>
        Console.WriteLine("\n---------------------------\n");

    public string AskUser(string prompt)
    {
        Console.WriteLine(prompt);
        return Console.ReadLine() ?? string.Empty;
    }

    public List<string> ReadUserNames(string prompt) =>
        AskUser(prompt).Split(',').ToList();

    public DateTime AskDate(string prompt)
    {
        var input = AskUser(prompt);
        DateTime date;
        while (!DateTime.TryParseExact(
                   input, "dd/MM/yyyy", null,
                   System.Globalization.DateTimeStyles.None,
                   out date))
        {
            input = AskUser("Invalid date, please try again (format dd/MM/yyyy):");
        }
        return date;
    }

    public int PrintAvailableOptions()
    {
        PrintSeparator();
        Console.WriteLine("What do you want to do?");
        foreach (var option in Enum.GetValues<MenuOptions>())
            Console.WriteLine($"{(int)option}. {option}");
        PrintSeparator();
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int choice) &&
                Enum.IsDefined(typeof(MenuOptions), choice))
                return choice;
            Console.WriteLine("Invalid input, please try again:");
        }
    }

    public DisplayOptions PrintDisplayOptions()
    {
        PrintSeparator();
        Console.WriteLine("What would you like to display?");
        foreach (var option in Enum.GetValues<DisplayOptions>())
            Console.WriteLine($"{(int)option}. {option}");
        PrintSeparator();
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int choice) &&
                Enum.IsDefined(typeof(DisplayOptions), choice))
                return (DisplayOptions)choice;
            Console.WriteLine("Invalid input, please try again:");
        }
    }

    public void PrintItems(List<ToDoItem> items)
    {
        if (!items.Any())
        {
            Console.WriteLine("No items to display.");
            return;
        }
        foreach (var item in items)
            Console.WriteLine(item);
    }

    public void PrintItemsByDateRange(DateTime startDate, DateTime endDate, List<ToDoItem> items)
    {
        Console.WriteLine($"Results from {startDate:dd/MM/yyyy} to {endDate:dd/MM/yyyy}:");
        PrintItems(items);
    }

    public void PrintItemsBySpecificDate(DateTime date, List<ToDoItem> items)
    {
        Console.WriteLine($"Results for {date:dd/MM/yyyy}:");
        PrintItems(items);
    }

    public void PrintEvents(List<ToDoEvent> events)
    {
        if (!events.Any())
        {
            Console.WriteLine("No events to display.");
            return;
        }
        Console.WriteLine("List of events:");
        foreach (var e in events) 
            Console.WriteLine($"{e.Date:dd/MM/yyyy HH:mm} - {e.EventType} - {e.Title}");
    }
}