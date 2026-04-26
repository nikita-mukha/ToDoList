using ToDoList.Application.Models;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Interfaces;

public interface IRecurringOccurrenceGenerator
{
    List<RecurringOccurrence> Generate(
        RecurringSeries series,
        DateTime rangeStart,
        DateTime rangeEnd);
}
