using ToDoList.Application.Interfaces;
using ToDoList.Domain.Entities;
using ToDoList.Web.Interfaces;
using ToDoList.Web.Models;

namespace ToDoList.Web.Services;

public class ToDoIndexService : IToDoIndexService
{
    private readonly IToDoManager _toDoManager;
    private readonly IRecurringOccurrenceService _recurringOccurrenceService;

    public ToDoIndexService(
        IToDoManager toDoManager,
        IRecurringOccurrenceService recurringOccurrenceService)
    {
        _toDoManager = toDoManager;
        _recurringOccurrenceService = recurringOccurrenceService;
    }

    public async Task<List<ToDoIndexItemViewModel>> GetIndexItemsAsync(
        string userId,
        DateTime? startDate,
        DateTime? endDate,
        string? title)
    {
        DateTime rangeStart;
        DateTime rangeEnd;

        if (startDate.HasValue && endDate.HasValue)
        {
            if (startDate.Value.Date > endDate.Value.Date)
            {
                endDate = startDate;
            }

            rangeStart = startDate.Value.Date;
            rangeEnd = endDate.Value.Date.AddDays(1).AddTicks(-1);
        }
        else if (startDate.HasValue)
        {
            rangeStart = startDate.Value.Date;
            rangeEnd = startDate.Value.Date.AddDays(1).AddTicks(-1);
        }
        else
        {
            rangeStart = DateTime.Today;
            rangeEnd = DateTime.Today.AddDays(31).AddTicks(-1);
        }

        List<ToDoItem> normalItems;

        if (startDate.HasValue)
        {
            normalItems = await _toDoManager.GetItemsByDateTimeRangeAsync(
                rangeStart,
                rangeEnd,
                userId);
        }
        else
        {
            normalItems = await _toDoManager.GetAllItemsAsync(userId);
        }

        var recurringOccurrences = await _recurringOccurrenceService.GetOccurrencesAsync(
            userId,
            rangeStart,
            rangeEnd);

        var sourceItemIds = recurringOccurrences
            .Select(occurrence => occurrence.SourceItemId)
            .Distinct()
            .ToList();

        var sourceItems = await _toDoManager.GetItemsByIdsAsync(sourceItemIds, userId);

        var sourceItemsById = sourceItems.ToDictionary(item => item.Id);

        var indexItems = normalItems
            .Select(item => new ToDoIndexItemViewModel
            {
                SourceItem = item,
                DisplayDateTime = item.TargetDayTime,
                IsRecurring = false,
                SeriesId = null
            })
            .ToList();

        foreach (var occurrence in recurringOccurrences)
        {
            if (!sourceItemsById.TryGetValue(occurrence.SourceItemId, out var sourceItem))
            {
                continue;
            }

            if (sourceItem.TargetDayTime == occurrence.OccurrenceDateTime)
            {
                continue;
            }

            indexItems.Add(new ToDoIndexItemViewModel
            {
                SourceItem = sourceItem,
                DisplayDateTime = occurrence.OccurrenceDateTime,
                IsRecurring = true,
                SeriesId = occurrence.SeriesId
            });
        }

        if (!string.IsNullOrWhiteSpace(title))
        {
            indexItems = indexItems
                .Where(item => item.SourceItem.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return indexItems;
    }
}