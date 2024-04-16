using Movies_Point.Models;
using System.ComponentModel.DataAnnotations;

namespace Movies_Point.ViewModels
{
    public class MoviesViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide a name.")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [Required(ErrorMessage = "Please provide a price.")]
        [Range(0, 100, ErrorMessage = "Price must be between 0 and 100.")]
        public double Price { get; set; }


        [Display(Name = "Movie Image URL")]
        public string? ImgUrl { get; set; }


        [Display(Name = "Movie Trailer URL")]
        [RegularExpression(@"^https:\/\/www\..*\.com$", ErrorMessage = "Invalid URL format. example: https://www.website.com")]
        public string? TrailerUrl { get; set; }


        [Required(ErrorMessage = "Please provide a Start Date.")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }


        [Required(ErrorMessage = "Please provide an End Date.")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }


        [Display(Name = "Cinema")]
        public int CinemaId { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public List<int> MovieActors { get; set; } = new List<int>();


    }
}
