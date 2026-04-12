using ToDoList.Models;

namespace ToDoList.Interfaces;

public interface IEventStorage
{
    void Save(ToDoEvent toDoEvent);
    List<ToDoEvent> Load();
}