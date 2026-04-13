using ToDoList.Models;

namespace ToDoList.Interfaces;

public interface IToDoManager
{
    void AddItem(ToDoItem item);
    bool RemoveItem(Guid id);
    bool CompleteItem(Guid id);
    List<ToDoItem> GetAllItems();
    List<ToDoItem> GetActiveItems();
    List<ToDoItem> GetItemsByDateTimeRange(DateTime startDate, DateTime endDate);
    List<ToDoItem> GetItemsBySpecificDate(DateTime date);
    List<ToDoEvent> GetAllEvents();
}