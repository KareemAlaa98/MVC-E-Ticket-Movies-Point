namespace Movies_Point.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Movie>? Movies { get; set; }
    }
}
