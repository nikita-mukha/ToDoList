using ToDoList.Application.Interfaces;
using ToDoList.Application.Models;

namespace ToDoList.Application.Services;

public class RecurringOccurrenceService : IRecurringOccurrenceService
{
    private IRecurringSeriesStorage _recurringSeriesStorage;
    private IRecurringOccurrenceGenerator _recurringOccurrenceGenerator;

    public RecurringOccurrenceService(
        IRecurringSeriesStorage recurringSeriesStorage,
        IRecurringOccurrenceGenerator recurringOccurrenceGenerator)
    {
        _recurringSeriesStorage = recurringSeriesStorage;
        _recurringOccurrenceGenerator = recurringOccurrenceGenerator;
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
            var generatedOccurrences = _recurringOccurrenceGenerator.Generate(series, rangeStart, rangeEnd);
            result.AddRange(generatedOccurrences);
        }
        return result.OrderBy(o => o.OccurrenceDateTime).ToList();
    }
}