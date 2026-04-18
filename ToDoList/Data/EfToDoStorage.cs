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
        _context.Items.Where
            (i => i.UserId == userId && i.TargetDayTime >= start 
                  && i.TargetDayTime <= end).ToList();

    public List<ToDoItem> GetByDate(DateTime date, string userId) =>
        _context.Items.Where(i => i.UserId == userId && i.TargetDayTime == date).ToList();
}