using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Interfaces;
using ToDoList.Domain.Entities;

namespace ToDoList.Infrastructure.Persistence;

public class EfRecurringSeriesStorage : IRecurringSeriesStorage
{
    private readonly AppDbContext _context;

    public EfRecurringSeriesStorage(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task SaveAsync(RecurringSeries series)
    {
        _context.RecurringSeries.Add(series);
        await _context.SaveChangesAsync();
    }

    public async Task<List<RecurringSeries>> LoadActiveAsync(string userId, DateTime rangeStart, DateTime rangeEnd)
    {
        return await _context.RecurringSeries.Where(series =>
            series.UserId == userId &&
            series.IsActive &&
            series.StartDateTime <= rangeEnd &&
            (!series.EndDateTime.HasValue || series.EndDateTime.Value >= rangeStart))
            .ToListAsync();
    }

    public async Task<bool> StopAsync(Guid seriesId, string userId)
    {
        var recurringSeries = await _context.RecurringSeries
            .FirstOrDefaultAsync(series =>
                series.Id == seriesId &&
                series.UserId == userId);

        if (recurringSeries == null)
            return false;

        recurringSeries.IsActive = false;

        await _context.SaveChangesAsync();
        return true;
    }
}