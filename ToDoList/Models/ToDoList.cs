using ToDoList.Enums;
using ToDoList.Interfaces;

namespace ToDoList.Models;

public class ToDoList
{
    private List<ToDoItem> _items;
    private readonly IToDoStorage _storage;
    private readonly IEventStorage _eventStorage;

    public ToDoList(IToDoStorage storage, IEventStorage eventStorage)
    {
        _storage = storage;
        _eventStorage = eventStorage;
        _items = new List<ToDoItem>();
    }

    public void SaveItems() => _storage.Save(_items);

    public void LoadItems() => _items = _storage.Load();

    public void AddItem(ToDoItem item)
    {
        _items.Add(item);
        _eventStorage.Save(new ToDoEvent(ToDoEventTypes.ItemAdded, item.Title, DateTime.Now));
    }

    public bool RemoveItem(string title)
    {
        var item = _items.FirstOrDefault(i =>
            i.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (item == null)
            return false; 
        _items.Remove(item);
        _eventStorage.Save(new ToDoEvent(ToDoEventTypes.ItemRemoved, title, DateTime.Now));
        return true;
    }

    public bool CompleteItem(string title)
    {
        var item = _items.FirstOrDefault(i =>
            i.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (item == null)
            return false;
        item.IsCompleted = true;
        _eventStorage.Save(new ToDoEvent(ToDoEventTypes.ItemCompleted, title, DateTime.Now));
        return true;
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

    public List<ToDoEvent> GetAllEvents() => _eventStorage.Load();
}