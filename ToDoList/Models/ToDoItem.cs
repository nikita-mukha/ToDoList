using ToDoList.Enums;

namespace ToDoList.Models;

public abstract class ToDoItem
{
    public DateTime TargetDayTime {get; set;}
    public ToDoItemTypes ItemType {get; set;}
    public string Description {get; set;}
    public string Title {get; set;}
    public bool IsCompleted {get; set;}

    public ToDoItem(DateTime targetDayTime, 
        ToDoItemTypes itemType, 
        string title, 
        bool isCompleted,
        string description = "")
    {
        TargetDayTime = targetDayTime;
        ItemType = itemType;
        Description = description;
        Title = title;
        IsCompleted = isCompleted;
    }
    public override string ToString()
    {
        return $"Title: {Title}, " + 
               $"Itemtype: {ItemType}, " +
               $"Date: {TargetDayTime}, " +
               $"Description: {Description}, " +
               $"Item status: {IsCompleted}";
    }
    
}