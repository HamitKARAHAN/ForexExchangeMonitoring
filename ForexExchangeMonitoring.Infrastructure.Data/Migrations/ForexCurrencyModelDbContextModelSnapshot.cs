﻿// <auto-generated />
using System;
using ForexExchangeMonitoring.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ForexExchangeMonitoring.Infrastructure.Data.Migrations
{
    [DbContext(typeof(ForexCurrencyModelDbContext))]
    partial class ForexCurrencyModelDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ForexExchangeMonitoring.Domain.Models.CurrencyModel", b =>
                {
                    b.Property<int>("CurrencyModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CurrencyName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CurrencyModelId");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("ForexExchangeMonitoring.Domain.Models.ForexCurrencyModel", b =>
                {
                    b.Property<int>("ForexCurrencyModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CurrencyModelId")
                        .HasColumnType("int");

                    b.Property<string>("ExchangeRate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FromCurrencyCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastRefreshedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ToCurrencyCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ForexCurrencyModelId");

                    b.HasIndex("CurrencyModelId");

                    b.ToTable("RealTimeCurrencyExchangeRates");
                });

            modelBuilder.Entity("ForexExchangeMonitoring.Domain.Models.ForexCurrencyModel", b =>
                {
                    b.HasOne("ForexExchangeMonitoring.Domain.Models.CurrencyModel", "currencyModelId")
                        .WithMany()
                        .HasForeignKey("CurrencyModelId");

                    b.Navigation("currencyModelId");
                });
#pragma warning restore 612, 618
        }
    }
}
