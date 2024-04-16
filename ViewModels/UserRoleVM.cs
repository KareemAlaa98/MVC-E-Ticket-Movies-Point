using System.ComponentModel.DataAnnotations;

namespace Movies_Point.ViewModels
{
    public class UserRoleVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
