using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Interfaces;
using ToDoList.Domain.Entities;

namespace ToDoList.Infrastructure.Persistence;

public class EfEventStorage : IEventStorage
{
    private readonly AppDbContext _context;

    public EfEventStorage(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveAsync(ToDoEvent toDoEvent)
    {
        _context.Events.Add(toDoEvent);
        await _context.SaveChangesAsync();
    }

    public Task<List<ToDoEvent>> LoadAsync() => _context.Events.ToListAsync();
}