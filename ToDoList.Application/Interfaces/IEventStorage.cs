using ToDoList.Models;

namespace ToDoList.Interfaces;

public interface IEventStorage
{
    Task SaveAsync(ToDoEvent toDoEvent);
    Task<List<ToDoEvent>> LoadAsync();
}