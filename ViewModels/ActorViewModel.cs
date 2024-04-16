using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Movies_Point.ViewModels
{
    public class ActorViewModel
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public string? Bio { get; set; }

        [DisplayName("Profile Picture")]
        public string? ProfilePicture { get; set; }
        public string? News { get; set; }

        public List<int> MoviesIds { get; set; } = new List<int>();
    }
}
