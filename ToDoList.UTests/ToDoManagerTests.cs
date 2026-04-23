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
    public async Task AddItem_WhenCalled_StoresItemCorrectly()
    {
        var toDoList = CreateToDoList();
        var item = new ToDoTask(
            targetDayTime: DateTime.Now,
            itemType: ToDoItemTypes.Task,
            title: "Test task",
            isCompleted: false);
        item.UserId = TestUserId;

        await toDoList.AddItemAsync(item);

        var items = await toDoList.GetAllItemsAsync(TestUserId);

        Assert.Single(items);
        Assert.Equal("Test task", items[0].Title);
        Assert.IsType<ToDoTask>(items[0]);
    }

    [Fact]
    public async Task RemoveItem_WhenCalled_RemovesItemCorrectly()
    {
        var toDoList = CreateToDoList();
        var item = new ToDoTask(
            targetDayTime: DateTime.Now,
            itemType: ToDoItemTypes.Task,
            title: "Test task",
            isCompleted: false);
        item.UserId = TestUserId;
        await toDoList.AddItemAsync(item);

        var result = await toDoList.RemoveItemAsync(item.Id, TestUserId);

        Assert.True(result);
        Assert.Empty(await toDoList.GetAllItemsAsync(TestUserId));
    }

    [Fact]
    public async Task RemoveItem_WhenItemDoesNotExist_ReturnsFalse()
    {
        var toDoList = CreateToDoList();
        var item = new ToDoTask(
            targetDayTime: DateTime.Now,
            itemType: ToDoItemTypes.Task,
            title: "Test task",
            isCompleted: false);
        item.UserId = TestUserId;
        await toDoList.AddItemAsync(item);

        var result = await toDoList.RemoveItemAsync(Guid.NewGuid(), TestUserId);

        Assert.False(result);
        Assert.Single(await toDoList.GetAllItemsAsync(TestUserId));
    }

    [Fact]
    public async Task CompleteItem_WhenItemExists_ReturnsTrueAndSetsIsCompleted()
    {
        var toDoList = CreateToDoList();
        var item = new ToDoTask(
            targetDayTime: DateTime.Now,
            itemType: ToDoItemTypes.Task,
            title: "Test task",
            isCompleted: false);
        item.UserId = TestUserId;
        await toDoList.AddItemAsync(item);

        var result = await toDoList.CompleteItemAsync(item.Id, TestUserId);
        var items = await toDoList.GetAllItemsAsync(TestUserId);

        Assert.True(result);
        Assert.True(items[0].IsCompleted);
    }

    [Fact]
    public async Task CompleteItem_WhenItemDoesNotExist_ReturnsFalse()
    {
        var toDoList = CreateToDoList();
        var item = new ToDoTask(
            targetDayTime: DateTime.Now,
            itemType: ToDoItemTypes.Task,
            title: "Test task",
            isCompleted: false);
        item.UserId = TestUserId;
        await toDoList.AddItemAsync(item);

        var result = await toDoList.CompleteItemAsync(Guid.NewGuid(), TestUserId);
        var items = await toDoList.GetAllItemsAsync(TestUserId);

        Assert.False(result);
        Assert.False(items[0].IsCompleted);
    }

    [Fact]
    public async Task GetActiveItems_WhenCalled_ReturnsOnlyActiveItems()
    {
        var toDoList = CreateToDoList();
        var item = new ToDoTask(
            targetDayTime: DateTime.Now,
            itemType: ToDoItemTypes.Task,
            title: "Test task",
            isCompleted: false);
        item.UserId = TestUserId;
        await toDoList.AddItemAsync(item);

        var item2 = new ToDoTask(
            targetDayTime: DateTime.Now,
            itemType: ToDoItemTypes.Task,
            title: "Test",
            isCompleted: false);
        item2.UserId = TestUserId;
        await toDoList.AddItemAsync(item2);

        await toDoList.CompleteItemAsync(item2.Id, TestUserId);

        Assert.Single(await toDoList.GetActiveItemsAsync(TestUserId));
    }

    [Fact]
    public async Task GetItemsByDateTimeRange_WhenItemsInRange_ReturnsCorrectItems()
    {
        var toDoList = CreateToDoList();
        var item = new ToDoTask(
            targetDayTime: DateTime.Now,
            itemType: ToDoItemTypes.Task,
            title: "Test task",
            isCompleted: false);
        item.UserId = TestUserId;
        await toDoList.AddItemAsync(item);

        var item2 = new ToDoTask(
            targetDayTime: new DateTime(2020, 1, 1),
            itemType: ToDoItemTypes.Task,
            title: "Test",
            isCompleted: false);
        item2.UserId = TestUserId;
        await toDoList.AddItemAsync(item2);

        var items = await toDoList.GetItemsByDateTimeRangeAsync(
            new DateTime(2021, 1, 1),
            new DateTime(2029, 1, 1),
            TestUserId);

        Assert.Single(items);
        Assert.Equal("Test task", items[0].Title);
    }

    [Theory]
    [InlineData(true, true, 0)]
    [InlineData(false, false, 1)]
    public async Task RemoveItem_ReturnsExpectedResult(bool useCorrectUserId, bool expectedCompletion, int expectedCount)
    {
        var toDoList = CreateToDoList();
        var item = new ToDoTask(
            targetDayTime: DateTime.Now,
            itemType: ToDoItemTypes.Task,
            title: "Test task",
            isCompleted: false);
        item.UserId = TestUserId;
        await toDoList.AddItemAsync(item);

        var userId = useCorrectUserId ? TestUserId : TestUserId2;
        var result = await toDoList.RemoveItemAsync(item.Id, userId);

        Assert.Equal(expectedCompletion, result);
        Assert.Equal(expectedCount, (await toDoList.GetAllItemsAsync(TestUserId)).Count);
    }
}
