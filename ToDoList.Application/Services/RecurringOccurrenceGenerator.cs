using ToDoList.Application.Interfaces;
using ToDoList.Application.Models;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Enums;

namespace ToDoList.Application.Services;

public class RecurringOccurrenceGenerator : IRecurringOccurrenceGenerator
{
    private static DateTime GetNextDate(
        DateTime currentDate,
        RecurrenceFrequency frequency,
        int interval)
    {
        if (interval < 1)
            throw new ArgumentOutOfRangeException(nameof(interval), "Interval must be at least 1.");
        return frequency switch
        {
            RecurrenceFrequency.Daily => currentDate.AddDays(interval),
            RecurrenceFrequency.Weekly => currentDate.AddDays(7 * interval),
            RecurrenceFrequency.Monthly => currentDate.AddMonths(interval),
            _ => throw new InvalidOperationException("Unsupported recurrence frequency")
        };
    }

    public List<RecurringOccurrence> Generate(RecurringSeries series, DateTime rangeStart, DateTime rangeEnd)
    {
        ArgumentNullException.ThrowIfNull(series);

        var result = new List<RecurringOccurrence>();
        var currentDate = series.StartDateTime;
        var endDate = rangeEnd;

        if (rangeEnd < rangeStart)
            return result;

        if (series.EndDateTime.HasValue && series.EndDateTime.Value < endDate)
            endDate = series.EndDateTime.Value;

        if (endDate < rangeStart)
            return result;

        while (currentDate < rangeStart)
            currentDate = GetNextDate(currentDate, series.Frequency, series.Interval);

        while (currentDate <= endDate)
        {
            result.Add(new RecurringOccurrence
            {
                SeriesId = series.Id,
                SourceItemId = series.SourceItemId,
                OccurrenceDateTime = currentDate
            });

            currentDate = GetNextDate(currentDate, series.Frequency, series.Interval);
        }
        return result;
    }
}