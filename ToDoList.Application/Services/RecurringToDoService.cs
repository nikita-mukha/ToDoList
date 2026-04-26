using ToDoList.Application.Interfaces;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Enums;

namespace ToDoList.Application.Services;

public class RecurringToDoService : IRecurringToDoService
{
    private readonly IRecurringSeriesStorage _recurringSeriesStorage;
    private readonly IToDoManager _toDoManager;

    public RecurringToDoService(IRecurringSeriesStorage recurringSeriesStorage, IToDoManager toDoManager)
    {
        _recurringSeriesStorage = recurringSeriesStorage;
        _toDoManager = toDoManager;
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
}