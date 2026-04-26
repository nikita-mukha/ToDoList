using ToDoList.Domain.Enums;

namespace ToDoList.Domain.Entities;

public class RecurringSeries
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; } = string.Empty;
    public Guid SourceItemId { get; set; }
    public DateTime StartDateTime { get; set; }
    public RecurrenceFrequency Frequency { get; set; }
    public int Interval { get; set; } = 1;
    public DateTime? EndDateTime { get; set; }
    public bool IsActive { get; set; } = true;
}