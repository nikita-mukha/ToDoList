namespace ToDoList.Application.Models;

public class RecurringOccurrence
{
    public Guid SeriesId { get; set; }
    public Guid SourceItemId { get; set; }
    public DateTime OccurrenceDateTime { get; set; }
    public bool IsCompleted { get; set; }
}