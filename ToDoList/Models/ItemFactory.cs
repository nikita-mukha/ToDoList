using ToDoList.Enums;

namespace ToDoList.Models;

public static class ItemFactory
{
    public static ToDoItem Create()
    {
        var title = ConsoleUi.AskUser("Enter title:");
        var description = ConsoleUi.AskUser("Enter description (Optional, press enter to skip):");
        var targetDayTime = AskDateTime();
        var itemType = AskItemType();

        return itemType switch
        {
            ToDoItemTypes.Call => CreateCall(title, description, targetDayTime),
            ToDoItemTypes.Meeting => CreateMeeting(title, description, targetDayTime),
            ToDoItemTypes.DayOfBirth => CreateDayOfBirth(title, description, targetDayTime),
            ToDoItemTypes.Task => CreateTask(title, description, targetDayTime),
            _ => throw new ArgumentException("Invalid item type")
        };
    }

    private static DateTime AskDateTime()
    {
        var input = ConsoleUi.AskUser("Enter date (format dd/MM/yyyy HH:mm):");
        while (!DateTime.TryParseExact(
                   input, "dd/MM/yyyy HH:mm", null,
                   System.Globalization.DateTimeStyles.None,
                   out _))
        {
            input = ConsoleUi.AskUser("Invalid date, please try again (format dd/MM/yyyy HH:mm):");
        }
        DateTime.TryParseExact(input, "dd/MM/yyyy HH:mm", null,
            System.Globalization.DateTimeStyles.None, out DateTime date);
        return date;
    }

    private static ToDoItemTypes AskItemType()
    {
        Console.WriteLine("What type of item would you like to add?");
        foreach (var option in Enum.GetValues<ToDoItemTypes>())
            Console.WriteLine($"{(int)option}. {option}");
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int type) &&
                Enum.IsDefined(typeof(ToDoItemTypes), type))
                return (ToDoItemTypes)type;
            Console.WriteLine("Invalid input, please try again:");
        }
    }

    private static Call CreateCall(string title, string description, DateTime date)
    {
        var invitedPersons = ConsoleUi.ReadUserNames("Enter invited person(s) (separate by comma):");
        return new Call(
            targetDayTime: date,
            itemType: ToDoItemTypes.Call,
            description: description,
            title: title,
            isCompleted: false,
            invitedPerson: invitedPersons);
    }

    private static Meeting CreateMeeting(string title, string description, DateTime date)
    {
        var invitedPersons = ConsoleUi.ReadUserNames("Enter invited person(s) (separate by comma):");
        var place = ConsoleUi.AskUser("Enter meeting place:");
        return new Meeting(
            targetDayTime: date,
            itemType: ToDoItemTypes.Meeting,
            description: description,
            title: title,
            isCompleted: false,
            invitedPerson: invitedPersons,
            place: place);
    }

    private static DateOfBirth CreateDayOfBirth(string title, string description, DateTime date)
    {
        var name = ConsoleUi.AskUser("Enter the name of person who's celebrating:");
        return new DateOfBirth(
            targetDayTime: date,
            itemType: ToDoItemTypes.DayOfBirth,
            description: description,
            title: title,
            isCompleted: false,
            nameOfPersonWithDoB: name);
    }

    private static Task CreateTask(string title, string description, DateTime date)
    {
        return new Task(
            targetDayTime: date,
            itemType: ToDoItemTypes.Task,
            description: description,
            title: title,
            isCompleted: false);
    }
}