using ToDoList.Domain.Entities;

namespace ToDoList.Application.Interfaces;

public interface IToDoStorage
{
    Task AddAsync(ToDoItem item);
    Task<ToDoItem?> GetByIdAsync(Guid id, string userId);
    Task<bool> RemoveAsync(Guid id, string userId);
    Task<bool> CompleteAsync(Guid id, string userId);
    Task<List<ToDoItem>> GetAllAsync(string userId);
    Task<List<ToDoItem>> GetActiveAsync(string userId);
    Task<List<ToDoItem>> GetByTitleAsync(string title, string userId);
    Task<List<ToDoItem>> GetByDateRangeAsync(DateTime start, DateTime end, string userId);
    Task<List<ToDoItem>> GetByDateAsync(DateTime date, string userId);
    Task<bool> HasTimeConflictAsync(DateTime date, string userId, Guid? currentItemId);
    Task<bool> UpdateAsync(Guid id, string userId, string title, string description, DateTime targetDayTime);
}