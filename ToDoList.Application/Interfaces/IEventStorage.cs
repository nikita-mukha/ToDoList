using ToDoList.Domain.Entities;

namespace ToDoList.Application.Interfaces;

public interface IEventStorage
{
    Task SaveAsync(ToDoEvent toDoEvent);
    Task<List<ToDoEvent>> LoadAsync(string userId);
}