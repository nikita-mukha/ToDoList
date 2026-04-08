using ToDoList.Enums;

namespace ToDoList.Models;

public class Call : ToDoItem
{
    public string InvitedPerson {get; set;}
    public Call(DateTime targetDayTime, 
        ToDoItemTypes itemType, 
        string description, 
        string title, 
        string invitedPerson) 
        : base(targetDayTime, itemType, description, title)
    {
        InvitedPerson = invitedPerson;
    }
    
}