using ToDoList.Domain.Entities;

namespace ToDoList.Application.Interfaces;

public interface IRecurringOccurrenceExceptionStorage
{
    Task<List<RecurringOccurrenceException>> LoadAsync(string userId, DateTime rangeStart, DateTime rangeEnd);
    Task CompleteAsync(string userId, Guid seriesId, DateTime occurrenceDateTime);
    Task CancelAsync(string userId, Guid seriesId, DateTime occurrenceDateTime);
}