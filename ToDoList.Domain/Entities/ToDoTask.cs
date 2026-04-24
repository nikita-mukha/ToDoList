using ToDoList.Domain.Enums;

namespace ToDoList.Domain.Entities;

public class ToDoTask : ToDoItem
{

    public ToDoTask(DateTime targetDayTime,
        ToDoItemTypes itemType,
        bool isCompleted,
        string title,
        string description = "")
        : base(targetDayTime, itemType, title, isCompleted, description) { }
}