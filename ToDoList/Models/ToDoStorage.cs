using System.Text.Json;
using ToDoList.Models;

namespace ToDoList.Models;

public static class ToDoStorage
{
    private static readonly string FilePath = "todos.json";

    public static void Save(List<ToDoItem> items)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(items, options);
        File.WriteAllText(FilePath, json);
    }

    public static List<ToDoItem> Load()
    {
        if (!File.Exists(FilePath))
            return new List<ToDoItem>();
        
        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<ToDoItem>>(json) 
               ?? new List<ToDoItem>();
    }
}