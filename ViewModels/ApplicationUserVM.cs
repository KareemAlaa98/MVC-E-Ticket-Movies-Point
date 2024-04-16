using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Movies_Point.ViewModels
{
    public class ApplicationUserVM
    {
        public int Id { get; set;}
        
        [Required]
        [RegularExpression(@"^\S*$", ErrorMessage = "Username cannot contain spaces.")]
        public string UserName { get; set;}

        [Required]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$", ErrorMessage = "Please enter a valid email address. Example: john.doe@example.com")]
        public string Email { get; set;}


        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).*$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set;}

        [DataType(DataType.Password)]
        [Compare("Password")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set;}

        [Range(18, 120, ErrorMessage = "Age must be between 18 and 120. Please note that by submitting your age, you confirm its accuracy at your own responsibility.")]
        public int Age { get; set;}

    }
}
