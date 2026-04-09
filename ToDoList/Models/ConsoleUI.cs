using ToDoList.Enums;

namespace ToDoList.Models;

public static class ConsoleUi
{
    public static void PrintSeparator() =>
        Console.WriteLine("\n---------------------------\n");

    public static string AskUser(string prompt)
    {
        Console.WriteLine(prompt);
        return Console.ReadLine() ?? string.Empty;
    }

    public static List<string> ReadUserNames(string prompt) =>
        AskUser(prompt).Split(',').ToList();

    public static DateTime AskDate(string prompt)
    {
        var input = AskUser(prompt);
        while (!DateTime.TryParseExact(
                   input, "dd/MM/yyyy", null,
                   System.Globalization.DateTimeStyles.None,
                   out _))
        {
            input = AskUser("Invalid date, please try again (format dd/MM/yyyy):");
        }
        DateTime.TryParseExact(input, "dd/MM/yyyy", null,
            System.Globalization.DateTimeStyles.None, out DateTime date);
        return date;
    }

    public static int PrintAvailableOptions()
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

    public static DisplayOptions PrintDisplayOptions()
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

    public static void PrintItems(List<ToDoItem> items)
    {
        if (!items.Any())
        {
            Console.WriteLine("No items to display.");
            return;
        }
        foreach (var item in items)
            Console.WriteLine(item);
    }

    public static void PrintItemsByDateRange(DateTime startDate, DateTime endDate, List<ToDoItem> items)
    {
        Console.WriteLine($"Results from {startDate:dd/MM/yyyy} to {endDate:dd/MM/yyyy}:");
        PrintItems(items);
    }

    public static void PrintItemsBySpecificDate(DateTime date, List<ToDoItem> items)
    {
        Console.WriteLine($"Results for {date:dd/MM/yyyy}:");
        PrintItems(items);
    }
}