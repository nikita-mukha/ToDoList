using ToDoList.Domain.Enums;

namespace ToDoList.Domain.Entities;

public class Meeting : ToDoItem
{
    public List<string> InvitedPerson {get; set;}
    public string Place {get; set;}
    public Meeting(DateTime targetDayTime, 
        ToDoItemTypes itemType, 
        string title,   
        List<string> invitedPerson,
        bool isCompleted,
        string place,
        string description = "") 
        : base(targetDayTime, itemType,title, isCompleted, description)
    {
        InvitedPerson = invitedPerson;
        Place = place;
    }
    public override string ToString()
    {
        return $"{base.ToString()}\n Place: {Place}\n Invited: {string.Join(", ", InvitedPerson)}";
    }
    
}