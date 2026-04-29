namespace ToDoList.Domain.Entities;

public class RecurringOccurrenceException
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; } = string.Empty;
    public Guid SeriesId { get; set; }
    public DateTime OccurrenceDateTime { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsCancelled { get; set; }
}