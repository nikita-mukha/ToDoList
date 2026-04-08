using ToDoList.Enums;

namespace ToDoList.Models;

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
        : base(targetDayTime, itemType, description, title, isCompleted)
    {
        InvitedPerson = invitedPerson;
        Place = place;
    }
    
}