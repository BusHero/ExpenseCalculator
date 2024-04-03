﻿// <auto-generated />
using ExpensesManager.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ExpenseManager.DataAccess.Migrations.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20240401193034_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.2");

            modelBuilder.Entity("ExpenseManager.Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ExternalId")
                        .IsUnique();

                    b.ToTable("DomainUsers");
                });

            modelBuilder.Entity("ExpenseManager.Domain.User", b =>
                {
                    b.OwnsMany("ExpenseManager.Domain.Expense", "Expenses", b1 =>
                        {
                            b1.Property<int>("UserId")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("INTEGER");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.HasKey("UserId", "Id");

                            b1.ToTable("DomainUsers");

                            b1.ToJson("Expenses");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("Expenses");
                });
#pragma warning restore 612, 618
        }
    }
}