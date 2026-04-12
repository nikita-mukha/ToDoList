using System.Text.Json;
using ToDoList.Interfaces;

namespace ToDoList.Models;

public class JsonEventStorage : IEventStorage
{
    private readonly string _filePath;
    public JsonEventStorage(string filePath = "events.json")
    {
        _filePath = filePath;
    }  
    public void Save(ToDoEvent item)
    {
        try
        {
            var events = Load();
            events.Add(item);
            var options = new JsonSerializerOptions{ WriteIndented = true }; 
            var json = JsonSerializer.Serialize(events, options); 
            File.WriteAllText(_filePath, json);
        }
        catch (IOException)
        {
            Console.WriteLine("Warning: could not save your list. Changes may be lost.");
        }
    }

    public  List<ToDoEvent> Load()
    {
        if (!File.Exists(_filePath))
            return new List<ToDoEvent>();
        try
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<ToDoEvent>>(json) 
                   ?? new List<ToDoEvent>();
        }
        catch (JsonException)
        {
            Console.WriteLine("Warning: events.json is corrupted. Starting with empty list.");
            return new List<ToDoEvent>();
        }
        catch (IOException)
        {
            Console.WriteLine("Warning: could not read events.json. Starting with empty list.");
            return new List<ToDoEvent>();
        }
    }
}