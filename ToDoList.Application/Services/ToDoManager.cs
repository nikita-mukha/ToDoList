using ToDoList.Application.Interfaces;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Enums;

namespace ToDoList.Application.Services;

public class ToDoManager : IToDoManager
{
    private readonly IToDoStorage _storage;
    private readonly IEventStorage _eventStorage;

    public ToDoManager(IToDoStorage storage, IEventStorage eventStorage)
    {
        _storage = storage;
        _eventStorage = eventStorage;
    }

    public async Task AddItemAsync(ToDoItem item)
    {
        await _storage.AddAsync(item);
        await _eventStorage.SaveAsync(new ToDoEvent(ToDoEventTypes.ItemAdded, item.UserId, item.Title, DateTime.Now));
    }

    public async Task<bool> RemoveItemAsync(Guid id, string userId)
    {
        var item = await _storage.GetByIdAsync(id, userId);
        if (item == null)
            return false;

        var removed = await _storage.RemoveAsync(id, userId);
        if (!removed)
            return false;

        await _eventStorage.SaveAsync(new ToDoEvent(ToDoEventTypes.ItemRemoved, item.UserId, item.Title, DateTime.Now));
        return true;
    }

    public async Task<bool> CompleteItemAsync(Guid id, string userId)
    {
        var item = await _storage.GetByIdAsync(id, userId);
        if (item == null)
            return false;

        var completed = await _storage.CompleteAsync(id, userId);
        if (!completed)
            return false;

        await _eventStorage.SaveAsync(new ToDoEvent(ToDoEventTypes.ItemCompleted, item.UserId, item.Title, DateTime.Now));
        return true;
    }

    public Task<List<ToDoItem>> GetAllItemsAsync(string userId) => _storage.GetAllAsync(userId);

    public Task<ToDoItem?> GetItemByIdAsync(Guid id, string userId) => _storage.GetByIdAsync(id, userId);

    public Task<List<ToDoItem>> GetActiveItemsAsync(string userId) => _storage.GetActiveAsync(userId);

    public Task<List<ToDoItem>> GetItemByTitleAsync(string title, string userId) => _storage.GetByTitleAsync(title, userId);

    public Task<List<ToDoItem>> GetItemsByDateTimeRangeAsync(DateTime startDate, DateTime endDate, string userId) =>
        _storage.GetByDateRangeAsync(startDate, endDate, userId);

    public Task<List<ToDoItem>> GetItemsBySpecificDateAsync(DateTime date, string userId) =>
        _storage.GetByDateAsync(date, userId);

    public async Task<List<ToDoEvent>> GetAllEventsAsync(string userId)
    {
        var events = await _eventStorage.LoadAsync();
        return events.Where(e => e.UserId == userId).ToList();
    }

    public async Task<bool> UpdateItemAsync(Guid id, string userId, string title, string description, DateTime targetDayTime)
    {
        var item = await _storage.GetByIdAsync(id, userId);
        if (item == null)
            return false;

        var updated = await _storage.UpdateAsync(id, userId, title, description, targetDayTime);
        if (!updated)
            return false;

        await _eventStorage.SaveAsync(new ToDoEvent(ToDoEventTypes.ItemEdited, userId, title, DateTime.Now));
        return true;
    }

    public Task<bool> HasTimeConflictItemAsync(DateTime date, string userId, Guid? currentItemId) =>
        _storage.HasTimeConflictAsync(date, userId, currentItemId);
}
