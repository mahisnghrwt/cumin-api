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
    [Migration("20210705010658_issue-status-added")]
    partial class issuestatusadded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.6");

            modelBuilder.Entity("cumin_api.Models.ActiveSprintProject", b =>
                {
                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("SprintId")
                        .HasColumnType("int");

                    b.HasKey("ProjectId");

                    b.HasAlternateKey("Id");

                    b.HasIndex("SprintId")
                        .IsUnique();

                    b.ToTable("ActiveSprintProject");
                });

            modelBuilder.Entity("cumin_api.Models.Issue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("ReporterId")
                        .HasColumnType("int");

                    b.Property<int?>("ResolverId")
                        .HasColumnType("int");

                    b.Property<int?>("SprintId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.Property<string>("Type")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("ReporterId");

                    b.HasIndex("ResolverId");

                    b.HasIndex("SprintId");

                    b.ToTable("Issues");
                });

            modelBuilder.Entity("cumin_api.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("ActiveSprintId")
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

            modelBuilder.Entity("cumin_api.Models.Sprint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("ActiveSprintId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Sprints");
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

            modelBuilder.Entity("cumin_api.Models.ActiveSprintProject", b =>
                {
                    b.HasOne("cumin_api.Models.Project", "Project")
                        .WithOne("ActiveSprint")
                        .HasForeignKey("cumin_api.Models.ActiveSprintProject", "ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("cumin_api.Models.Sprint", "Sprint")
                        .WithOne("ActiveSprint")
                        .HasForeignKey("cumin_api.Models.ActiveSprintProject", "SprintId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("Sprint");
                });

            modelBuilder.Entity("cumin_api.Models.Issue", b =>
                {
                    b.HasOne("cumin_api.Models.Project", "Project")
                        .WithMany("Issues")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("cumin_api.Models.User", "Reporter")
                        .WithMany("IssueReporter")
                        .HasForeignKey("ReporterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("cumin_api.Models.User", "Resolver")
                        .WithMany("IssueResolver")
                        .HasForeignKey("ResolverId");

                    b.HasOne("cumin_api.Models.Sprint", "Sprint")
                        .WithMany("Issues")
                        .HasForeignKey("SprintId");

                    b.Navigation("Project");

                    b.Navigation("Reporter");

                    b.Navigation("Resolver");

                    b.Navigation("Sprint");
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

            modelBuilder.Entity("cumin_api.Models.Sprint", b =>
                {
                    b.HasOne("cumin_api.Models.Project", "Project")
                        .WithMany("Sprints")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

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
                    b.Navigation("ActiveSprint");

                    b.Navigation("Issues");

                    b.Navigation("ProjectInvitations");

                    b.Navigation("Sprints");

                    b.Navigation("UserProjects");
                });

            modelBuilder.Entity("cumin_api.Models.Sprint", b =>
                {
                    b.Navigation("ActiveSprint");

                    b.Navigation("Issues");
                });

            modelBuilder.Entity("cumin_api.Models.User", b =>
                {
                    b.Navigation("IssueReporter");

                    b.Navigation("IssueResolver");

                    b.Navigation("ProjectInvitationSent");

                    b.Navigation("ProjectInvitedTo");

                    b.Navigation("UserProjects");
                });
#pragma warning restore 612, 618
        }
    }
}
