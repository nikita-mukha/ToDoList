using ToDoList.Enums;
using ToDoList.Models;

var toDoList = new ToDoList.Models.ToDoList();
toDoList.AddItem(new Call(
    targetDayTime: new DateTime(2026, 04, 25, 12, 00,00),
    itemType: ToDoItemTypes.Call,
    description: "Weekly sync meeting",
    title: "Team Call",
    invitedPerson: "John Doe"
));

foreach (var entry in toDoList._items)
{
Console.WriteLine(entry);
}