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
        _eventStorage.Save(new ToDoEvent(ToDoEventTypes.ItemAdded,item.UserId, item.Title, DateTime.Now));
    }

    public bool RemoveItem(Guid id, string userId)
    {
        var item = _storage.GetById(id, userId);
        if (item == null)
            return false;

        var removed = _storage.Remove(id, userId);
        if (!removed)
            return false;

        _eventStorage.Save(new ToDoEvent(ToDoEventTypes.ItemRemoved, item.UserId, item.Title, DateTime.Now));
        return true;
    }

    public bool CompleteItem(Guid id, string userId)
    {
        var item = _storage.GetById(id, userId);
        if (item == null)
            return false;

        var completed = _storage.Complete(id, userId);
        if (!completed)
            return false;

        _eventStorage.Save(new ToDoEvent(ToDoEventTypes.ItemCompleted, item.UserId, item.Title, DateTime.Now));
        return true;
    }
    
    public List<ToDoItem> GetAllItems(string userId) => _storage.GetAll(userId);
    
    public ToDoItem? GetItemById(Guid id, string userId) => _storage.GetById(id, userId);
    
    public List<ToDoItem> GetActiveItems(string userId) => _storage.GetActive(userId);
    public List<ToDoItem> GetItemByTitle(string title, string userId) => _storage.GetByTitle(title, userId);

    public List<ToDoItem> GetItemsByDateTimeRange(DateTime startDate, DateTime endDate, string userId) => 
        _storage.GetByDateRange(startDate, endDate, userId);

    public List<ToDoItem> GetItemsBySpecificDate(DateTime date, string userId) => 
        _storage.GetByDate(date, userId);

    public List<ToDoEvent> GetAllEvents(string userId) =>
        _eventStorage.Load().Where(e => e.UserId == userId).ToList();

    public bool UpdateItem(Guid id, string userId, string title, string description, DateTime targetDayTime)
    {
        var item = _storage.GetById(id, userId);
        if (item == null)
            return false;

        var updated = _storage.Update(id, userId, title, description, targetDayTime);
        if (!updated)
            return false;

        _eventStorage.Save(new ToDoEvent(ToDoEventTypes.ItemEdited, userId, title, DateTime.Now));
        return true;
    }
}