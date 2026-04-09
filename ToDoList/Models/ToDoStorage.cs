using System.Text.Json;

namespace ToDoList.Models;

public static class ToDoStorage
{
    private static readonly string FilePath = "todos.json";

    public static void Save(List<ToDoItem> items)
    {
        try
        { 
            var options = new JsonSerializerOptions { WriteIndented = true }; 
            var json = JsonSerializer.Serialize(items, options); 
            File.WriteAllText(FilePath, json);
        
        }
        catch (IOException)
        {
            Console.WriteLine("Warning: could not save your list. Changes may be lost.");
        }
    }

    public static List<ToDoItem> Load()
    {
        if (!File.Exists(FilePath))
            return new List<ToDoItem>();
        try
        {
            var json = File.ReadAllText(FilePath);
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