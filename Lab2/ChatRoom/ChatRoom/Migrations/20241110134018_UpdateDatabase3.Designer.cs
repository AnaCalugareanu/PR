﻿// <auto-generated />
using System;
using ChatRoom.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ChatRoom.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241110134018_UpdateDatabase3")]
    partial class UpdateDatabase3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("ChatRoom.Models.ChatRoomEntity", b =>
                {
                    b.Property<int>("ChatRoomEntityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ChatName")
                        .HasColumnType("TEXT");

                    b.HasKey("ChatRoomEntityId");

                    b.ToTable("ChatRooms");
                });

            modelBuilder.Entity("ChatRoom.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal?>("Price")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Specifications")
                        .HasColumnType("TEXT");

                    b.HasKey("ProductId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ChatRoom.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ChatRoomEntityUser", b =>
                {
                    b.Property<int>("ChatRoomsChatRoomEntityId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UsersUserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ChatRoomsChatRoomEntityId", "UsersUserId");

                    b.HasIndex("UsersUserId");

                    b.ToTable("ChatRoomEntityUser");
                });

            modelBuilder.Entity("ChatRoomEntityUser", b =>
                {
                    b.HasOne("ChatRoom.Models.ChatRoomEntity", null)
                        .WithMany()
                        .HasForeignKey("ChatRoomsChatRoomEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ChatRoom.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}