using ToDoList.Interfaces;
using ToDoList.Models;

namespace ToDoList.UTests;

public class FakeStorage : IToDoStorage
{
    public List<ToDoItem> Items = new();
    public void Save(List<ToDoItem> items) => 
        Items = items;

    public List<ToDoItem> Load() =>
        Items;
}