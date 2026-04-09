using ToDoList.Enums;
using ToDoList.Models;

var toDoList1 = new ToDoList.Models.ToDoList();
toDoList1._items = ToDoStorage.Load();

while(true)
{
    var choice = int.Parse(PrintAvailableOptions());
    var choiceType = (MenuOptions)choice;
    switch(choiceType)
    {
        case MenuOptions.AddItem: 
            AddItemInList();
            ToDoStorage.Save(toDoList1._items);
            break;
        case MenuOptions.RemoveItem:
        {
            Console.WriteLine("Enter the title of the item you want to remove");
            var title = Console.ReadLine();
            toDoList1.RemoveItem(title);
            ToDoStorage.Save(toDoList1._items);
            break;
        }
        case MenuOptions.CompleteItem:
        {
            Console.WriteLine("Enter the title of the item you want to complete");
            var title = Console.ReadLine();
            toDoList1.CompleteItem(title);
            ToDoStorage.Save(toDoList1._items);
            break;
            
        }
        case MenuOptions.DisplayAll:
        {
            toDoList1.PrintAllItems();
            break;
        }
        case MenuOptions.Exit:
            return;
    }
}

string PrintAvailableOptions()
{
    Console.WriteLine("\nWhat do you want to do?");
    foreach (var option in Enum.GetValues<MenuOptions>())
        Console.WriteLine($"{(int)option}. {option}");
    return Console.ReadLine();
}

List<string> ReadUserNames()
{
    return Console.ReadLine().Split(',').ToList();
}

void AddItemInList()
{
    Console.WriteLine("Enter title:");
    var title = Console.ReadLine();
    Console.WriteLine("Enter description (Optional, you can just press enter to continue):");
    var description = Console.ReadLine();
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
    var type = int.Parse(Console.ReadLine());
    var itemType = (ToDoItemTypes)type;
    switch (itemType)
    {
        case ToDoItemTypes.Call:
        {
            Console.WriteLine("Enter invited for the Call person(s) (Separate multiple persons by comma");
            var invitedPersons = ReadUserNames();
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
            Console.WriteLine("Enter invited for the Meeting person(s)(Separate multiple persons by comma)");
            var invitedPersons = ReadUserNames();
            Console.WriteLine("Enter meeting place");
            var place = Console.ReadLine();
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
            Console.WriteLine("Enter the name of person Day of Birth");
            var nameOfPersonWithDoB = Console.ReadLine();
            toDoList1.AddItem(new DateOfBirth(
                targetDayTime: targetDayTime,
                itemType: ToDoItemTypes.DayOfBirth,
                description: description,
                title: title,
                isCompleted: false,
                nameOfPersonWithDoB: nameOfPersonWithDoB
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
