using ToDoList.Enums;
using ToDoList.Interfaces;

namespace ToDoList.Models;

public class ToDoManager : IToDoManager
{
    private readonly IToDoStorage _storage;
    private readonly IEventStorage _eventStorage;
    
    public ToDoManager(IToDoStorage storage, IEventStorage eventStorage)
    {
        _storage = storage;
        _eventStorage = eventStorage;
    }
    
    public void AddItem(ToDoItem item)
    {
        _storage.Add(item);
        _eventStorage.Save(new ToDoEvent(ToDoEventTypes.ItemAdded, item.Title, DateTime.Now));
    }

    public bool RemoveItem(Guid id)
    {
        var item = _storage.GetById(id);
        if (item == null)
            return false;
        _storage.Remove(id);
        _eventStorage.Save(new ToDoEvent(ToDoEventTypes.ItemRemoved, item.Title, DateTime.Now));
        return true;
    }

    public bool CompleteItem(Guid id)
    {
        var item = _storage.GetById(id);
        if (item == null)
            return false;
        _storage.Complete(id);
        _eventStorage.Save(new ToDoEvent(ToDoEventTypes.ItemCompleted, item.Title, DateTime.Now));
        return true;
    }
    
    public List<ToDoItem> GetAllItems() => _storage.GetAll();

    public List<ToDoItem> GetActiveItems() => _storage.GetActive();

    public List<ToDoItem> GetItemsByDateTimeRange(DateTime startDate, DateTime endDate) => 
        _storage.GetByDateRange(startDate, endDate);

    public List<ToDoItem> GetItemsBySpecificDate(DateTime date) => 
        _storage.GetByDate(date);

    public List<ToDoEvent> GetAllEvents() => _eventStorage.Load();
}