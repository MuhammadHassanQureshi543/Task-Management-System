using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskManagementSystem.Models.Config
{
    public class UserConfig:IEntityTypeConfiguration<UsersTable>
    {
        public void Configure(EntityTypeBuilder<UsersTable> builder) {

            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder.Property(x=>x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x=>x.Password).IsRequired();
            builder.Property(x => x.Role).IsRequired().HasMaxLength(10);
            builder.Property(x => x.CreatedAt).IsRequired();
        }
    }
}
