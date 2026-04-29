using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Interfaces;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Enums;
using ToDoList.Web.Interfaces;
using ToDoList.Web.Models;

namespace ToDoList.Web.Controllers;

[Authorize]
public class ToDoController : Controller
{
    private readonly IToDoManager _toDoManager;
    private readonly IRecurringToDoService _recurringToDoService;
    private readonly IToDoIndexService _toDoIndexService;

    public ToDoController(
        IToDoManager toDoManager,
        IRecurringToDoService recurringToDoService,
        IToDoIndexService toDoIndexService)
    {
        _toDoManager = toDoManager;
        _recurringToDoService = recurringToDoService;
        _toDoIndexService = toDoIndexService;
    }

    public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, string? title)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var indexItems = await _toDoIndexService.GetIndexItemsAsync(
            userId,
            startDate,
            endDate,
            title);

        return View(indexItems);
    }

    public async Task<IActionResult> Events()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var events = await _toDoManager.GetAllEventsAsync(userId!);
        return View(events);
    }

    public IActionResult Create()
    {
        return View(new CreateToDoItemViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateToDoItemViewModel model)
    {
        if (model.ItemType == ToDoItemTypes.Call && string.IsNullOrEmpty(model.InvitedPersons))
            ModelState.AddModelError("InvitedPersons", "Invited persons are required for a Call");

        if (model.ItemType == ToDoItemTypes.Meeting && string.IsNullOrEmpty(model.InvitedPersons))
            ModelState.AddModelError("InvitedPersons", "Invited persons are required for a Meeting");

        if (model.ItemType == ToDoItemTypes.Meeting && string.IsNullOrEmpty(model.Place))
            ModelState.AddModelError("Place", "Place is required for a Meeting");

        if (model.ItemType == ToDoItemTypes.DayOfBirth && string.IsNullOrEmpty(model.PersonName))
            ModelState.AddModelError("PersonName", "Person name is required for a Date of Birth");

        if (model.IsRecurring)
        {
            if (!model.RecurrenceFrequency.HasValue)
                ModelState.AddModelError(nameof(model.RecurrenceFrequency),
                    "Recurrence frequency is required");

            if (model.RecurrenceEndDateTime.HasValue && model.RecurrenceEndDateTime.Value < model.TargetDayTime)
                ModelState.AddModelError(nameof(model.RecurrenceEndDateTime),
                    "Recurrence end should be after the Target event date");
        }

        if (!ModelState.IsValid)
            return View(model);

        var item = ToDoItemMapper.FromViewModel(model);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        item.UserId = userId!;

        var currentConflictKey = ToConflictKey(item.TargetDayTime);
        var hasConflict = await _toDoManager.HasTimeConflictItemAsync(item.TargetDayTime, userId!, null);
        var isConfirmed = model.IgnoreTimeConflicts && model.ConfirmedConflictKey == currentConflictKey;

        if (hasConflict && !isConfirmed)
        {
            ModelState.AddModelError("",
                "You already have another item scheduled for this time. " +
                "Change the time please or continue anyway");

            model.IgnoreTimeConflicts = true;
            model.ConfirmedConflictKey = currentConflictKey;

            ModelState.Remove(nameof(model.IgnoreTimeConflicts));
            ModelState.Remove(nameof(model.ConfirmedConflictKey));

            return View(model);
        }

        if (model.IsRecurring)
            await _recurringToDoService.CreateRecurringItemAsync(
                item,
                model.RecurrenceFrequency!.Value,
                model.RecurrenceInterval,
                model.RecurrenceEndDateTime);
        else
            await _toDoManager.AddItemAsync(item);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(
        Guid id,
        DateTime? startDate,
        DateTime? endDate,
        string? title,
        bool showCompleted)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _toDoManager.RemoveItemAsync(id, userId!);
        return RedirectToAction("Index", new
        {
            startDate,
            endDate,
            title,
            showCompleted
        });
    }

    [HttpPost]
    public async Task<IActionResult> Complete(
        Guid id,
        DateTime? startDate,
        DateTime? endDate,
        string? title,
        bool showCompleted)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _toDoManager.CompleteItemAsync(id, userId!);
        return RedirectToAction("Index", new
        {
            startDate,
            endDate,
            title,
            showCompleted
        });
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var item = await _toDoManager.GetItemByIdAsync(id, userId!);

        if (item == null) return NotFound();

        var model = new EditToDoItemViewModel()
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            TargetDayTime = item.TargetDayTime
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditToDoItemViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var currentConflictKey = ToConflictKey(model.TargetDayTime);
        var hasConflict = await _toDoManager.HasTimeConflictItemAsync(model.TargetDayTime, userId!, model.Id);
        var isConfirmed = model.IgnoreTimeConflicts && model.ConfirmedConflictKey == currentConflictKey;

        if (hasConflict && !isConfirmed)
        {
            ModelState.AddModelError("",
                "You already have another item scheduled for this time. " +
                "Change the time please or continue anyway");

            model.IgnoreTimeConflicts = true;
            model.ConfirmedConflictKey = currentConflictKey;

            ModelState.Remove(nameof(model.IgnoreTimeConflicts));
            ModelState.Remove(nameof(model.ConfirmedConflictKey));

            return View(model);
        }

        var updated = await _toDoManager.UpdateItemAsync(
            model.Id,
            userId!,
            model.Title,
            model.Description,
            model.TargetDayTime);

        if (!updated)
            return NotFound();

        return RedirectToAction("Index");
    }

    private static string ToConflictKey(DateTime date) =>
        $"{date.Year:D4}{date.Month:D2}{date.Day:D2}{date.Hour:D2}{date.Minute:D2}";

    [HttpPost]
    public async Task<IActionResult> StopRecurrence(
        Guid seriesId,
        DateTime? startDate,
        DateTime? endDate,
        string? title,
        bool showCompleted)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var stopped = await _recurringToDoService.StopRecurringSeriesAsync(seriesId, userId!);

        if (!stopped)
            return NotFound();

        return RedirectToAction("Index", new
        {
            startDate,
            endDate,
            title,
            showCompleted
        });
    }

    [HttpPost]
    public async Task<IActionResult> CompleteRecurringOccurrence(
        Guid seriesId,
        DateTime occurrenceDateTime,
        DateTime? startDate,
        DateTime? endDate,
        string? title,
        bool showCompleted)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _recurringToDoService.CompleteRecurringOccurrenceAsync(seriesId, occurrenceDateTime, userId!);

        return RedirectToAction("Index", new
        {
            startDate,
            endDate,
            title,
            showCompleted
        });
    }

    [HttpPost]
    public async Task<IActionResult> CancelRecurringOccurrence(
        Guid seriesId,
        DateTime occurrenceDateTime,
        DateTime? startDate,
        DateTime? endDate,
        string? title,
        bool showCompleted)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _recurringToDoService.CancelRecurringOccurrenceAsync(seriesId, occurrenceDateTime, userId!);

        return RedirectToAction("Index", new
        {
            startDate,
            endDate,
            title,
            showCompleted
        });
    }
}
