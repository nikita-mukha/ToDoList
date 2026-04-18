using ToDoList.Interfaces;
using ToDoList.Models;

namespace ToDoList.UTests;

public class FakeStorage : IToDoStorage
{
    private readonly List<ToDoItem> _items = new();
    public void Add(ToDoItem item)
    {
        _items.Add(item);
    }
    public ToDoItem? GetById(Guid id, string userId) => 
        _items.FirstOrDefault(i => i.Id == id && i.UserId == userId);
    public bool Remove(Guid id, string userId)
    {
        var item = _items.FirstOrDefault(i => i.Id == id && i.UserId == userId);
        if (item == null)
            return false; 
        _items.Remove(item);
        return true;
    }

    public bool Complete(Guid id, string userId)
    {
        var item = _items.FirstOrDefault(i => i.Id == id && i.UserId == userId);
        if (item == null)
            return false;
        item.IsCompleted = true;
        return true;
    }

    public List<ToDoItem> GetAll(string userId) => _items.Where(i => i.UserId == userId).ToList();

    public List<ToDoItem> GetActive(string userId) => _items.Where(i => i.UserId == userId && !i.IsCompleted).ToList();

    public List<ToDoItem> GetByDateRange(DateTime start, DateTime end, string userId) =>
        _items.Where(i => i.UserId == userId
                          && i.TargetDayTime.Date >= start.Date
                          && i.TargetDayTime.Date <= end.Date).ToList();

    public List<ToDoItem> GetByDate(DateTime date, string userId) => 
        _items.Where(i => i.UserId == userId && i.TargetDayTime.Date == date.Date).ToList();

}