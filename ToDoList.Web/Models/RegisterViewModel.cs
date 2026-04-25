using System.ComponentModel.DataAnnotations;

namespace ToDoList.Web.Models;

public class RegisterViewModel
{
    [Display(Name = "User Name")]
    [Required]
    public string UserName { get; set; } = string.Empty;

    [Display(Name = "Password")]
    [Required]
    [MinLength(4, ErrorMessage = "Password must be at least 4 characters")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Confirm Password")]
    [Required]
    [Compare("Password", ErrorMessage = "Passwords don't match")]
    public string ConfirmPassword { get; set; } = string.Empty;
}