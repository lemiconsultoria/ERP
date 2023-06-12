using ERP.Crud.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Crud.Infra.Data.Configurations
{
    public class EntryCreditConfiguration : IEntityTypeConfiguration<EntryCredit>
    {
        public void Configure(EntityTypeBuilder<EntryCredit> builder)
        {
            builder.ToTable("entries_credit");
        }
    }
}