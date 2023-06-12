using ERP.Crud.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Crud.Infra.Data.Configurations
{
    internal class EntryConfiguration : IEntityTypeConfiguration<Entry>
    {
        public void Configure(EntityTypeBuilder<Entry> builder)
        {
            builder.ToTable("entries");

            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.Description)
                .IsRequired()
                .HasColumnName("Description")
                .HasMaxLength(60)
                .HasColumnType("VARCHAR");

            builder.Property(prop => prop.Value)
                .IsRequired()
                .HasColumnName("Value")
                .HasColumnType("Decimal")
                .HasPrecision(14, 2);

            builder.Property(prop => prop.DateOfIssue)
                .IsRequired()
                .HasColumnName("DateOfIssue")
                .HasColumnType("DATETIME");
        }
    }
}