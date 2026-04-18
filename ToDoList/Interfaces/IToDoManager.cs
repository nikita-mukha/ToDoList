using ToDoList.Models;

namespace ToDoList.Interfaces;

public interface IToDoManager
{
    void AddItem(ToDoItem item);
    bool RemoveItem(Guid id, string userId);
    bool CompleteItem(Guid id, string userId);
    ToDoItem? GetItemById(Guid id, string userId);
    List<ToDoItem> GetAllItems(string userId);
    List<ToDoItem> GetActiveItems(string userId);
    List<ToDoItem> GetItemsByDateTimeRange(DateTime startDate, DateTime endDate, string userId);
    List<ToDoItem> GetItemsBySpecificDate(DateTime date, string userId);
    List<ToDoEvent> GetAllEvents(string userId);
    bool UpdateItem(Guid id, string userId, string title, string description, DateTime targetDayTime);
}