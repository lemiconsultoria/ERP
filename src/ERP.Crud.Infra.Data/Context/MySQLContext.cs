using ERP.Crud.Domain.Entities;
using ERP.Crud.Infra.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ERP.Crud.Infra.Data.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext()
        { }

        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options)
        {
        }

        public DbSet<Entry> Entries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entry>(new EntryConfiguration().Configure);
            modelBuilder.Entity<EntryDebit>(new EntryDebitConfiguration().Configure);
            modelBuilder.Entity<EntryCredit>(new EntryCreditConfiguration().Configure);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("DefaultConnection", new MySqlServerVersion(new Version(8, 0, 33)));
                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);                
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("Created") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("Created").CurrentValue = DateTime.Now;
                    continue;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("Created").IsModified = false;
                    entry.Property("Updated").CurrentValue = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}