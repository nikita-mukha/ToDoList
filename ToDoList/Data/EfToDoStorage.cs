using ToDoList.Interfaces;
using ToDoList.Models;

namespace ToDoList.Data;

public class EfToDoStorage : IToDoStorage
{
    private readonly AppDbContext _context;

    public EfToDoStorage(AppDbContext context)
    {
        _context = context;
    }

    public void Add(ToDoItem item)
    {
        _context.Items.Add(item);
        _context.SaveChanges();
    }

    public ToDoItem? GetById(Guid id, string userId) => 
        _context.Items.FirstOrDefault(i => i.Id == id && i.UserId == userId);

    public bool Remove(Guid id, string userId)
    {
        var item = _context.Items.FirstOrDefault(i => i.Id == id && i.UserId == userId);
        if (item == null) 
            return false;
        _context.Items.Remove(item);
        _context.SaveChanges();
        return true;
    }

    public bool Complete(Guid id, string userId)
    {
        var item = _context.Items.FirstOrDefault(i => i.Id == id && i.UserId == userId);
        if (item == null)
            return false;
        item.IsCompleted = true;
        _context.SaveChanges();
        return true;
    }

    public List<ToDoItem> GetAll(string userId) => 
        _context.Items.Where(i => i.UserId == userId).ToList();

    public List<ToDoItem> GetActive(string userId) => 
        _context.Items.Where(i => i.UserId == userId && i.IsCompleted == false).ToList();

    public List<ToDoItem> GetByDateRange(DateTime start, DateTime end, string userId) => 
        _context.Items.Where(i =>
            i.UserId == userId &&
            i.TargetDayTime.Date >= start.Date &&
            i.TargetDayTime.Date <= end.Date).ToList();

    public List<ToDoItem> GetByDate(DateTime date, string userId) =>
        _context.Items.Where(i => i.UserId == userId && i.TargetDayTime.Date == date.Date).ToList();
    
    public bool Update(Guid id, string userId, string title, string description, DateTime targetDayTime)
    {
        var item = _context.Items.FirstOrDefault(i => i.Id == id && i.UserId == userId);
        if (item == null)
            return false;
        item.Title = title;
        item.Description = description;
        item.TargetDayTime = targetDayTime;
        _context.SaveChanges();
        return true;
    }
}