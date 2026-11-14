using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using API.Domain.Features.Personnel.Departments;

namespace API.Infrastructure.Features.Personnel.Departments
{
    internal class DepartmentConfiguration
        : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .UseIdentityByDefaultColumn();

            builder.Property(x => x.PublicId)
                .IsRequired();
            builder.HasIndex(x => x.PublicId)
                .IsUnique();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.HasIndex(x => x.Code)
                .IsUnique();

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.HasMany(x => x.Positions)
                .WithOne(x => x.Department)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
