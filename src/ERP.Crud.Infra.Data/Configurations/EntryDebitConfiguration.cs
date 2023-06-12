using ERP.Crud.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Crud.Infra.Data.Configurations
{
    public class EntryDebitConfiguration : IEntityTypeConfiguration<EntryDebit>
    {
        public void Configure(EntityTypeBuilder<EntryDebit> builder)
        {
            builder.ToTable("entries_debit");
        }
    }
}