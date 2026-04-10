using ToDoList.Models;

namespace ToDoList.Interfaces;

public interface IToDoStorage
{
    void Save(List<ToDoItem> items);
    List<ToDoItem> Load();
}