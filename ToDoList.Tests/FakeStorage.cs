using ToDoList.Application.Interfaces;
using ToDoList.Domain.Entities;

namespace ToDoList.Tests;

public class FakeStorage : IToDoStorage
{
    private readonly List<ToDoItem> _items = new();

    public Task AddAsync(ToDoItem item)
    {
        _items.Add(item);
        return Task.CompletedTask;
    }

    public Task<ToDoItem?> GetByIdAsync(Guid id, string userId) =>
        Task.FromResult(_items.FirstOrDefault(i => i.Id == id && i.UserId == userId));

    public Task<bool> RemoveAsync(Guid id, string userId)
    {
        var item = _items.FirstOrDefault(i => i.Id == id && i.UserId == userId);
        if (item == null)
            return Task.FromResult(false);
        _items.Remove(item);
        return Task.FromResult(true);
    }

    public Task<bool> CompleteAsync(Guid id, string userId)
    {
        var item = _items.FirstOrDefault(i => i.Id == id && i.UserId == userId);
        if (item == null)
            return Task.FromResult(false);
        item.IsCompleted = true;
        return Task.FromResult(true);
    }

    public Task<List<ToDoItem>> GetAllAsync(string userId) =>
        Task.FromResult(_items.Where(i => i.UserId == userId).ToList());

    public Task<List<ToDoItem>> GetActiveAsync(string userId) =>
        Task.FromResult(_items.Where(i => i.UserId == userId && !i.IsCompleted).ToList());

    public Task<List<ToDoItem>> GetByTitleAsync(string title, string userId) =>
        throw new NotImplementedException();

    public Task<List<ToDoItem>> GetByDateRangeAsync(DateTime start, DateTime end, string userId) =>
        Task.FromResult(_items.Where(i => i.UserId == userId
                                         && i.TargetDayTime.Date >= start.Date
                                         && i.TargetDayTime.Date <= end.Date).ToList());

    public Task<List<ToDoItem>> GetByDateAsync(DateTime date, string userId) =>
        Task.FromResult(_items.Where(i => i.UserId == userId && i.TargetDayTime.Date == date.Date).ToList());

    public Task<bool> HasTimeConflictAsync(DateTime date, string userId, Guid? currentItemId) =>
        throw new NotImplementedException();

    public Task<bool> UpdateAsync(Guid id, string userId, string title, string description, DateTime targetDayTime) =>
        throw new NotImplementedException();

    public Task<List<ToDoItem>> GetByIdsAsync(IEnumerable<Guid> ids, string userId)
    {
        throw new NotImplementedException();
    }
}
