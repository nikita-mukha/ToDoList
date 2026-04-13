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

 public IActionResult Index()
 {
     var items = _toDoManager.GetAllItems();
     return View(items);
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
     _toDoManager.AddItem(item);
     return RedirectToAction("Index");
 }
 [HttpPost]
 public IActionResult Delete(Guid id)
 {
     _toDoManager.RemoveItem(id);
     return RedirectToAction("Index");
 }

 [HttpPost]
 public IActionResult Complete(Guid id)
 {
     _toDoManager.CompleteItem(id);
     return RedirectToAction("Index");
 }
}