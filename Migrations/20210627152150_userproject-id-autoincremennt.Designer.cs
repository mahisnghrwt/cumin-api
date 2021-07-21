﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using cumin_api;

namespace cumin_api.Migrations
{
    [DbContext(typeof(CuminApiContext))]
    [Migration("20210627152150_userproject-id-autoincremennt")]
    partial class userprojectidautoincremennt
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.6");

            modelBuilder.Entity("cumin_api.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("cumin_api.Models.ProjectInvitation", b =>
                {
                    b.Property<int>("InviteeId")
                        .HasColumnType("int");

                    b.Property<int>("InviterId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("InvitedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("InviteeId", "InviterId", "ProjectId");

                    b.HasAlternateKey("Id");

                    b.HasIndex("InviterId");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectInvitations");
                });

            modelBuilder.Entity("cumin_api.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Username");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("cumin_api.Models.UserProject", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.HasKey("UserId", "ProjectId");

                    b.HasAlternateKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("UserProjects");
                });

            modelBuilder.Entity("cumin_api.Models.ProjectInvitation", b =>
                {
                    b.HasOne("cumin_api.Models.User", "Invitee")
                        .WithMany("ProjectInvitationSent")
                        .HasForeignKey("InviteeId")
                        .HasConstraintName("invitee-projectinvitation")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("cumin_api.Models.User", "Inviter")
                        .WithMany("ProjectInvitedTo")
                        .HasForeignKey("InviterId")
                        .HasConstraintName("inviter-projectinvitation")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("cumin_api.Models.Project", "Project")
                        .WithMany("ProjectInvitations")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Invitee");

                    b.Navigation("Inviter");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("cumin_api.Models.UserProject", b =>
                {
                    b.HasOne("cumin_api.Models.Project", "Project")
                        .WithMany("UserProjects")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("cumin_api.Models.User", "User")
                        .WithMany("UserProjects")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("User");
                });

            modelBuilder.Entity("cumin_api.Models.Project", b =>
                {
                    b.Navigation("ProjectInvitations");

                    b.Navigation("UserProjects");
                });

            modelBuilder.Entity("cumin_api.Models.User", b =>
                {
                    b.Navigation("ProjectInvitationSent");

                    b.Navigation("ProjectInvitedTo");

                    b.Navigation("UserProjects");
                });
#pragma warning restore 612, 618
        }
    }
}
