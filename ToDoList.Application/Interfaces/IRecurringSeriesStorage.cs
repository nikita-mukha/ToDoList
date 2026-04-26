using ToDoList.Domain.Entities;

namespace ToDoList.Application.Interfaces;

public interface IRecurringSeriesStorage
{
    Task SaveAsync(RecurringSeries series);
    Task<List<RecurringSeries>> LoadActiveAsync(string userId, DateTime rangeStart, DateTime rangeEnd);
}