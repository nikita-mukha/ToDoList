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
}