﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyHotel.EntityFrameworkCore;

namespace MyHotel.Migrations
{
    [DbContext(typeof(MyHotelDbContext))]
    partial class MyHotelDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MyHotel.EntityFrameworkCore.Entities.Guest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.Property<int?>("NullableInt");

                    b.Property<DateTime>("RegisterDate");

                    b.HasKey("Id");

                    b.ToTable("Guests");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Alper Ebicoglu",
                            RegisterDate = new DateTime(2019, 7, 31, 9, 38, 20, 747, DateTimeKind.Local).AddTicks(2192)
                        },
                        new
                        {
                            Id = 2,
                            Name = "George Michael",
                            RegisterDate = new DateTime(2019, 8, 5, 9, 38, 20, 756, DateTimeKind.Local).AddTicks(9171)
                        },
                        new
                        {
                            Id = 3,
                            Name = "Daft Punk",
                            RegisterDate = new DateTime(2019, 8, 9, 9, 38, 20, 756, DateTimeKind.Local).AddTicks(9366)
                        });
                });

            modelBuilder.Entity("MyHotel.EntityFrameworkCore.Entities.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CheckinDate");

                    b.Property<DateTime>("CheckoutDate");

                    b.Property<int>("GuestId");

                    b.Property<int>("RoomId");

                    b.HasKey("Id");

                    b.HasIndex("GuestId");

                    b.HasIndex("RoomId");

                    b.ToTable("Reservations");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CheckinDate = new DateTime(2019, 8, 8, 9, 38, 20, 758, DateTimeKind.Local).AddTicks(380),
                            CheckoutDate = new DateTime(2019, 8, 13, 9, 38, 20, 758, DateTimeKind.Local).AddTicks(399),
                            GuestId = 1,
                            RoomId = 3
                        },
                        new
                        {
                            Id = 2,
                            CheckinDate = new DateTime(2019, 8, 9, 9, 38, 20, 758, DateTimeKind.Local).AddTicks(5151),
                            CheckoutDate = new DateTime(2019, 8, 14, 9, 38, 20, 758, DateTimeKind.Local).AddTicks(5167),
                            GuestId = 2,
                            RoomId = 4
                        });
                });

            modelBuilder.Entity("MyHotel.EntityFrameworkCore.Entities.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("AllowedSmoking");

                    b.Property<string>("Name")
                        .HasMaxLength(200);

                    b.Property<int>("Number");

                    b.Property<int?>("RoomDetailId");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("RoomDetailId");

                    b.ToTable("Rooms");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AllowedSmoking = false,
                            Name = "yellow-room",
                            Number = 101,
                            RoomDetailId = 100,
                            Status = 1
                        },
                        new
                        {
                            Id = 2,
                            AllowedSmoking = false,
                            Name = "blue-room",
                            Number = 102,
                            RoomDetailId = 101,
                            Status = 1
                        },
                        new
                        {
                            Id = 3,
                            AllowedSmoking = false,
                            Name = "white-room",
                            Number = 103,
                            RoomDetailId = 102,
                            Status = 0
                        },
                        new
                        {
                            Id = 4,
                            AllowedSmoking = false,
                            Name = "black-room",
                            Number = 104,
                            RoomDetailId = 103,
                            Status = 0
                        });
                });

            modelBuilder.Entity("MyHotel.EntityFrameworkCore.Entities.RoomDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Beds");

                    b.Property<int>("Windows");

                    b.HasKey("Id");

                    b.ToTable("RoomDetails");

                    b.HasData(
                        new
                        {
                            Id = 100,
                            Beds = 1,
                            Windows = 2
                        },
                        new
                        {
                            Id = 101,
                            Beds = 1,
                            Windows = 4
                        },
                        new
                        {
                            Id = 102,
                            Beds = 2,
                            Windows = 3
                        },
                        new
                        {
                            Id = 103,
                            Beds = 2,
                            Windows = 0
                        });
                });

            modelBuilder.Entity("MyHotel.EntityFrameworkCore.Entities.Reservation", b =>
                {
                    b.HasOne("MyHotel.EntityFrameworkCore.Entities.Guest", "Guest")
                        .WithMany()
                        .HasForeignKey("GuestId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MyHotel.EntityFrameworkCore.Entities.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MyHotel.EntityFrameworkCore.Entities.Room", b =>
                {
                    b.HasOne("MyHotel.EntityFrameworkCore.Entities.RoomDetail", "RoomDetail")
                        .WithMany()
                        .HasForeignKey("RoomDetailId");
                });
#pragma warning restore 612, 618
        }
    }
}
