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

 public IActionResult Index(DateTime? startDate, DateTime? endDate, string? title)
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
             ? _toDoManager.GetItemsByDateTimeRange(startDate.Value, endDate.Value, userId)
             : _toDoManager.GetItemsBySpecificDate(startDate.Value, userId);
     }
     else
     {
         items = _toDoManager.GetAllItems(userId);
     }

     if (!string.IsNullOrWhiteSpace(title))
     {
         items = items
             .Where(item => item.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
             .ToList();
     }

     return View(items);
 }
 public IActionResult Events()
 {
     var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
     var events = _toDoManager.GetAllEvents(userId!);
     return View(events);
 }
 public IActionResult Create()
 {
     return View();
 }

 [HttpPost]
 public IActionResult Create(CreateToDoItemViewModel model)
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
     _toDoManager.AddItem(item);
     return RedirectToAction("Index");
 }
 [HttpPost]
 public IActionResult Delete(Guid id)
 {
     var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
     _toDoManager.RemoveItem(id, userId!);
     return RedirectToAction("Index");
 }

 [HttpPost]
 public IActionResult Complete(Guid id)
 {
     var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
     _toDoManager.CompleteItem(id, userId!);
     return RedirectToAction("Index");
 }
 
 public IActionResult Edit(Guid id)
 {
     var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
     var item = _toDoManager.GetItemById(id, userId!);
     
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
 public IActionResult Edit(EditToDoItemViewModel model)
     { 
         if (!ModelState.IsValid)
             return View(model);

         var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

         var updated = _toDoManager.UpdateItem(
             model.Id,
             userId!,
             model.Title,
             model.Description,
             model.TargetDayTime);

         if (!updated)
             return NotFound();

         return RedirectToAction("Index");
     }
 }
 