using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Enums;
using ToDoList.Interfaces;
using ToDoList.Models;
using ToDoList.Web.Models;

namespace ToDoList.Web.Controllers;

[Authorize]
public class ToDoController : Controller
{
    private readonly IToDoManager _toDoManager;

    public ToDoController(IToDoManager toDoManager)
    {
        _toDoManager = toDoManager;
    }

    public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, string? title)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        if (startDate.HasValue && endDate.HasValue && startDate.Value.Date > endDate.Value.Date)
        {
            endDate = startDate;
        }

        List<ToDoItem> items;

        if (startDate.HasValue)
        {
            items = endDate.HasValue
                ? await _toDoManager.GetItemsByDateTimeRangeAsync(startDate.Value, endDate.Value, userId)
                : await _toDoManager.GetItemsBySpecificDateAsync(startDate.Value, userId);
        }
        else
        {
            items = await _toDoManager.GetAllItemsAsync(userId);
        }

        if (!string.IsNullOrWhiteSpace(title))
        {
            items = items
                .Where(item => item.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return View(items);
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

        await _toDoManager.AddItemAsync(item);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _toDoManager.RemoveItemAsync(id, userId!);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Complete(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _toDoManager.CompleteItemAsync(id, userId!);
        return RedirectToAction("Index");
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
}
