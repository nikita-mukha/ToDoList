using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ToDoList.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    [StringLength(100)]
    public string? DisplayName { get; set; }
}
