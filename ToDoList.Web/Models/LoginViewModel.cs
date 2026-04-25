using System.ComponentModel.DataAnnotations;

namespace ToDoList.Web.Models;

public class LoginViewModel
{
    [Display(Name = "User Name")]
    [Required]
    public string UserName { get; set; } = string.Empty;

    [Display(Name = "Password")]
    [Required]
    public string Password { get; set; } = string.Empty;
}