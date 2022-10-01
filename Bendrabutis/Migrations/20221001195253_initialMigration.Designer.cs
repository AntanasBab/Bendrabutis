﻿// <auto-generated />
using System;
using Bendrabutis.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Bendrabutis.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20221001195253_initialMigration")]
    partial class initialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Bendrabutis.Models.Dormitory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoomCapacity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Dormitories");
                });

            modelBuilder.Entity("Bendrabutis.Models.Floor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("DormitoryId")
                        .HasColumnType("int");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DormitoryId");

                    b.ToTable("Floors");
                });

            modelBuilder.Entity("Bendrabutis.Models.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RequestType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("Bendrabutis.Models.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<double>("Area")
                        .HasColumnType("float");

                    b.Property<int?>("FloorId")
                        .HasColumnType("int");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfLivingPlaces")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FloorId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Bendrabutis.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<int?>("RoomId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Bendrabutis.Models.Floor", b =>
                {
                    b.HasOne("Bendrabutis.Models.Dormitory", null)
                        .WithMany("Floors")
                        .HasForeignKey("DormitoryId");
                });

            modelBuilder.Entity("Bendrabutis.Models.Request", b =>
                {
                    b.HasOne("Bendrabutis.Models.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Bendrabutis.Models.Room", b =>
                {
                    b.HasOne("Bendrabutis.Models.Floor", null)
                        .WithMany("Rooms")
                        .HasForeignKey("FloorId");
                });

            modelBuilder.Entity("Bendrabutis.Models.User", b =>
                {
                    b.HasOne("Bendrabutis.Models.Room", "Room")
                        .WithMany("Residents")
                        .HasForeignKey("RoomId");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Bendrabutis.Models.Dormitory", b =>
                {
                    b.Navigation("Floors");
                });

            modelBuilder.Entity("Bendrabutis.Models.Floor", b =>
                {
                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("Bendrabutis.Models.Room", b =>
                {
                    b.Navigation("Residents");
                });
#pragma warning restore 612, 618
        }
    }
}
