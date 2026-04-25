namespace ToDoList.Web.Models;

public class ToDoItemDto
{
    public Guid Id { get; set; }
    public DateTime TargetDayTime { get; set; }
    public string ItemType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }

}
