using ToDoList.Enums;
using ToDoList.Interfaces;

namespace ToDoList.Models;

public class ItemFactory
{
    private readonly IUserInterface _ui;

    public ItemFactory(IUserInterface ui)
    {
        _ui = ui;
    }

    public ToDoItem Create()
    {
        var title = _ui.AskUser("Enter title:");
        var description = _ui.AskUser("Enter description (Optional, press enter to skip):");
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

    private  DateTime AskDateTime()
    {
        var input = _ui.AskUser("Enter date (format dd/MM/yyyy HH:mm):");
        while (!DateTime.TryParseExact(
                   input, "dd/MM/yyyy HH:mm", null,
                   System.Globalization.DateTimeStyles.None,
                   out _))
        {
            input = _ui.AskUser("Invalid date, please try again (format dd/MM/yyyy HH:mm):");
        }
        DateTime.TryParseExact(input, "dd/MM/yyyy HH:mm", null,
            System.Globalization.DateTimeStyles.None, out DateTime date);
        return date;
    }

    private ToDoItemTypes AskItemType()
    {
        _ui.PrintMessage("What type of item would you like to add?");
        foreach (var option in Enum.GetValues<ToDoItemTypes>())
            _ui.PrintMessage($"{(int)option}. {option}");
        while (true)
        {
            if (int.TryParse(_ui.AskUser(""), out int type) &&
                Enum.IsDefined(typeof(ToDoItemTypes), type))
                return (ToDoItemTypes)type;
            _ui.PrintMessage("Invalid input, please try again:");
        }
    }

    private Call CreateCall(string title, string description, DateTime date)
    {
        var invitedPersons = _ui.ReadUserNames("Enter invited person(s) (separate by comma):");
        return new Call(
            targetDayTime: date,
            itemType: ToDoItemTypes.Call,
            description: description,
            title: title,
            isCompleted: false,
            invitedPerson: invitedPersons);
    }

    private Meeting CreateMeeting(string title, string description, DateTime date)
    {
        var invitedPersons = _ui.ReadUserNames("Enter invited person(s) (separate by comma):");
        var place = _ui.AskUser("Enter meeting place:");
        return new Meeting(
            targetDayTime: date,
            itemType: ToDoItemTypes.Meeting,
            description: description,
            title: title,
            isCompleted: false,
            invitedPerson: invitedPersons,
            place: place);
    }

    private DateOfBirth CreateDayOfBirth(string title, string description, DateTime date)
    {
        var name = _ui.AskUser("Enter the name of person who's celebrating:");
        return new DateOfBirth(
            targetDayTime: date,
            itemType: ToDoItemTypes.DayOfBirth,
            description: description,
            title: title,
            isCompleted: false,
            nameOfPersonWithDoB: name);
    }

    private ToDoTask CreateTask(string title, string description, DateTime date)
    {
        return new ToDoTask(
            targetDayTime: date,
            itemType: ToDoItemTypes.Task,
            description: description,
            title: title,
            isCompleted: false);
    }
}