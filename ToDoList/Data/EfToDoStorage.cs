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

    public ToDoItem? GetById(Guid id) => 
        _context.Items.FirstOrDefault(i => i.Id == id);

    public bool Remove(Guid id)
    {
        var item = _context.Items.FirstOrDefault(i => i.Id == id);
        if (item == null) 
            return false;
        _context.Items.Remove(item);
        _context.SaveChanges();
        return true;
    }

    public bool Complete(Guid id)
    {
        var item = _context.Items.FirstOrDefault(i => i.Id == id);
        if (item == null)
            return false;
        item.IsCompleted = true;
        _context.SaveChanges();
        return true;
    }

    public List<ToDoItem> GetAll() => _context.Items.ToList();

    public List<ToDoItem> GetActive() => 
        _context.Items.Where(i => i.IsCompleted == false).ToList();

    public List<ToDoItem> GetByDateRange(DateTime start, DateTime end) => 
        _context.Items.Where
            (i => i.TargetDayTime >= start 
                  && i.TargetDayTime <= end).ToList();

    public List<ToDoItem> GetByDate(DateTime date) =>
        _context.Items.Where(i => i.TargetDayTime == date).ToList();
}