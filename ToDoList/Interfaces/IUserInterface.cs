using ToDoList.Enums;
using ToDoList.Models;

namespace ToDoList.Interfaces;

public interface IUserInterface
{
    void PrintSeparator();
    string AskUser(string prompt);
    List<string> ReadUserNames(string prompt);
    DateTime AskDate(string prompt);
    int PrintAvailableOptions();
    DisplayOptions PrintDisplayOptions();
    void PrintItems(List<ToDoItem> items);
    void PrintItemsByDateRange(DateTime startDate, DateTime endDate, List<ToDoItem> items); 
    void PrintItemsBySpecificDate(DateTime date, List<ToDoItem> items);
    void PrintMessage(string message);
}