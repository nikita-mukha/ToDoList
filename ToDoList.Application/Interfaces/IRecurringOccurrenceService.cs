using ToDoList.Application.Models;

namespace ToDoList.Application.Interfaces;

public interface IRecurringOccurrenceService
{
    Task<List<RecurringOccurrence>> GetOccurrencesAsync(
        string userId,
        DateTime rangeStart,
        DateTime rangeEnd);
}