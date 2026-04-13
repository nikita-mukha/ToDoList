using ToDoList.Interfaces;
using ToDoList.Models;

namespace ToDoList.Data;

public class EfEventStorage : IEventStorage
{
    private readonly AppDbContext _context;
    
    public EfEventStorage(AppDbContext context)
    {
        _context = context;
    }
    public void Save(ToDoEvent toDoEvent)
    {
        _context.Events.Add(toDoEvent);
        _context.SaveChanges();
    }

    public List<ToDoEvent> Load() => _context.Events.ToList();
}