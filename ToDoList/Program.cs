using ToDoList.Enums;
using ToDoList.Models;

var toDoList = new ToDoList.Models.ToDoList();
toDoList.LoadItems();

while (true)
{
    var choice = ConsoleUI.PrintAvailableOptions();
    var choiceType = (MenuOptions)choice;
    switch (choiceType)
    {
        case MenuOptions.AddItem:
        {
            var item = ItemFactory.Create();
            toDoList.AddItem(item);
            Console.WriteLine($"Item '{item.Title}' has been added");
            toDoList.SaveItems();
            break;
        }
        case MenuOptions.RemoveItem:
        {
            var title = ConsoleUI.AskUser("Enter the title of the item you want to remove:");
            toDoList.RemoveItem(title);
            toDoList.SaveItems();
            break;
        }
        case MenuOptions.CompleteItem:
        {
            var title = ConsoleUI.AskUser("Enter the title of the item you want to complete:");
            toDoList.CompleteItem(title);
            toDoList.SaveItems();
            break;
        }
        case MenuOptions.Display:
        {
            var displayChoice = ConsoleUI.PrintDisplayOptions();
            switch (displayChoice)
            {
                case DisplayOptions.All:
                    ConsoleUI.PrintItems(toDoList.GetAllItems());
                    break;
                case DisplayOptions.ActiveOnly:
                    ConsoleUI.PrintItems(toDoList.GetActiveItems());
                    break;
                case DisplayOptions.ByDateRange:
                {
                    var startDate = ConsoleUI.AskDate("Enter start date (format dd/MM/yyyy):");
                    var endDate = ConsoleUI.AskDate("Enter end date (format dd/MM/yyyy):");
                    ConsoleUI.PrintItemsByDateRange(startDate, endDate,
                        toDoList.GetItemsByDateTimeRange(startDate, endDate));
                    break;
                }
                case DisplayOptions.BySpecificDate:
                {
                    var date = ConsoleUI.AskDate("Enter date (format dd/MM/yyyy):");
                    ConsoleUI.PrintItemsBySpecificDate(date,
                        toDoList.GetItemsBySpecificDate(date));
                    break;
                }
                default:
                    Console.WriteLine("Invalid choice, please try again:");
                    break;
            }
            break;
        }
        case MenuOptions.Exit:
            return;
        default:
            Console.WriteLine("Invalid choice, please try again:");
            break;
    }
}