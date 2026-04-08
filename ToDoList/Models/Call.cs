using ToDoList.Enums;

namespace ToDoList.Models;

public class Call : ToDoItem
{
    public List<string> InvitedPerson {get; set;}
    public Call(DateTime targetDayTime, 
        ToDoItemTypes itemType, 
        string description, 
        string title, 
        bool isCompleted,
        List<string> invitedPerson) 
        : base(targetDayTime, itemType, description, title, isCompleted)
    {
        InvitedPerson = invitedPerson;
    }

}