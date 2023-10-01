using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ChemicalConfiguration : IEntityTypeConfiguration<Chemical>
{
    public void Configure(EntityTypeBuilder<Chemical> builder)
    {
        builder.ToTable("Chemical");
        builder
            .HasKey(x => x.Id)
            .HasName("PK_Chemical_Id");
        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        builder
            .Property(x => x.ChemicalType)
            .HasMaxLength(100);
    }
}
