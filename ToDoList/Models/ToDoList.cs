namespace ToDoList.Models;

public class ToDoList
{
    private List<ToDoItem> _items;

    public ToDoList()
    {
        _items = new List<ToDoItem>();
    }

    public void SaveItems() => ToDoStorage.Save(_items);

    public void LoadItems() => _items = ToDoStorage.Load();

    public void AddItem(ToDoItem item) => _items.Add(item);

    public void RemoveItem(string title)
    {
        var item = _items.FirstOrDefault(i =>
            i.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (item != null)
        {
            _items.Remove(item);
            Console.WriteLine($"Item '{title}' was removed.");
        }
        else
            Console.WriteLine($"Item '{title}' was not found.");
    }

    public void CompleteItem(string title)
    {
        var item = _items.FirstOrDefault(i =>
            i.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (item != null)
        {
            item.IsCompleted = true;
            Console.WriteLine($"Item '{title}' was completed.");
        }
        else
            Console.WriteLine($"Item '{title}' was not found.");
    }

    public List<ToDoItem> GetAllItems() => _items.ToList();

    public List<ToDoItem> GetActiveItems() =>
        _items.Where(i => !i.IsCompleted).ToList();

    public List<ToDoItem> GetCompletedItems() =>
        _items.Where(i => i.IsCompleted).ToList();

    public List<ToDoItem> GetItemsByDateTimeRange(DateTime startDate, DateTime endDate) =>
        _items.Where(i => i.TargetDayTime.Date >= startDate.Date
                          && i.TargetDayTime.Date <= endDate.Date).ToList();

    public List<ToDoItem> GetItemsBySpecificDate(DateTime date) =>
        _items.Where(i => i.TargetDayTime.Date == date.Date).ToList();
}