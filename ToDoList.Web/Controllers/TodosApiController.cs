using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Interfaces;
using ToDoList.Web.Models;

namespace ToDoList.Web.Controllers;

[ApiController]
[Route("api/todos")]
[Authorize]
public class TodosApiController : ControllerBase
{
    private readonly IToDoManager _toDoManager;

    public TodosApiController(IToDoManager toDoManager)
    {
        _toDoManager = toDoManager;
    }

    [HttpGet]
    public async Task<IActionResult> Get(string? title)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var items = await _toDoManager.GetAllItemsAsync(userId!);

        if (!string.IsNullOrWhiteSpace(title))
        {
            items = items.Where(item => item.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        var result = items.Select(item => new ToDoItemDto
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            TargetDayTime = item.TargetDayTime,
            IsCompleted = item.IsCompleted,
            ItemType = item.ItemType.ToString()
        }).ToList();

        return Ok(result);
    }
}