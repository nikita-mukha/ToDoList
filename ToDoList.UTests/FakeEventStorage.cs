using ToDoList.Interfaces;
using ToDoList.Models;

namespace ToDoList.UTests.TestResults;

public class FakeEventStorage : IEventStorage
{
    public List<ToDoEvent> Events = new();
    
    public void Save(ToDoEvent toDoEvent) => Events.Add(toDoEvent);
    public List<ToDoEvent> Load() => Events;
}