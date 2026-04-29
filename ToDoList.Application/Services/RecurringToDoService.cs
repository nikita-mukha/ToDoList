using ToDoList.Application.Interfaces;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Enums;

namespace ToDoList.Application.Services;

public class RecurringToDoService : IRecurringToDoService
{
    private readonly IRecurringSeriesStorage _recurringSeriesStorage;
    private readonly IToDoManager _toDoManager;
    private readonly IRecurringOccurrenceExceptionStorage _recurringOccurrenceExceptionStorage;
    
    public RecurringToDoService(
        IRecurringSeriesStorage recurringSeriesStorage,
        IToDoManager toDoManager,
        IRecurringOccurrenceExceptionStorage recurringOccurrenceExceptionStorage)
    {
        _recurringSeriesStorage = recurringSeriesStorage;
        _toDoManager = toDoManager;
        _recurringOccurrenceExceptionStorage = recurringOccurrenceExceptionStorage;
    }
    
    public async Task CreateRecurringItemAsync(ToDoItem sourceItem,
        RecurrenceFrequency frequency,
        int interval,
        DateTime? endDateTime)
    { 
        ArgumentNullException.ThrowIfNull(sourceItem);
        
        if (interval < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(interval), "Interval must be at least 1");
        }
        
        if (endDateTime.HasValue && endDateTime.Value < sourceItem.TargetDayTime)
        {
            throw new ArgumentException("End date must be after the start date", nameof(endDateTime));
        }
        await _toDoManager.AddItemAsync(sourceItem);

        var series = new RecurringSeries
        {
            UserId = sourceItem.UserId,
            SourceItemId = sourceItem.Id,
            StartDateTime = sourceItem.TargetDayTime,
            Frequency = frequency,
            Interval = interval,
            EndDateTime = endDateTime,
            IsActive = true
        };
        await _recurringSeriesStorage.SaveAsync(series);
    }

    public Task<bool> StopRecurringSeriesAsync(Guid seriesId, string userId) =>
        _recurringSeriesStorage.StopAsync(seriesId, userId);

    public Task CompleteRecurringOccurrenceAsync(Guid seriesId, DateTime occurrenceDateTime, string userId) => 
        _recurringOccurrenceExceptionStorage.CompleteAsync(userId, seriesId, occurrenceDateTime);

    public Task CancelRecurringOccurrenceAsync(Guid seriesId, DateTime occurrenceDateTime, string userId) =>
        _recurringOccurrenceExceptionStorage.CancelAsync(userId, seriesId, occurrenceDateTime);
}