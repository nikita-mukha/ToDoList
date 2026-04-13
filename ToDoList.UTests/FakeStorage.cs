using ToDoList.Interfaces;
using ToDoList.Models;

namespace ToDoList.UTests;

public class FakeStorage : IToDoStorage
{
    private List<ToDoItem> _items = new();
    public void Add(ToDoItem item)
    {
        _items.Add(item);
    }
    public ToDoItem? GetById(Guid id) => 
        _items.FirstOrDefault(i => i.Id == id);
    public bool Remove(Guid id)
    {
        var item = _items.FirstOrDefault(i => i.Id == id);
        if (item == null)
            return false; 
        _items.Remove(item);
        return true;
    }

    public bool Complete(Guid id)
    {
        var item = _items.FirstOrDefault(i => i.Id == id);
        if (item == null)
            return false;
        item.IsCompleted =  true;
        return true;
    }

    public List<ToDoItem> GetAll() => _items.ToList();

    public List<ToDoItem> GetActive() => _items.Where(i => !i.IsCompleted).ToList();

    public List<ToDoItem> GetByDateRange(DateTime start, DateTime end) =>
        _items.Where(i => i.TargetDayTime.Date >= start.Date
                          && i.TargetDayTime.Date <= end.Date).ToList();

    public List<ToDoItem> GetByDate(DateTime date) => 
        _items.Where(i => i.TargetDayTime.Date == date.Date).ToList();

}