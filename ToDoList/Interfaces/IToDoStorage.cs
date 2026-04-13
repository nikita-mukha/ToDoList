using ToDoList.Models;

namespace ToDoList.Interfaces;

public interface IToDoStorage
{
    void Add(ToDoItem item);
    ToDoItem? GetById(Guid id);
    bool Remove(Guid id);
    bool Complete(Guid id);
    List<ToDoItem> GetAll();
    List<ToDoItem> GetActive();
    List<ToDoItem> GetByDateRange(DateTime start, DateTime end);
    List<ToDoItem> GetByDate(DateTime date);
}