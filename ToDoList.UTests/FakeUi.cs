using ToDoList.Enums;
using ToDoList.Interfaces;
using ToDoList.Models;

namespace ToDoList.UTests;

public class FakeUi : IUserInterface
{
    private readonly Queue<string> _answers; 
    public FakeUi(Queue<string> answers) => 
        _answers = answers;
    
    public void PrintSeparator() { }

    public string AskUser(string prompt) => _answers.Dequeue();

    public List<string> ReadUserNames(string prompt) => 
        _answers.Dequeue().Split(',').ToList();

    public DateTime AskDate(string prompt) => DateTime.Parse(_answers.Dequeue());

    public int PrintAvailableOptions() => 0;

    public DisplayOptions PrintDisplayOptions() => DisplayOptions.All;

    public void PrintItems(List<ToDoItem> items){ }

    public void PrintItemsByDateRange(DateTime startDate, DateTime endDate, List<ToDoItem> items){ }

    public void PrintItemsBySpecificDate(DateTime date, List<ToDoItem> items){ }

    public void PrintMessage(string message) { }
}