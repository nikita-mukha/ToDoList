using ToDoList.Enums;

namespace ToDoList.Models;

public class Task : ToDoItem
{

    public Task(DateTime targetDayTime,
        ToDoItemTypes itemType,
        bool isCompleted,
        string title,
        string description = "")
        : base(targetDayTime, itemType, title, isCompleted, description) { }
}