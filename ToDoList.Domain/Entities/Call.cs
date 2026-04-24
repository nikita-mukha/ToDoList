using ToDoList.Enums;

namespace ToDoList.Models;

public class Call : ToDoItem
{
    public List<string> InvitedPerson {get; set;}
    public Call(DateTime targetDayTime, 
        ToDoItemTypes itemType, 
        string title, 
        bool isCompleted,
        List<string> invitedPerson,
        string description = "") 
        : base(targetDayTime, itemType, title, isCompleted, description)
    {
        InvitedPerson = invitedPerson;
    }
    public override string ToString()
    {
        return $"{base.ToString()}\n Invited: {string.Join(", ", InvitedPerson)}";
    }


}