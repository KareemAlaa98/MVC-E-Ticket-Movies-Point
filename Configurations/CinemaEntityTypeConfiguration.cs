using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movies_Point.Models;

namespace Movies_Point.Configurations
{
    public class CinemaEntityTypeConfiguration : IEntityTypeConfiguration<Cinema>
    {
        public void Configure(EntityTypeBuilder<Cinema> builder)
        {
            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.Address).IsRequired();
        }
    }
}
