using ToDoList.Application.Interfaces;
using ToDoList.Application.Models;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Services;

public class RecurringOccurrenceService : IRecurringOccurrenceService
{
    private readonly IRecurringSeriesStorage _recurringSeriesStorage;
    private readonly IRecurringOccurrenceGenerator _recurringOccurrenceGenerator;
    private readonly IRecurringOccurrenceExceptionStorage _recurringOccurrenceExceptionStorage;

    public RecurringOccurrenceService(
        IRecurringSeriesStorage recurringSeriesStorage,
        IRecurringOccurrenceGenerator recurringOccurrenceGenerator,
        IRecurringOccurrenceExceptionStorage recurringOccurrenceExceptionStorage)
    {
        _recurringSeriesStorage = recurringSeriesStorage;
        _recurringOccurrenceGenerator = recurringOccurrenceGenerator;
        _recurringOccurrenceExceptionStorage = recurringOccurrenceExceptionStorage;
    }

    public async Task<List<RecurringOccurrence>> GetOccurrencesAsync(
        string userId,
        DateTime rangeStart,
        DateTime rangeEnd)
    {
        var result = new List<RecurringOccurrence>();

        var seriesList = await _recurringSeriesStorage.LoadActiveAsync(userId, rangeStart, rangeEnd);

        foreach (var series in seriesList)
        {
            var generatedOccurrences = _recurringOccurrenceGenerator.Generate(
                series,
                rangeStart,
                rangeEnd);
            result.AddRange(generatedOccurrences);
        }
        var exceptions = await _recurringOccurrenceExceptionStorage.LoadAsync(
            userId,
            rangeStart,
            rangeEnd);

        var exceptionsByOccurrence = exceptions.ToDictionary(
            exception => (exception.SeriesId, exception.OccurrenceDateTime));

        var finalResult = new List<RecurringOccurrence>();

        foreach (var occurrence in result)
        {
            if (!exceptionsByOccurrence.TryGetValue(
                    (occurrence.SeriesId, occurrence.OccurrenceDateTime),
                    out var exception))
            {
                finalResult.Add(occurrence);
                continue;
            }

            if (exception.IsCancelled)
            {
                continue;
            }

            if (exception.IsCompleted)
            {
                occurrence.IsCompleted = true;
            }

            finalResult.Add(occurrence);
        }

        return finalResult.OrderBy(o => o.OccurrenceDateTime).ToList();
    }
}