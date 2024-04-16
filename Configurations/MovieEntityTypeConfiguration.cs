using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movies_Point.Models;

namespace Movies_Point.Configurations
{
    public class MovieEntityTypeConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            //// make the Movie Name, EtartDate, EndDate required
            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.StartDate).IsRequired();
            builder.Property(e => e.EndDate).IsRequired();
          
            //// make the MovieStatus a computed column based on Start & End dates
            builder.Property(e=>e.MovieStatus).HasComputedColumnSql("CASE " +
                                              "WHEN GETDATE() < StartDate THEN 0 " +
                                              "WHEN GETDATE() > EndDate THEN 2 " +
                                              "ELSE 1 END");
        }
    }
}
