// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using cumin_api;

namespace cumin_api.Migrations
{
    [DbContext(typeof(CuminApiContext))]
    partial class CuminApiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.6");

            modelBuilder.Entity("cumin_api.Models.Epic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("Row")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Epics");
                });

            modelBuilder.Entity("cumin_api.Models.Issue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("AssignedToId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<int?>("EpicId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("ReporterId")
                        .HasColumnType("int");

                    b.Property<int?>("SprintId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("AssignedToId");

                    b.HasIndex("EpicId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("ReporterId");

                    b.HasIndex("SprintId");

                    b.ToTable("Issues");
                });

            modelBuilder.Entity("cumin_api.Models.Path", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("FromEpicId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("ToEpicId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FromEpicId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("ToEpicId");

                    b.ToTable("Paths");
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

                    b.HasIndex("ActiveSprintId")
                        .IsUnique();

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("cumin_api.Models.ProjectInvitation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("InvitedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("InviteeId")
                        .HasColumnType("int");

                    b.Property<int>("InviterId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("InviterId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("InviteeId", "InviterId", "ProjectId")
                        .IsUnique();

                    b.ToTable("ProjectInvitations");
                });

            modelBuilder.Entity("cumin_api.Models.Sprint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Title")
                        .IsRequired()
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

                    b.Property<int?>("ActiveProjectId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("ActiveProjectId");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("cumin_api.Models.UserProject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("UserRole")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("UserId", "ProjectId")
                        .IsUnique();

                    b.ToTable("UserProjects");
                });

            modelBuilder.Entity("cumin_api.Models.Epic", b =>
                {
                    b.HasOne("cumin_api.Models.Project", "Project")
                        .WithMany("Epics")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("cumin_api.Models.Issue", b =>
                {
                    b.HasOne("cumin_api.Models.User", "AssignedTo")
                        .WithMany("IssueAssigned")
                        .HasForeignKey("AssignedToId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("cumin_api.Models.Epic", "Epic")
                        .WithMany("Issues")
                        .HasForeignKey("EpicId")
                        .OnDelete(DeleteBehavior.SetNull);

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

                    b.HasOne("cumin_api.Models.Sprint", "Sprint")
                        .WithMany("Issues")
                        .HasForeignKey("SprintId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("AssignedTo");

                    b.Navigation("Epic");

                    b.Navigation("Project");

                    b.Navigation("Reporter");

                    b.Navigation("Sprint");
                });

            modelBuilder.Entity("cumin_api.Models.Path", b =>
                {
                    b.HasOne("cumin_api.Models.Epic", "FromEpic")
                        .WithMany("PathsFrom")
                        .HasForeignKey("FromEpicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("cumin_api.Models.Project", "Project")
                        .WithMany("Paths")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("cumin_api.Models.Epic", "ToEpic")
                        .WithMany("PathsTo")
                        .HasForeignKey("ToEpicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FromEpic");

                    b.Navigation("Project");

                    b.Navigation("ToEpic");
                });

            modelBuilder.Entity("cumin_api.Models.Project", b =>
                {
                    b.HasOne("cumin_api.Models.Sprint", "ActiveSprint")
                        .WithOne("ActiveForProject")
                        .HasForeignKey("cumin_api.Models.Project", "ActiveSprintId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("ActiveSprint");
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

            modelBuilder.Entity("cumin_api.Models.User", b =>
                {
                    b.HasOne("cumin_api.Models.Project", "ActiveProject")
                        .WithMany("ActiveForUser")
                        .HasForeignKey("ActiveProjectId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("ActiveProject");
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

            modelBuilder.Entity("cumin_api.Models.Epic", b =>
                {
                    b.Navigation("Issues");

                    b.Navigation("PathsFrom");

                    b.Navigation("PathsTo");
                });

            modelBuilder.Entity("cumin_api.Models.Project", b =>
                {
                    b.Navigation("ActiveForUser");

                    b.Navigation("Epics");

                    b.Navigation("Issues");

                    b.Navigation("Paths");

                    b.Navigation("ProjectInvitations");

                    b.Navigation("Sprints");

                    b.Navigation("UserProjects");
                });

            modelBuilder.Entity("cumin_api.Models.Sprint", b =>
                {
                    b.Navigation("ActiveForProject");

                    b.Navigation("Issues");
                });

            modelBuilder.Entity("cumin_api.Models.User", b =>
                {
                    b.Navigation("IssueAssigned");

                    b.Navigation("IssueReporter");

                    b.Navigation("ProjectInvitationSent");

                    b.Navigation("ProjectInvitedTo");

                    b.Navigation("UserProjects");
                });
#pragma warning restore 612, 618
        }
    }
}
