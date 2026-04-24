using ToDoList.Enums;
using System.Text.Json.Serialization;

namespace ToDoList.Models;
[JsonDerivedType(typeof(Call), typeDiscriminator: "Call")]
[JsonDerivedType(typeof(Meeting), typeDiscriminator: "Meeting")]
[JsonDerivedType(typeof(DateOfBirth), typeDiscriminator: "DateOfBirth")]
[JsonDerivedType(typeof(ToDoTask), typeDiscriminator: "Task")]
public abstract class ToDoItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; } = string.Empty;
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
        return $"\n Title: {Title}\n" + 
               $" Item type: {ItemType}\n" +
               $" Date: {TargetDayTime}\n" +
               $" Description: {Description}\n" +
               $" Item status: {IsCompleted}";
    }
    
}