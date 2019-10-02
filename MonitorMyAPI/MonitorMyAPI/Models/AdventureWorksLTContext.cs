using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MonitorMyAPI.Models
{
    public partial class AdventureWorksLTContext : DbContext
    {
        public AdventureWorksLTContext()
        {
        }

        public AdventureWorksLTContext(DbContextOptions<AdventureWorksLTContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.EmailAddress);

                entity.HasIndex(e => e.Rowguid)
                    .HasName("AK_Customer_rowguid")
                    .IsUnique();

                entity.Property(e => e.ModifiedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PasswordHash).IsUnicode(false);

                entity.Property(e => e.PasswordSalt).IsUnicode(false);

                entity.Property(e => e.Rowguid).HasDefaultValueSql("(newid())");
            });

            modelBuilder.HasSequence<int>("SalesOrderNumber", "SalesLT");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
