using ToDoList.Application.Services;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Enums;

namespace ToDoList.Tests;

public class RecurringOccurrenceGeneratorTests
{
    [Fact]
    public void Generate_WeeklySeries_ReturnsOccurrencesInsideRange()
    {
        var generator = new RecurringOccurrenceGenerator();

        var series = new RecurringSeries
        {
            SourceItemId = Guid.NewGuid(),
            StartDateTime = new DateTime(2026, 4, 1, 10, 0, 0),
            Frequency = RecurrenceFrequency.Weekly,
            Interval = 1
        };

        var rangeStart = new DateTime(2026, 4, 10);
        var rangeEnd = new DateTime(2026, 4, 30);

        var result = generator.Generate(series, rangeStart, rangeEnd);

        Assert.Equal(3, result.Count);
        Assert.Equal(new DateTime(2026, 4, 15, 10, 0, 0), result[0].OccurrenceDateTime);
        Assert.Equal(new DateTime(2026, 4, 22, 10, 0, 0), result[1].OccurrenceDateTime);
        Assert.Equal(new DateTime(2026, 4, 29, 10, 0, 0), result[2].OccurrenceDateTime);
    }

    [Fact]
    public void Generate_WeeklySeriesWithIntervalTwo_SkipsEveryOtherWeek()
    {
        var generator = new RecurringOccurrenceGenerator();

        var series = new RecurringSeries
        {
            SourceItemId = Guid.NewGuid(),
            StartDateTime = new DateTime(2026, 4, 1, 10, 0, 0),
            Frequency = RecurrenceFrequency.Weekly,
            Interval = 2
        };

        var rangeStart = new DateTime(2026, 4, 1);
        var rangeEnd = new DateTime(2026, 5, 1);

        var result = generator.Generate(series, rangeStart, rangeEnd);

        Assert.Equal(3, result.Count);
        Assert.Equal(new DateTime(2026, 4, 1, 10, 0, 0), result[0].OccurrenceDateTime);
        Assert.Equal(new DateTime(2026, 4, 15, 10, 0, 0), result[1].OccurrenceDateTime);
        Assert.Equal(new DateTime(2026, 4, 29, 10, 0, 0), result[2].OccurrenceDateTime);
    }

    [Fact]
    public void Generate_WhenSeriesHasEndDate_DoesNotGenerateAfterEndDate()
    {
        var generator = new RecurringOccurrenceGenerator();

        var series = new RecurringSeries
        {
            SourceItemId = Guid.NewGuid(),
            StartDateTime = new DateTime(2026, 4, 1, 10, 0, 0),
            EndDateTime = new DateTime(2026, 4, 15, 10, 0, 0),
            Frequency = RecurrenceFrequency.Weekly,
            Interval = 2
        };

        var rangeStart = new DateTime(2026, 4, 1);
        var rangeEnd = new DateTime(2026, 5, 1);

        var result = generator.Generate(series, rangeStart, rangeEnd);

        Assert.Equal(2, result.Count);
        Assert.Equal(new DateTime(2026, 4, 1, 10, 0, 0), result[0].OccurrenceDateTime);
        Assert.Equal(new DateTime(2026, 4, 15, 10, 0, 0), result[1].OccurrenceDateTime);
    }

    [Fact]
    public void Generate_WhenRangeEndIsBeforeRangeStart_ReturnsEmptyList()
    {
        var generator = new RecurringOccurrenceGenerator();

        var series = new RecurringSeries
        {
            SourceItemId = Guid.NewGuid(),
            StartDateTime = new DateTime(2026, 4, 1, 10, 0, 0),
            EndDateTime = new DateTime(2026, 4, 15, 10, 0, 0),
            Frequency = RecurrenceFrequency.Weekly,
            Interval = 2
        };

        var rangeStart = new DateTime(2026, 4, 1);
        var rangeEnd = new DateTime(2026, 3, 1);

        var result = generator.Generate(series, rangeStart, rangeEnd);

        Assert.Empty(result);
    }
}