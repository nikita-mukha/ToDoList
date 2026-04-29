using ToDoList.Domain.Entities;
using ToDoList.Domain.Enums;

namespace ToDoList.Application.Interfaces;

public interface IRecurringToDoService
{
    Task CreateRecurringItemAsync(
        ToDoItem sourceItem,
        RecurrenceFrequency frequency,
        int interval,
        DateTime? endDateTime);
    Task<bool> StopRecurringSeriesAsync(Guid seriesId, string userId);
    Task CompleteRecurringOccurrenceAsync(Guid seriesId, DateTime occurrenceDateTime, string userId);
    Task CancelRecurringOccurrenceAsync(Guid seriesId, DateTime occurrenceDateTime, string userId);
}