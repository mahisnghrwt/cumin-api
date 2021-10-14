using cumin_api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api {
    public class CuminApiContext : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }
        public DbSet<ProjectInvitation> ProjectInvitations { get; set; }
        public DbSet<Sprint> Sprints { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Epic> Epics { get; set; }
        public DbSet<Path> Paths { get; set; }

        public CuminApiContext(DbContextOptions<CuminApiContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            //==============================================================================
            // pk
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().Property(x => x.Id).ValueGeneratedOnAdd();
            // fks
            modelBuilder.Entity<User>()
               .HasOne(x => x.ActiveProject)
               .WithMany(x => x.ActiveForUser)
               .HasForeignKey(x => x.ActiveProjectId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.SetNull);
            // constriants
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            //==============================================================================
            // pk
            modelBuilder.Entity<Project>().HasKey(x => x.Id);
            modelBuilder.Entity<Project>().Property(x => x.Id).ValueGeneratedOnAdd();
            // fk
            modelBuilder.Entity<Project>()
                .HasOne(x => x.ActiveSprint)
                .WithOne(x => x.ActiveForProject)
                .HasForeignKey<Project>(x => x.ActiveSprintId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            //==============================================================================
            // pk
            modelBuilder.Entity<UserProject>().HasKey(x => x.Id);
            modelBuilder.Entity<UserProject>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<UserProject>().HasIndex(t => new { t.UserId, t.ProjectId }).IsUnique();
            // fks
            modelBuilder.Entity<UserProject>()
                .HasOne(up => up.Project)
                .WithMany(p => p.UserProjects)
                .HasForeignKey(up => up.ProjectId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserProject>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserProjects)
                .HasForeignKey(up => up.UserId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
            // fields
            modelBuilder.Entity<UserProject>().Property(u => u.UserRole).IsRequired();
            //==============================================================================
            //pk
            modelBuilder.Entity<ProjectInvitation>().HasKey(x => x.Id);
            modelBuilder.Entity<ProjectInvitation>().Property(t => t.Id).ValueGeneratedOnAdd();
            // constraint
            modelBuilder.Entity<ProjectInvitation>().HasIndex(t => new { t.InviteeId, t.InviterId, t.ProjectId }).IsUnique();
            // fks
            modelBuilder.Entity<ProjectInvitation>()
                .HasOne(pi => pi.Invitee)
                .WithMany(iv => iv.ProjectInvitationSent)
                .HasForeignKey(pi => pi.InviteeId)
                .HasConstraintName("invitee-projectinvitation")
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProjectInvitation>()
                .HasOne(pi => pi.Inviter)
                .WithMany(iv => iv.ProjectInvitedTo)
                .HasForeignKey(pi => pi.InviterId)
                .HasConstraintName("inviter-projectinvitation")
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProjectInvitation>()
                .HasOne(pi => pi.Project)
                .WithMany(p => p.ProjectInvitations)
                .HasForeignKey(pi => pi.ProjectId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
            //==============================================================================
            // pk
            modelBuilder.Entity<Sprint>().HasKey(x => x.Id);
            modelBuilder.Entity<Sprint>().Property(x => x.Id).ValueGeneratedOnAdd();
            //fks
            modelBuilder.Entity<Sprint>()
                .HasOne(x => x.Project)
                .WithMany(x => x.Sprints)
                .HasForeignKey(x => x.ProjectId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
            // constraints
            modelBuilder.Entity<Sprint>()
                .Property(s => s.Title)
                .IsRequired();
            //==============================================================================
            // pk
            modelBuilder.Entity<Issue>().HasKey(x => x.Id);
            modelBuilder.Entity<Issue>().Property(x => x.Id).ValueGeneratedOnAdd();
            // fks
            modelBuilder.Entity<Issue>()
                .HasOne(x => x.Project)
                .WithMany(x => x.Issues)
                .HasForeignKey(x => x.ProjectId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Issue>()
                .HasOne(x => x.Reporter)
                .WithMany(x => x.IssueReporter)
                .HasForeignKey(x => x.ReporterId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Issue>()
                .HasOne(x => x.AssignedTo)
                .WithMany(x => x.IssueAssigned)
                .HasForeignKey(x => x.AssignedToId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Issue>()
                .HasOne(x => x.Sprint)
                .WithMany(x => x.Issues)
                .HasForeignKey(x => x.SprintId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Issue>()
                .HasOne(x => x.Epic)
                .WithMany(x => x.Issues)
                .HasForeignKey(x => x.EpicId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
            // constraints
            modelBuilder.Entity<Issue>()
                .Property(i => i.Title)
                .IsRequired();
            modelBuilder.Entity<Issue>()
                .Property(i => i.Type)
                .IsRequired();
            modelBuilder.Entity<Issue>()
                .Property(i => i.Status)
                .IsRequired();
            //==============================================================================
            // pk
            modelBuilder.Entity<Epic>().HasKey(t => t.Id);
            modelBuilder.Entity<Epic>().Property(t => t.Id).ValueGeneratedOnAdd();
            // fks
            modelBuilder.Entity<Epic>()
                .HasOne(x => x.Project)
                .WithMany(x => x.Epics)
                .HasForeignKey(x => x.ProjectId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
            // constraints
            modelBuilder.Entity<Epic>()
                .Property(e => e.Title)
                .IsRequired();
            modelBuilder.Entity<Epic>()
                .Property(e => e.StartDate)
                .IsRequired();
            modelBuilder.Entity<Epic>()
                .Property(e => e.EndDate)
                .IsRequired();
            modelBuilder.Entity<Epic>()
                .Property(e => e.Color)
                .IsRequired();
            modelBuilder.Entity<Epic>()
                .Property(e => e.Row)
                .IsRequired();
            //==============================================================================
            // pk
            modelBuilder.Entity<Path>().HasKey(t => t.Id);
            modelBuilder.Entity<Path>().Property(t => t.Id).ValueGeneratedOnAdd();
            // fks
            modelBuilder.Entity<Path>()
                   .HasOne(x => x.Project)
                   .WithMany(x => x.Paths)
                   .HasForeignKey(x => x.ProjectId)
                   .IsRequired(true)
                   .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Path>()
                   .HasOne(x => x.FromEpic)
                   .WithMany(x => x.PathsFrom)
                   .HasForeignKey(x => x.FromEpicId)
                   .IsRequired(true)
                   .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Path>()
                .HasOne(x => x.ToEpic)
                .WithMany(x => x.PathsTo)
                .HasForeignKey(x => x.ToEpicId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
