using ToDoList.Enums;

namespace ToDoList.Models;

public class ToDoEvent
{ 
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; } = string.Empty;
    public ToDoEventTypes EventType { get; set; }
    public string Title {get; set;} = string.Empty;
    public DateTime Date {get; set;}

    public ToDoEvent(ToDoEventTypes toDoEvents, string userId, string title, DateTime date)
  {
    Title = title;
    UserId = userId;
    Date = date;
    EventType = toDoEvents;
  }
  public ToDoEvent() { }  
}