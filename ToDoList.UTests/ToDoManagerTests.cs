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
    private const string TestUserId = "user-1";
    private const string TestUserId2 = "user-2";
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
        item.UserId = TestUserId;
        
        toDoList.AddItem(item);
        
        var items = toDoList.GetAllItems(TestUserId);
        
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
        item.UserId = TestUserId;
        toDoList.AddItem(item);
        
        var result = toDoList.RemoveItem(item.Id, TestUserId);
        
        Assert.True(result);
        Assert.Empty(toDoList.GetAllItems(TestUserId));
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
        item.UserId = TestUserId;
        toDoList.AddItem(item);
        
        var result = toDoList.RemoveItem(Guid.NewGuid(), TestUserId);
        
        Assert.False(result);
        Assert.Single(toDoList.GetAllItems(TestUserId));
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
        item.UserId = TestUserId;
        toDoList.AddItem(item);
        
        var result = toDoList.CompleteItem(item.Id, TestUserId);
        var items = toDoList.GetAllItems(TestUserId);
        
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
        item.UserId = TestUserId;
        toDoList.AddItem(item);
        
        var result = toDoList.CompleteItem(Guid.NewGuid(), TestUserId);
        var items = toDoList.GetAllItems(TestUserId);
        
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
        item.UserId = TestUserId;
        toDoList.AddItem(item);
        
        var item2 = new ToDoList.Models.Task(
            targetDayTime: DateTime.Now,
            itemType: ToDoItemTypes.Task,
            title: "Test",
            isCompleted: false);
        item2.UserId = TestUserId;
        toDoList.AddItem(item2);
        
        toDoList.CompleteItem(item2.Id, TestUserId);
        
        Assert.Single(toDoList.GetActiveItems(TestUserId));
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
        item.UserId = TestUserId;
        toDoList.AddItem(item);
        
        var item2 = new ToDoList.Models.Task(
            targetDayTime: new DateTime(2020,1,1),
            itemType: ToDoItemTypes.Task,
            title: "Test",
            isCompleted: false);
        item2.UserId = TestUserId;
        toDoList.AddItem(item2);
            
        var items = toDoList.GetItemsByDateTimeRange(
            new DateTime(2021, 1, 1),
            new DateTime(2029, 1, 1),
            TestUserId); 
        
        Assert.Single(items); 
        Assert.Equal("Test task", items[0].Title);
    }
    
    [Theory]
    [InlineData(true, true, 0)]
    [InlineData(false, false, 1)]
    public void RemoveItem_ReturnsExpectedResult(bool useCorrectUserId, bool expectedCompletion, int expectedCount)
    {
        var toDoList = CreateToDoList();
        var item = new ToDoList.Models.Task(
            targetDayTime: DateTime.Now,
            itemType: ToDoItemTypes.Task,
            title: "Test task",
            isCompleted: false);
        item.UserId = TestUserId;
        toDoList.AddItem(item);

        var userId = useCorrectUserId ? TestUserId : TestUserId2;
        var result = toDoList.RemoveItem(item.Id, userId);

        Assert.Equal(expectedCompletion, result);
        Assert.Equal(expectedCount, toDoList.GetAllItems(TestUserId).Count);
    }
}