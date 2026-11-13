using API.Domain.Common.Helpers;
using API.Domain.Features.Personnel.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Features.Personnel.Employees
{
    internal class EmployeeConfiguration
        : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .UseIdentityByDefaultColumn();

            builder.Property(x => x.PublicId)
                .IsRequired();
            builder.HasIndex(x => x.PublicId)
                .IsUnique();

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .IsRequired();

            builder.Ignore(x => x.Code);
            builder.Property<long>("EmployeeSequence")
                .IsRequired()
                .HasDefaultValueSql("nextval('\"EmployeeSequence\"')")
                .ValueGeneratedOnAdd();
            builder.HasIndex("EmployeeSequence")
                .IsUnique();

            builder.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<Employee>(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
