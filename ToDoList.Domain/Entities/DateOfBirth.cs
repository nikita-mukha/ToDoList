using ToDoList.Domain.Enums;

namespace ToDoList.Domain.Entities;

public class DateOfBirth : ToDoItem
{
    public string NameOfPersonWithDoB { get; set; }

    public DateOfBirth(DateTime targetDayTime,
        ToDoItemTypes itemType,
        string title,
        bool isCompleted,
        string nameOfPersonWithDoB,
        string description = "")
        : base(targetDayTime, itemType, title, isCompleted, description)
    {
        NameOfPersonWithDoB = nameOfPersonWithDoB;
    }
    public override string ToString()
    {
        return $"{base.ToString()}\n Person: {NameOfPersonWithDoB}";
    }

}