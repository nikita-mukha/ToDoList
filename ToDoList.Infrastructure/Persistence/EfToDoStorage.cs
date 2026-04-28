using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Interfaces;
using ToDoList.Domain.Entities;

namespace ToDoList.Infrastructure.Persistence;

public class EfToDoStorage : IToDoStorage
{
    private readonly AppDbContext _context;

    public EfToDoStorage(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ToDoItem item)
    {
        _context.Items.Add(item);
        await _context.SaveChangesAsync();
    }

    public Task<ToDoItem?> GetByIdAsync(Guid id, string userId) =>
        _context.Items.FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

    public async Task<bool> RemoveAsync(Guid id, string userId)
    {
        var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);
        if (item == null)
            return false;
        _context.Items.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CompleteAsync(Guid id, string userId)
    {
        var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);
        if (item == null)
            return false;
        item.IsCompleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public Task<List<ToDoItem>> GetAllAsync(string userId) =>
        _context.Items.Where(i => i.UserId == userId).ToListAsync();

    public Task<List<ToDoItem>> GetActiveAsync(string userId) =>
        _context.Items.Where(i => i.UserId == userId && i.IsCompleted == false).ToListAsync();

    public Task<List<ToDoItem>> GetByDateRangeAsync(DateTime start, DateTime end, string userId) =>
        _context.Items.Where(i =>
            i.UserId == userId &&
            i.TargetDayTime.Date >= start.Date &&
            i.TargetDayTime.Date <= end.Date).ToListAsync();

    public Task<List<ToDoItem>> GetByDateAsync(DateTime date, string userId) =>
        _context.Items.Where(i => i.UserId == userId && i.TargetDayTime.Date == date.Date).ToListAsync();

    public async Task<bool> UpdateAsync(Guid id, string userId, string title, string description, DateTime targetDayTime)
    {
        var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);
        if (item == null)
            return false;
        item.Title = title;
        item.Description = description;
        item.TargetDayTime = targetDayTime;
        await _context.SaveChangesAsync();
        return true;
    }

    public Task<List<ToDoItem>> GetByTitleAsync(string title, string userId) =>
        _context.Items
            .Where(i => i.UserId == userId &&
                        i.Title.ToLower().Contains(title.ToLower())).ToListAsync();

    public Task<bool> HasTimeConflictAsync(DateTime date, string userId, Guid? currentItemId)
    {
        var minuteStart = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
        var minuteEnd = minuteStart.AddMinutes(1);

        return _context.Items
            .AnyAsync(i => i.UserId == userId &&
                           i.TargetDayTime >= minuteStart &&
                           i.TargetDayTime < minuteEnd &&
                           i.Id != currentItemId);
    }

    public async Task<List<ToDoItem>> GetByIdsAsync(IEnumerable<Guid> ids, string userId)
    {
        var idList = ids.ToList();

        if (!idList.Any())
            return new List<ToDoItem>();

        return await _context.Items
            .Where(i => i.UserId == userId && idList.Contains(i.Id))
            .ToListAsync();
    }
}