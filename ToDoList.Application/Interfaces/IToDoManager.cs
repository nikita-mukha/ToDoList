using ToDoList.Domain.Entities;

namespace ToDoList.Application.Interfaces;

public interface IToDoManager
{
    Task AddItemAsync(ToDoItem item);
    Task<bool> RemoveItemAsync(Guid id, string userId);
    Task<bool> CompleteItemAsync(Guid id, string userId);
    Task<ToDoItem?> GetItemByIdAsync(Guid id, string userId);
    Task<List<ToDoItem>> GetAllItemsAsync(string userId);
    Task<List<ToDoItem>> GetActiveItemsAsync(string userId);
    Task<List<ToDoItem>> GetItemByTitleAsync(string title, string userId);
    Task<List<ToDoItem>> GetItemsByDateTimeRangeAsync(DateTime startDate, DateTime endDate, string userId);
    Task<List<ToDoItem>> GetItemsBySpecificDateAsync(DateTime date, string userId);
    Task<List<ToDoEvent>> GetAllEventsAsync(string userId);
    Task<bool> HasTimeConflictItemAsync(DateTime date, string userId, Guid? currentItemId);
    Task<bool> UpdateItemAsync(Guid id, string userId, string title, string description, DateTime targetDayTime);
    Task<List<ToDoItem>> GetItemsByIdsAsync(IEnumerable<Guid> ids, string userId);
}