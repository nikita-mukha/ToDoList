using ToDoList.Enums;

namespace ToDoList.Models;

public class ToDoList
{
    private List<ToDoItem> _items;

    public void SaveItems()
    {
        ToDoStorage.Save(_items);
    }

    public void LoadItems()
    {
        _items = ToDoStorage.Load();
    }
    public ToDoList()
    {
        _items = new List<ToDoItem>();
    }

    public void AddItem(ToDoItem item)
    {
        _items.Add(item);
    }

    public void RemoveItem(string title)
    {
        var item = _items.FirstOrDefault(i => 
            i.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (item != null)
            _items.Remove(item);
        Console.WriteLine($"{title} has been removed");
    }

    public void CompleteItem(string title)
    {
        var item = _items.FirstOrDefault(i => 
            i.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (item != null)
        {
            item.IsCompleted = true;
            Console.WriteLine($"{title} {item.ItemType} has been completed");
        }
    }

    public void PrintAllItems()
    {
        foreach (var item in _items)
            Console.WriteLine(item);
    }

    public void PrintOnlyNotCompletedItems()
    {
        foreach (var item in _items.Where(i => !i.IsCompleted))
            Console.WriteLine(item);
    }

    public void PrintOnlyCompletedItems()
    {
        foreach (var item in _items)
        {
            if (item.IsCompleted)
                Console.WriteLine($"{item.Title} - {item.ItemType} - {item.Description}");

        }
    }

    public List<ToDoItem> GetItemsByDateTimeRange(DateTime startDate, DateTime endDate)
    {
        return _items.Where(i => i.TargetDayTime.Date
                                 >= startDate.Date
                                 && i.TargetDayTime.Date
                                 <= endDate.Date).ToList();
    }

    public List<ToDoItem> GetItemsBySpecificDate(DateTime startDate)
    {
        return _items.Where(i => i.TargetDayTime.Date == startDate.Date).ToList();
    }

    public void PrintItemsByDateRange(DateTime startDate, DateTime endDate)
    {
        Console.WriteLine($"Results from {startDate:dd/MM/yyyy} - to {endDate:dd/MM/yyyy}:");
        foreach (var item in GetItemsByDateTimeRange(startDate, endDate))
            Console.WriteLine($"{item.Title} - {item.ItemType} - {item.Description}");
    }

    public void PrintItemsBySpecificDate(DateTime startDate)
    {
        Console.WriteLine($"Results for {startDate:dd/MM/yyyy}:");
        foreach (var item in GetItemsBySpecificDate(startDate))
            Console.WriteLine($"{item.Title} - {item.ItemType} - {item.Description}");
    }
    
}