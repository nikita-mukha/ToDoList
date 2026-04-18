using System.ComponentModel.DataAnnotations;

namespace ToDoList.Web.Models;

public class EditToDoItemViewModel
{
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title can't exceed 100 characters")]
        public required string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "Date is required")]
        [Display(Name = "Event Date")]
        public required DateTime TargetDayTime { get; set; }
}