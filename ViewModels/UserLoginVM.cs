using System.ComponentModel.DataAnnotations;

namespace Movies_Point.ViewModels
{
    public class UserLoginVM
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
