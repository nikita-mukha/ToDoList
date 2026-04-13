using ToDoList.Enums;
using ToDoList.Models;
using Xunit;
using Assert = Xunit.Assert;
using Moq;
using ToDoList.Interfaces;
using ToDoList.UTests.TestResults;

namespace ToDoList.UTests;

public class ToDoManagerTests
{
    private ToDoList.Models.ToDoManager CreateToDoList()
    {
        var eventStorage = new FakeEventStorage();
        var storage = new FakeStorage();
        return new ToDoList.Models.ToDoManager(storage, eventStorage);
    }

    [Fact]
    public void AddItem_WhenCalled_StoresItemCorrectly()
    {
        var toDoList = CreateToDoList();
        var item = new ToDoList.Models.Task(
            targetDayTime: DateTime.Now,
            itemType: ToDoItemTypes.Task,
            title: "Test task",
            isCompleted: false);
        toDoList.AddItem(item);
        
        var items = toDoList.GetAllItems();
        
        Assert.Single(items);
        Assert.Equal("Test task", items[0].Title);
        Assert.IsType<ToDoList.Models.Task>(items[0]);
    }

    [Fact]
    public void RemoveItem_WhenCalled_RemovesItemCorrectly()
    {
        var toDoList = CreateToDoList();
        var item = new ToDoList.Models.Task(
            targetDayTime: DateTime.Now,
            itemType: ToDoItemTypes.Task,
            title: "Test task",
            isCompleted: false);
        toDoList.AddItem(item);
        
        var result = toDoList.RemoveItem("Test task");
        
        Assert.True(result);
        Assert.Empty(toDoList.GetAllItems());
    }

    [Fact]
    public void RemoveItem_WhenItemDoesNotExist_ReturnsFalse()
    {
        var toDoList = CreateToDoList();
        var item = new ToDoList.Models.Task(
            targetDayTime: DateTime.Now,
            itemType: ToDoItemTypes.Task,
            title: "Test task",
            isCompleted: false);
        toDoList.AddItem(item);
        
        var result = toDoList.RemoveItem("Test");
        
        Assert.False(result);
        Assert.Single(toDoList.GetAllItems());
    }

    [Fact]
    public void CompleteItem_WhenItemExists_ReturnsTrueAndSetsIsCompleted()
    {
        var toDoList = CreateToDoList();
        var item = new ToDoList.Models.Task(
            targetDayTime: DateTime.Now,
            itemType: ToDoItemTypes.Task,
            title: "Test task",
            isCompleted: false);
        toDoList.AddItem(item);
        
        var result = toDoList.CompleteItem("Test task");
        var items = toDoList.GetAllItems();
        
        Assert.True(result);
        Assert.True(items[0].IsCompleted);
    }

    [Fact]
    public void CompleteItem_WhenItemDoesNotExist_ReturnsFalse()
    {
        var toDoList = CreateToDoList();
        var item = new ToDoList.Models.Task(
            targetDayTime: DateTime.Now,
            itemType: ToDoItemTypes.Task,
            title: "Test task",
            isCompleted: false);
        toDoList.AddItem(item);
        
        var result = toDoList.CompleteItem("Test");
        var items = toDoList.GetAllItems();
        
        Assert.False(result);
        Assert.False(items[0].IsCompleted);
    }

    [Fact]
    public void GetActiveItems_WhenCalled_ReturnsOnlyActiveItems()
    {
        var toDoList = CreateToDoList();
        var item = new ToDoList.Models.Task(
            targetDayTime: DateTime.Now,
            itemType: ToDoItemTypes.Task,
            title: "Test task",
            isCompleted: false);
        toDoList.AddItem(item);
        
        var item2 = new ToDoList.Models.Task(
            targetDayTime: DateTime.Now,
            itemType: ToDoItemTypes.Task,
            title: "Test",
            isCompleted: false);
        toDoList.AddItem(item2);
        
        toDoList.CompleteItem("Test");
        
        Assert.Single(toDoList.GetActiveItems());
    }

    [Fact]
    public void GetItemsByDateTimeRange_WhenItemsInRange_ReturnsCorrectItems()
    {
        var toDoList = CreateToDoList();
        var item = new ToDoList.Models.Task(
            targetDayTime: DateTime.Now,
            itemType: ToDoItemTypes.Task,
            title: "Test task",
            isCompleted: false);
        toDoList.AddItem(item);
        
        var item2 = new ToDoList.Models.Task(
            targetDayTime: new DateTime(2020,1,1),
            itemType: ToDoItemTypes.Task,
            title: "Test",
            isCompleted: false);
            toDoList.AddItem(item2);
            
            var items = toDoList.GetItemsByDateTimeRange(
                new DateTime(2021, 1, 1),
                new DateTime(2029, 1, 1)); 
            
            Assert.Single(items); 
            Assert.Equal("Test task", items[0].Title);
    }
    
    [Fact]
    public void MoqTest_RemoveItem_WhenCalled_RemovesItemCorrectly()
    {
        var mockStorage = new Mock<IToDoStorage>();
        var mockEventStorage = new Mock<IEventStorage>();
        mockStorage
            .Setup(s => s.Load())
            .Returns(new List<ToDoItem>());
        var toDoList = new ToDoList.Models.ToDoManager(mockStorage.Object, mockEventStorage.Object);
        var item = new ToDoList.Models.Task(
            targetDayTime: DateTime.Now,
            itemType: ToDoItemTypes.Task,
            title: "Test task",
            isCompleted: false);
        toDoList.AddItem(item);
        
        var result = toDoList.RemoveItem("Test task");
        
        Assert.True(result);
        Assert.Empty(toDoList.GetAllItems());
    }
    
    [Theory]
    [InlineData("test task")]
    [InlineData("TEST TASK")]
    [InlineData("Test Task")]
    public void RemoveItem_IsCaseInsensitive_ReturnsTrue(string titleInput)
    {
        var toDoList = CreateToDoList();
        var item = new ToDoList.Models.Task(
            targetDayTime: DateTime.Now,
            itemType: ToDoItemTypes.Task,
            title: "Test task",
            isCompleted: false);
        toDoList.AddItem(item);
        
        var result = toDoList.RemoveItem(titleInput); 
        
        Assert.True(result);
        Assert.Empty(toDoList.GetAllItems());
    }
    
    [Theory]
    [InlineData("Test task", true, 0)]
    [InlineData("Not existing task", false, 1)]
    public void RemoveItem_ReturnsExpectedResult(string titleInput, bool expectedCompletion, int expectedCount)
    {
        var toDoList = CreateToDoList();
        var item = new ToDoList.Models.Task(
            targetDayTime: DateTime.Now,
            itemType: ToDoItemTypes.Task,
            title: "Test task",
            isCompleted: false);
        toDoList.AddItem(item);
        
        var result = toDoList.RemoveItem(titleInput);
        
        Assert.Equal(expectedCompletion, result);
        Assert.Equal(expectedCount, toDoList.GetAllItems().Count);
        
    }
}