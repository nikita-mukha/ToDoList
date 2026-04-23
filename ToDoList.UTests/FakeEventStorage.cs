using ToDoList.Interfaces;
using ToDoList.Models;

namespace ToDoList.UTests.TestResults;

public class FakeEventStorage : IEventStorage
{
    public List<ToDoEvent> Events = new();

    public Task SaveAsync(ToDoEvent toDoEvent)
    {
        Events.Add(toDoEvent);
        return Task.CompletedTask;
    }

    public Task<List<ToDoEvent>> LoadAsync() => Task.FromResult(Events);
}