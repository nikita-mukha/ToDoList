using ToDoList.Enums;

namespace ToDoList.Models;

public class ToDoList
{
    public List<ToDoItem> _items;

    public ToDoList()
    {
        _items = new List<ToDoItem>();
    }

    public void AddItem(ToDoItem item)
    {
        _items.Add(item);
    }
    //public li GetItemsByDateTimeRange(DateTime startDate, DateTime endDate)
}