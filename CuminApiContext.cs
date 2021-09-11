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
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Sprint> Sprints { get; set; }
        public DbSet<Roadmap> Roadmaps { get; set; }
        public DbSet<Epic> Epics { get; set; }
        public DbSet<RoadmapEpic> RoadmapEpics { get; set; }
        public DbSet<Path> Paths { get; set; }
        public DbSet<RoadmapPath> RoadmapPaths { get; set; }


        public CuminApiContext(DbContextOptions<CuminApiContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
        // why not just add a ? activeSprintId optinal field in the "Project" entity??
        //  A project can have only one active sprint
            //modelBuilder.Entity<ActiveSprintProject>().HasKey(x => new { x.ProjectId});
            //modelBuilder.Entity<ActiveSprintProject>().HasAlternateKey(x => x.Id);
            //modelBuilder.Entity<ActiveSprintProject>().Property(x => x.Id).ValueGeneratedOnAdd();
            //modelBuilder.Entity<ActiveSprintProject>()
            //    .HasOne(x => x.Project)
            //    .WithOne(x => x.ActiveSprint)
            //    .HasForeignKey<ActiveSprintProject>(x => x.ProjectId);
            //modelBuilder.Entity<ActiveSprintProject>()
            //    .HasOne(x => x.Sprint)
            //    .WithOne(x => x.ActiveSprint)
            //    .HasForeignKey<ActiveSprintProject>(x => x.SprintId);




            modelBuilder.Entity<Sprint>().HasKey(x => x.Id);
            modelBuilder.Entity<Sprint>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Sprint>()
                .HasOne(x => x.Project)
                .WithMany(x => x.Sprints)
                .HasForeignKey(x => x.ProjectId)
                .IsRequired(true);
            

            modelBuilder.Entity<Issue>().HasKey(x => x.Id);
            modelBuilder.Entity<Issue>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Issue>()
                .HasOne(x => x.Project)
                .WithMany(x => x.Issues)
                .HasForeignKey(x => x.ProjectId);

            modelBuilder.Entity<Issue>()
                .HasOne(x => x.Reporter)
                .WithMany(x => x.IssueReporter)
                .HasForeignKey(x => x.ReporterId);

            modelBuilder.Entity<Issue>()
                .HasOne(x => x.Resolver)
                .WithMany(x => x.IssueResolver)
                .HasForeignKey(x => x.ResolverId)
                .IsRequired(false);

            modelBuilder.Entity<Issue>()
                .HasOne(x => x.Sprint)
                .WithMany(x => x.Issues)
                .HasForeignKey(x => x.SprintId)
                .IsRequired(false);

            modelBuilder.Entity<Issue>()
            .HasOne(x => x.Epic)
            .WithMany(x => x.Issues)
            .HasForeignKey(x => x.EpicId)
            .IsRequired(false);


            modelBuilder.Entity<User>().HasAlternateKey(x => x.Username);

            modelBuilder.Entity<UserProject>().HasKey(t => new { t.UserId, t.ProjectId });
            modelBuilder.Entity<UserProject>().HasAlternateKey(x => x.Id);
            modelBuilder.Entity<UserProject>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<UserProject>().Property(u => u.UserRole).IsRequired();

            modelBuilder.Entity<UserProject>()
                .HasOne(up => up.Project)
                .WithMany(p => p.UserProjects)
                .HasForeignKey(up => up.ProjectId);

            modelBuilder.Entity<UserProject>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserProjects)
                .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<ProjectInvitation>().HasKey(t => new { t.InviteeId, t.InviterId, t.ProjectId });
            modelBuilder.Entity<ProjectInvitation>().HasAlternateKey(x => x.Id);
            modelBuilder.Entity<ProjectInvitation>().Property(t => t.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<ProjectInvitation>()
                .HasOne(pi => pi.Invitee)
                .WithMany(iv => iv.ProjectInvitationSent)
                .HasForeignKey(pi => pi.InviteeId)
                .HasConstraintName("invitee-projectinvitation");

            modelBuilder.Entity<ProjectInvitation>()
                .HasOne(pi => pi.Inviter)
                .WithMany(iv => iv.ProjectInvitedTo)
                .HasForeignKey(pi => pi.InviterId)
                .HasConstraintName("inviter-projectinvitation");

            modelBuilder.Entity<ProjectInvitation>()
                .HasOne(pi => pi.Project)
                .WithMany(p => p.ProjectInvitations)
                .HasForeignKey(pi => pi.ProjectId);

             modelBuilder.Entity<Project>()
                .HasOne(x => x.ActiveSprint)
                .WithOne(x => x.ActiveForProject)
                .HasForeignKey<Project>(x => x.ActiveSprintId)
                .IsRequired(false);

            modelBuilder.Entity<User>()
                .HasOne(x => x.ActiveProject)
                .WithMany(x => x.ActiveForUser)
                .HasForeignKey(x => x.ActiveProjectId)
                .IsRequired(false);

            modelBuilder.Entity<Epic>()
                .HasOne(x => x.Project)
                .WithMany(x => x.Epics)
                .HasForeignKey(x => x.ProjectId)
                .IsRequired(true);

            modelBuilder.Entity<Epic>()
            .HasMany(x => x.Issues)
            .WithOne(x => x.Epic);

            modelBuilder.Entity<Epic>()
                .HasAlternateKey(x => new { x.ProjectId, x.Row });

            modelBuilder.Entity<Path>()
               .HasOne(x => x.FromEpic)
               .WithMany(x => x.PathsFrom)
               .HasForeignKey(x => x.FromEpicId)
               .IsRequired(true);

            modelBuilder.Entity<Path>()
                .HasOne(x => x.ToEpic)
                .WithMany(x => x.PathsTo)
                .HasForeignKey(x => x.ToEpicId)
                .IsRequired(true);

            modelBuilder.Entity<RoadmapEpic>()
                .HasKey(t => new { t.EpicId, t.RoadmapId });

            modelBuilder.Entity<RoadmapEpic>()
                .HasOne(t => t.Epic)
                .WithMany(t => t.RoadmapEpics)
                .HasForeignKey(t => t.EpicId);

            modelBuilder.Entity<RoadmapEpic>()
                .HasOne(t => t.Roadmap)
                .WithMany(t => t.RoadmapEpics)
                .HasForeignKey(t => t.RoadmapId);

            modelBuilder.Entity<RoadmapPath>()
            .HasKey(t => new { t.PathId, t.RoadmapId });

            modelBuilder.Entity<RoadmapPath>()
                .HasOne(t => t.Path)
                .WithMany(t => t.RoadmapPaths)
                .HasForeignKey(t => t.PathId);

            modelBuilder.Entity<RoadmapPath>()
                .HasOne(t => t.Roadmap)
                .WithMany(t => t.RoadmapPaths)
                .HasForeignKey(t => t.RoadmapId);

            modelBuilder.Entity<Roadmap>()
                .HasOne(t => t.Creator)
                .WithOne(t => t.Roadmap)
                .HasForeignKey<Roadmap>(t => t.CreatorId);
        }
    }
}
