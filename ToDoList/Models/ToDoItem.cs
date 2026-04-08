using ToDoList.Enums;

namespace ToDoList.Models;

public abstract class ToDoItem
{
    public DateTime TargetDayTime {get; set;}
    public ToDoItemTypes ItemType {get; set;}
    public string? Description {get; set;}
    public string Title {get; set;}

    public ToDoItem(DateTime targetDayTime, ToDoItemTypes itemType, string description, string title)
    {
        TargetDayTime = targetDayTime;
        ItemType = itemType;
        Description = description;
        Title = title;
    }
    public override string ToString()
    {
        return $"Title: {Title}, Date: {TargetDayTime}, Description: {Description}";
    }
    
}