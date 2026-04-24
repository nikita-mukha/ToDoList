using Microsoft.AspNetCore.Identity;

namespace ToDoList.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }
}
