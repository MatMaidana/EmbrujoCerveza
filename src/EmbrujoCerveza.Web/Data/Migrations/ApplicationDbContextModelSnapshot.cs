using System;
using EmbrujoCerveza.Web.Data;
using EmbrujoCerveza.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EmbrujoCerveza.Web.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("EmbrujoCerveza.Web.Models.BeerLot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("BeerStyleId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("BottledOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("BottleCount")
                        .HasColumnType("integer");

                    b.Property<int>("BottleTypeId")
                        .HasColumnType("integer");

                    b.Property<string>("Notes")
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.HasKey("Id");

                    b.HasIndex("BeerStyleId");

                    b.HasIndex("BottleTypeId");

                    b.ToTable("BeerLots");
                });

            modelBuilder.Entity("EmbrujoCerveza.Web.Models.BeerStyle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<decimal>("Abv")
                        .HasColumnType("numeric");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int?>("Ibu")
                        .HasColumnType("integer");

                    b.Property<string>("ImageFileName")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("BeerStyles");
                });

            modelBuilder.Entity("EmbrujoCerveza.Web.Models.BottleType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("CapacityMl")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Material")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("character varying(80)");

                    b.HasKey("Id");

                    b.ToTable("BottleTypes");
                });

            modelBuilder.Entity("EmbrujoCerveza.Web.Models.BeerLot", b =>
                {
                    b.HasOne("EmbrujoCerveza.Web.Models.BeerStyle", "BeerStyle")
                        .WithMany("Lots")
                        .HasForeignKey("BeerStyleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EmbrujoCerveza.Web.Models.BottleType", "BottleType")
                        .WithMany("Lots")
                        .HasForeignKey("BottleTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BeerStyle");

                    b.Navigation("BottleType");
                });

            modelBuilder.Entity("EmbrujoCerveza.Web.Models.BeerStyle", b =>
                {
                    b.Navigation("Lots");
                });

            modelBuilder.Entity("EmbrujoCerveza.Web.Models.BottleType", b =>
                {
                    b.Navigation("Lots");
                });
#pragma warning restore 612, 618
        }
    }
}
