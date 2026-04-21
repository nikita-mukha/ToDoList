using ToDoList.Models;

namespace ToDoList.Interfaces;

public interface IToDoStorage
{
    void Add(ToDoItem item);
    ToDoItem? GetById(Guid id, string userId);
    bool Remove(Guid id, string userId);
    bool Complete(Guid id, string userId);
    List<ToDoItem> GetAll(string userId);
    List<ToDoItem> GetActive(string userId);
    List<ToDoItem> GetByTitle(string title, string userId);
    List<ToDoItem> GetByDateRange(DateTime start, DateTime end, string userId);
    List<ToDoItem> GetByDate(DateTime date, string userId);
    bool Update(Guid id, string userId, string title, string description, DateTime targetDayTime);
}