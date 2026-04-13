using Microsoft.AspNetCore.Identity;

namespace ToDoList.Models;

public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }
}
