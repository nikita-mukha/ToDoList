using ToDoList.Enums;
using ToDoList.Interfaces;
using ToDoList.Models;

IUserInterface ui = new ConsoleUi();
IToDoStorage storage = new ToDoStorage();
IEventStorage eventStorage = new JsonEventStorage();
ItemFactory itemFactory = new ItemFactory(ui);
    
var toDoList = new ToDoList.Models.ToDoList(storage, eventStorage);
toDoList.LoadItems();

while (true)
{
    var choice = ui.PrintAvailableOptions();
    var choiceType = (MenuOptions)choice;
    switch (choiceType)
    {
        case MenuOptions.AddItem:
        {
            var item = itemFactory.Create();
            toDoList.AddItem(item);
            ui.PrintMessage($"Item '{item.Title}' has been added");
            toDoList.SaveItems();
            break;
        }
        case MenuOptions.RemoveItem:
        {
            var title = ui.AskUser("Enter the title of the item you want to remove:");
            var removed = toDoList.RemoveItem(title);
            ui.PrintMessage(removed
            ? $"Item '{title}' was removed."
            : $"Item '{title}' was not found.");
            toDoList.SaveItems();
            break;
        }
        case MenuOptions.CompleteItem:
        {
            var title = ui.AskUser("Enter the title of the item you want to complete:");
            var completed = toDoList.CompleteItem(title);
            ui.PrintMessage(completed
            ? $"Item '{title}' was completed." 
            : $"Item '{title}' was not found.");
            toDoList.SaveItems();
            break;
        }
        case MenuOptions.Display:
        {
            var displayChoice = ui.PrintDisplayOptions();
            switch (displayChoice)
            {
                case DisplayOptions.All:
                    ui.PrintItems(toDoList.GetAllItems());
                    break;
                case DisplayOptions.ActiveOnly:
                    ui.PrintItems(toDoList.GetActiveItems());
                    break;
                case DisplayOptions.ByDateRange:
                {
                    var startDate = ui.AskDate("Enter start date (format dd/MM/yyyy):");
                    var endDate = ui.AskDate("Enter end date (format dd/MM/yyyy):");
                    ui.PrintItemsByDateRange(startDate, endDate,
                        toDoList.GetItemsByDateTimeRange(startDate, endDate));
                    break;
                }
                case DisplayOptions.BySpecificDate:
                {
                    var date = ui.AskDate("Enter date (format dd/MM/yyyy):");
                    ui.PrintItemsBySpecificDate(date,
                        toDoList.GetItemsBySpecificDate(date));
                    break;
                }
            }
            break;
        }
        case MenuOptions.DisplayEventLogs:
            ui.PrintEvents(toDoList.GetAllEvents());
            break;
        case MenuOptions.Exit:
            return;
        default:
            ui.PrintMessage("Invalid choice, please try again:");
            break;
    }
}