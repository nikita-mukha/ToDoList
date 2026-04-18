using System.ComponentModel.DataAnnotations;

namespace ToDoList.Web.Models;

public class LoginViewModel
{
    [Display(Name = "User Name")]
    [Required]
    public string UserName { get; set; }

    [Display(Name = "Password")]
    [Required]
    public string Password { get; set; }
}