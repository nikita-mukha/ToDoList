using ToDoList.Enums;

namespace ToDoList.Models;

public class ToDoTask : ToDoItem
{

    public ToDoTask(DateTime targetDayTime,
        ToDoItemTypes itemType,
        bool isCompleted,
        string title,
        string description = "")
        : base(targetDayTime, itemType, title, isCompleted, description) { }
}