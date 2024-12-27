using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementSystem.Models.Config
{
    public class TaskConfig : IEntityTypeConfiguration<TasksTable>
    {
        public void Configure(EntityTypeBuilder<TasksTable> builder)
        {

            builder.ToTable("Tasks");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Deadline).IsRequired();
            builder.Property(x => x.Status).IsRequired().HasMaxLength(20);
            builder.Property(x => x.AssignedTo).IsRequired();
            builder.Property(x => x.CreatedBy).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.UpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.AssignedUser)
            .WithMany()
            .HasForeignKey(x => x.AssignedTo)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CreatedUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
