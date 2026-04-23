using System.ComponentModel.DataAnnotations;
using ToDoList.Enums;

namespace ToDoList.Web.Models;

public class CreateToDoItemViewModel
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title can't exceed 100 characters")]
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    [Display(Name = "Event Date")]
    [Required(ErrorMessage = "Date is required")]
    public DateTime TargetDayTime { get; set; } = DateTime.Now;
    [Display(Name = "Item Type")]
    public ToDoItemTypes ItemType { get; set; }
    [Display(Name = "Invited Person(s)")]
    public string? InvitedPersons { get; set; }
    public string? Place { get; set; }
    [Display(Name = "Person Name")]
    public string? PersonName { get; set; }
    public string? ConfirmedConflictKey { get; set; }
    public bool IgnoreTimeConflicts { get; set; }
}