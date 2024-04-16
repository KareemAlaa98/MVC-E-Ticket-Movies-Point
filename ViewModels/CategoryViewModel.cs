using System.ComponentModel.DataAnnotations;

namespace Movies_Point.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        
        [Required(ErrorMessage = "Please provide a name.")]
        public string Name { get; set; } = null!;
    }
}
