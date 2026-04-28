using ToDoList.Web.Models;

namespace ToDoList.Web.Interfaces;

public interface IToDoIndexService
{
    Task<List<ToDoIndexItemViewModel>> GetIndexItemsAsync(
        string userId,
        DateTime? startDate,
        DateTime? endDate,
        string? title);
}