using ToDoList.Enums;
using ToDoList.Models;
using Xunit;
using Assert = Xunit.Assert;
using Moq;
using ToDoList.Interfaces;

namespace ToDoList.UTests;

public class ItemFactoryTests
{
    private ItemFactory CreateFactory(Queue<string> answers)
    {
        var fakeUi = new FakeUi(answers);
        return new ItemFactory(fakeUi);
    }

    [Fact]
    public void Create_WhenTypeIsTask_ReturnsTaskItem()
    {
        var factory = CreateFactory(new Queue<string>(new[]
        {
            "Buy juice",
            "desc",
            "11/11/2026 14:00",
            "4"
        }));
        
        var result = factory.Create();
        
        Assert.IsType<ToDoList.Models.Task>(result);
    }
    
}