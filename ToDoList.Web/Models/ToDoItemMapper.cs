using ToDoList.Enums;
using ToDoList.Models;

namespace ToDoList.Web.Models;

public static class ToDoItemMapper
{
    public static ToDoItem FromViewModel(CreateToDoItemViewModel model)
    {
        return model.ItemType switch
        { 
            ToDoItemTypes.Task => new ToDoTask(
                targetDayTime: model.TargetDayTime,
                itemType: model.ItemType,
                title: model.Title,
                isCompleted: false,
                description: model.Description ?? ""),

            ToDoItemTypes.Call => new Call(
                targetDayTime: model.TargetDayTime,
                itemType: model.ItemType,
                title: model.Title,
                isCompleted: false,
                description: model.Description ?? "",
                invitedPerson: model.InvitedPersons?.Split(',').ToList() ?? new List<string>()),

            ToDoItemTypes.Meeting => new Meeting(
                targetDayTime: model.TargetDayTime,
                itemType: model.ItemType,
                title: model.Title,
                isCompleted: false,
                description: model.Description ?? "",
                invitedPerson: model.InvitedPersons?.Split(',').ToList() ?? new List<string>(),
                place: model.Place ?? ""),

            ToDoItemTypes.DayOfBirth => new DateOfBirth(
                targetDayTime: model.TargetDayTime,
                itemType: model.ItemType,
                title: model.Title,
                isCompleted: false,
                description: model.Description ?? "",
                nameOfPersonWithDoB: model.PersonName ?? ""),

            _ => throw new ArgumentException("Invalid item type")
        };
    }
}