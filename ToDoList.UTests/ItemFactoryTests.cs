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
    public void Create_WhenTypeIsTask_ReturnsTaskWithCorrectProperties()
    {
        var factory = CreateFactory(new Queue<string>(new[]
        {
            "Buy juice",
            "desc",
            "11/11/2026 14:00",
            "4"
        }));
        
        var result = factory.Create();
        
        Assert.IsType<ToDoTask>(result);
        Assert.Equal(new DateTime 
            (2026,11,11,14,00,00),
            result.TargetDayTime);
        Assert.Equal("Buy juice", result.Title);
        Assert.Equal("desc", result.Description);
        Assert.False(result.IsCompleted);
    }
    
    [Fact]
    public void Create_WhenTypeIsCall_ReturnsCallWithCorrectProperties()
    {
        var factory = CreateFactory(new Queue<string>(new[]
        {
            "Call",
            "desc",
            "11/11/2026 14:00",
            "1", 
            "John,Jane"
        }));
        
        var call = Assert.IsType<Call>(factory.Create());
        
        Assert.Equal("Call", call.Title);
        Assert.Equal("desc", call.Description);
        Assert.Equal(new DateTime
            (2026, 11, 11, 14, 00, 00),
            call.TargetDayTime);
        Assert.Contains("John", call.InvitedPerson);
        Assert.Contains("Jane", call.InvitedPerson);
    }

    [Fact]
    public void Create_WhenTypeIsMeeting_ReturnsMeetingWithCorrectProperties()
    {
        var factory = CreateFactory(new Queue<string>(new[]
        {
            "Meeting",
            "desc", 
            "11/11/2026 14:00",
            "2", 
            "John,Jane", 
            "Office"
        }));
        
        var meeting = Assert.IsType<Meeting>(factory.Create());
        
        Assert.Equal("Meeting", meeting.Title);
        Assert.Equal("desc", meeting.Description);
        Assert.Equal(new DateTime
                (2026, 11, 11, 14, 00, 00),
            meeting.TargetDayTime);
        Assert.Equal("Office", meeting.Place);
        Assert.Contains("John", meeting.InvitedPerson);
        Assert.Contains("Jane", meeting.InvitedPerson);
    }

    [Fact]
    public void Create_WhenTypeIsDateOfBirth_ReturnsDateOfBirthWithCorrectProperties()
    {
        var factory = CreateFactory(new Queue<string>(new[]
        {
            "DateOfBirth",
            "desc", 
            "11/11/2026 14:00",
            "3", 
            "John"
        }));
        
        var dob = Assert.IsType<DateOfBirth>(factory.Create());
            
        Assert.Equal("DateOfBirth", dob.Title);
        Assert.Equal("desc", dob.Description);
        Assert.Equal(new DateTime
            (2026, 11, 11, 14, 00, 00),
            dob.TargetDayTime);
        Assert.Equal("John", dob.NameOfPersonWithDoB);
    }

    [Fact]
    public void Create_WhenTypeIsInvalid_ThrowsInvalidOperationException()
    {
        var factory = CreateFactory(new Queue<string>(new[] {
            "DateOfBirth",
            "desc", 
            "11/11/2026 14:00",
            "6", 
            "John"
        })); 
        Assert.Throws<InvalidOperationException>(() => factory.Create());
    }
    [Fact]
    public void Create_WhenRequiredParameterIsMissing_ThrowsInvalidOperationException()
    {
        var factory = CreateFactory(new Queue<string>(new[] {
            "DateOfBirth",
            "desc", 
        })); 
        Assert.Throws<InvalidOperationException>(() => factory.Create());
    }
}