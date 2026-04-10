using System.Text.Json;
using ToDoList.Interfaces;

namespace ToDoList.Models;

public class ToDoStorage : IToDoStorage
{
    private readonly string _filePath;
    public ToDoStorage(string filePath = "todos.json")
    {
        _filePath = filePath;
    }

    public void Save(List<ToDoItem> items)
    {
        try
        { 
            var options = new JsonSerializerOptions { WriteIndented = true }; 
            var json = JsonSerializer.Serialize(items, options); 
            File.WriteAllText(_filePath, json);
        
        }
        catch (IOException)
        {
            Console.WriteLine("Warning: could not save your list. Changes may be lost.");
        }
    }

    public  List<ToDoItem> Load()
    {
        if (!File.Exists(_filePath))
            return new List<ToDoItem>();
        try
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<ToDoItem>>(json) 
                   ?? new List<ToDoItem>();
        }
        catch (JsonException)
        {
            Console.WriteLine("Warning: todos.json is corrupted. Starting with empty list.");
            return new List<ToDoItem>();
        }
        catch (IOException)
        {
            Console.WriteLine("Warning: could not read todos.json. Starting with empty list.");
            return new List<ToDoItem>();
        }
    }
}