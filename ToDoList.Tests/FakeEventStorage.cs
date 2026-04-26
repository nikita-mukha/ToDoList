using ToDoList.Application.Interfaces;
using ToDoList.Domain.Entities;

namespace ToDoList.Tests;

public class FakeEventStorage : IEventStorage
{
    public List<ToDoEvent> Events = new();

    public Task SaveAsync(ToDoEvent toDoEvent)
    {
        Events.Add(toDoEvent);
        return Task.CompletedTask;
    }

    public Task<List<ToDoEvent>> LoadAsync(string userId)
    {
        return Task.FromResult(
            Events.Where(e => e.UserId == userId).ToList()
        );
    }
}