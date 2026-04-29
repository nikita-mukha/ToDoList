using ToDoList.Domain.Entities;

namespace ToDoList.Web.Models;

public class ToDoIndexItemViewModel
{
    public required ToDoItem SourceItem { get; set; }
    public Guid? SeriesId { get; set; }
    public bool IsRecurring { get; set; }
    public DateTime DisplayDateTime { get; set; }
    public bool IsCompleted { get; set; }
}