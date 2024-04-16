

using System.ComponentModel.DataAnnotations;

namespace Movies_Point.ViewModels
{
    public class CinemaViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }

        //[Display(Name="Cinema Logo")]
        //public string? CinemaLogo { get; set; }
        [Required]
        public string Address { get; set; }

        public List<int> MoviesIds { get; set; } = new List<int>();
    }
}
