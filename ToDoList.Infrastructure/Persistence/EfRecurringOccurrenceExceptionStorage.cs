using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Interfaces;
using ToDoList.Domain.Entities;

namespace ToDoList.Infrastructure.Persistence;

public class EfRecurringOccurrenceExceptionStorage : IRecurringOccurrenceExceptionStorage
{
    private readonly AppDbContext _context;

    public EfRecurringOccurrenceExceptionStorage(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<RecurringOccurrenceException>> LoadAsync(
        string userId,
        DateTime rangeStart,
        DateTime rangeEnd)
    {
        return await _context.RecurringOccurrenceExceptions.Where(exception =>
                exception.UserId == userId &&
                exception.OccurrenceDateTime >= rangeStart &&
                exception.OccurrenceDateTime <= rangeEnd)
            .ToListAsync();
    }

    public async Task CompleteAsync(string userId, Guid seriesId, DateTime occurrenceDateTime)
    {
        var exception = await _context.RecurringOccurrenceExceptions
            .FirstOrDefaultAsync(exception =>
                exception.UserId == userId &&
                exception.SeriesId == seriesId &&
                exception.OccurrenceDateTime == occurrenceDateTime);

        if (exception != null)
        {
            exception.IsCompleted = true;
            exception.IsCancelled = false;
        }
        else
        {
            _context.RecurringOccurrenceExceptions.Add(new RecurringOccurrenceException
            {
                UserId = userId,
                SeriesId = seriesId,
                OccurrenceDateTime = occurrenceDateTime,
                IsCompleted = true,
                IsCancelled = false
            });
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task CancelAsync(string userId, Guid seriesId, DateTime occurrenceDateTime)
    {
        var exception = await _context.RecurringOccurrenceExceptions
            .FirstOrDefaultAsync(exception =>
                exception.UserId == userId &&
                exception.SeriesId == seriesId &&
                exception.OccurrenceDateTime == occurrenceDateTime);

        if (exception != null)
        {
            exception.IsCompleted = false;
            exception.IsCancelled = true;
        }
        else
        {
            _context.RecurringOccurrenceExceptions.Add(new RecurringOccurrenceException
            {
                UserId = userId,
                SeriesId = seriesId,
                OccurrenceDateTime = occurrenceDateTime,
                IsCompleted = false,
                IsCancelled = true
            });
        }
        
        await _context.SaveChangesAsync();
    }
}