using ToDoList.Enums;
using ToDoList.Models;

var toDoList1 = new ToDoList.Models.ToDoList();
toDoList1.LoadItems();
while(true)
{
    var choice = (PrintAvailableOptions());
    var choiceType = (MenuOptions)choice;
    switch(choiceType)
    {
        case MenuOptions.AddItem: 
            AddItemInList();
            toDoList1.SaveItems();
            break;
        case MenuOptions.RemoveItem:
        {
            var title = AskUser ("Enter the title of the item you want to remove");
            toDoList1.RemoveItem(title);
            toDoList1.SaveItems();
            break;
        }
        case MenuOptions.CompleteItem:
        {
            var title = AskUser ("Enter the title of the item you want to complete");
            toDoList1.CompleteItem(title);
            toDoList1.SaveItems();
            break;
            
        }
        case MenuOptions.DisplayAll:
        {
            toDoList1.PrintAllItems();
            break;
        }
        case MenuOptions.DisplayOnlyActive:
        {
            toDoList1.PrintOnlyNotCompletedItems();
            break;
        }
        case MenuOptions.Exit:
            return;
        default:
            Console.WriteLine("Invalid choice, please try again:");
                break;
    }
}
void PrintSeparator()
{
    Console.WriteLine("\n---------------------------\n");
}

string AskUser(string prompt)
{
    Console.WriteLine(prompt);
    return Console.ReadLine() ?? string.Empty;
}

int PrintAvailableOptions()
{
    PrintSeparator();
    Console.WriteLine("\nWhat do you want to do?");
    foreach (var option in Enum.GetValues<MenuOptions>())
        Console.WriteLine($"{(int)option}. {option}");
    PrintSeparator();
    while (true)
    {
        if (int.TryParse(Console.ReadLine(), out int choice))
            return choice;
        Console.WriteLine("Invalid input, please try again:");
    }
}

List<string> ReadUserNames(string prompt)
    {
        return AskUser(prompt).Split(',').ToList();
    }

void AddItemInList()
{
    var title = AskUser("Enter title:");
    var description = AskUser("Enter description (Optional, press enter to skip):");
    Console.WriteLine("Enter date (format dd/MM/yyyy HH:mm):");
    DateTime targetDayTime;
    while (!DateTime.TryParseExact
               (Console.ReadLine(),
                   "dd/MM/yyyy HH:mm",
                   null, 
                   System.Globalization.DateTimeStyles.None, 
                   out targetDayTime))
    {
        Console.WriteLine("Invalid date, please try again (format dd/MM/yyyy HH:mm):");
    }
    Console.WriteLine("What type of item would you like to add?");
    foreach (var option in Enum.GetValues<ToDoItemTypes>())
        Console.WriteLine($"{(int)option}. {option}");
    int type;
    while (true)
    {
        if (int.TryParse(Console.ReadLine(), out type))
            break;

        Console.WriteLine("Invalid input, please enter a number:");
    }
    var itemType = (ToDoItemTypes)type;
    switch (itemType)
    {
        case ToDoItemTypes.Call:
        {
            var invitedPersons = ReadUserNames("Enter invited for the Call person(s) (Separate multiple persons by comma");
            toDoList1.AddItem(new Call(
                targetDayTime: targetDayTime,
                itemType: ToDoItemTypes.Call,
                description: description,
                title: title,
                isCompleted: false,
                invitedPerson: invitedPersons
            ));
            Console.WriteLine($"Item {title} has been added");
            break;
        }
        case ToDoItemTypes.Meeting:
        {
            var invitedPersons = ReadUserNames("Enter invited for the Meeting person(s)(Separate multiple persons by comma)");
            var place = AskUser("Enter meeting place");
            toDoList1.AddItem(new Meeting(
                targetDayTime: targetDayTime,
                itemType: ToDoItemTypes.Meeting,
                description: description,
                title: title,
                isCompleted: false,
                invitedPerson: invitedPersons,
                place: place
            ));
            Console.WriteLine($"Item {title} has been added");
            break;
        }
        case ToDoItemTypes.DayOfBirth:
        {
            var nameOfPersonWithDoD = AskUser ("Enter the name of person who's celebrating");
            toDoList1.AddItem(new DateOfBirth(
                targetDayTime: targetDayTime,
                itemType: ToDoItemTypes.DayOfBirth,
                description: description,
                title: title,
                isCompleted: false,
                nameOfPersonWithDoB: nameOfPersonWithDoD
            ));
            Console.WriteLine($"Item {title} has been added");
            break;
        }
        case ToDoItemTypes.Task:
        {
            toDoList1.AddItem(new ToDoList.Models.Task(
                targetDayTime: targetDayTime,
                itemType: ToDoItemTypes.Task,
                description: description,
                title: title,
                isCompleted: false
            ));
            Console.WriteLine($"Item {title} has been added");
            break;
        }
        default:
        {
            Console.WriteLine("Invalid option, please try again:");
            break;
        }

    }
}
