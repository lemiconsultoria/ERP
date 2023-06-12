﻿// <auto-generated />
using System;
using ERP.Crud.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ERP.Crud.Infra.Data.Migrations
{
    [DbContext(typeof(MySQLContext))]
    partial class MySQLContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ERP.Crud.Domain.Entities.Entry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DateOfIssue")
                        .HasColumnType("DATETIME")
                        .HasColumnName("DateOfIssue");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("Description");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("Value")
                        .HasPrecision(14, 2)
                        .HasColumnType("Decimal")
                        .HasColumnName("Value");

                    b.HasKey("Id");

                    b.ToTable("entries", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("ERP.Crud.Domain.Entities.EntryCredit", b =>
                {
                    b.HasBaseType("ERP.Crud.Domain.Entities.Entry");

                    b.ToTable("entries_credit", (string)null);
                });

            modelBuilder.Entity("ERP.Crud.Domain.Entities.EntryDebit", b =>
                {
                    b.HasBaseType("ERP.Crud.Domain.Entities.Entry");

                    b.ToTable("entries_debit", (string)null);
                });

            modelBuilder.Entity("ERP.Crud.Domain.Entities.EntryCredit", b =>
                {
                    b.HasOne("ERP.Crud.Domain.Entities.Entry", null)
                        .WithOne()
                        .HasForeignKey("ERP.Crud.Domain.Entities.EntryCredit", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ERP.Crud.Domain.Entities.EntryDebit", b =>
                {
                    b.HasOne("ERP.Crud.Domain.Entities.Entry", null)
                        .WithOne()
                        .HasForeignKey("ERP.Crud.Domain.Entities.EntryDebit", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
