using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Application.Commons.Extensions;

namespace Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        builder
            .HasKey(x => x.Id)
            .HasName("PK_User_Id");
        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        builder
            .Property(x => x.Username)
            .HasMaxLength(100);
        builder
            .Property(x => x.Role)
            .HasDefaultValue(UserRole.Anonymous)
            .HasMaxLength(10)
            .HasConversion(
                x => x.ToStringValue(),
                x => x.ToUserRole()
            );
    }
}
