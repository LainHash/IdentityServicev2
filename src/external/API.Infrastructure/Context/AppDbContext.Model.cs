using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace API.Infrastructure.Context
{
    public partial class AppDbContext
    {
        // ── Model building ──────────────────────────────────────────────────
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            AddSequences(modelBuilder);

            // Auto-register all IEntityTypeConfiguration<T> classes in this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public static void AddSequences(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<long>("EmployeeSequence")
                .StartsAt(100610142026)
                .IncrementsBy(1);
        }
    }
}
