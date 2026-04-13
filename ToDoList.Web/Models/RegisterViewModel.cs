using System.ComponentModel.DataAnnotations;

namespace ToDoList.Web.Models;

public class RegisterViewModel
{
        [Required]
        public string UserName { get; set; }

        [Required]
        [MinLength(4, ErrorMessage = "Password must be at least 4 characters")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        public string ConfirmPassword { get; set; }
}