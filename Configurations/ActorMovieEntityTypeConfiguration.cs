using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movies_Point.Models;

namespace Movies_Point.Configurations
{
    public class ActorMovieEntityTypeConfiguration : IEntityTypeConfiguration<ActorMovie>
    {
        public void Configure(EntityTypeBuilder<ActorMovie> builder)
        {
            builder.HasKey(e => new { e.ActorId, e.MovieId });
        }
    }
}
