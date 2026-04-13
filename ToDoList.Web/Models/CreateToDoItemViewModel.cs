using System.ComponentModel.DataAnnotations;
using ToDoList.Enums;

namespace ToDoList.Web.Models;

public class CreateToDoItemViewModel
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title can't exceed 100 characters")]
    public required string  Title { get; set; }
    public string? Description { get; set; }
    [Required(ErrorMessage = "Date is required")]
    public required DateTime TargetDayTime { get; set; }
    public required ToDoItemTypes ItemType { get; set; }
    public string? InvitedPersons { get; set; }
    public string? Place { get; set; }
    public string? PersonName { get; set; }
}