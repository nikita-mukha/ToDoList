using ToDoList.Enums;

namespace ToDoList.Models;

public class DateOfBirth : ToDoItem
{
    public string NameOfPersonWithDoB {get; set;}

    public DateOfBirth(DateTime targetDayTime,
        ToDoItemTypes itemType,
        string title,
        bool isCompleted,
        string nameOfPersonWithDoB,
        string description = "")
        : base(targetDayTime, itemType, description, title, isCompleted)
    {
        this.NameOfPersonWithDoB = nameOfPersonWithDoB;
    }

}