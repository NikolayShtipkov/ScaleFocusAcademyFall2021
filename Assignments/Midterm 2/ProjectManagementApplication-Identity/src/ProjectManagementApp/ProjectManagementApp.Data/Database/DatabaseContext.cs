using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectManagementApp.Data.Database
{
    public class DatabaseContext : DbContext
    {
        private static string _connectionString;

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DatabaseContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<TaskProj> Tasks { get; set; }

        public DbSet<WorkLog> WorkLogs { get; set; }

        public DbSet<TeamUser> TeamUser { get; set; }

        public DbSet<ProjectTeam> ProjectTeam { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_connectionString != null)
            {
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer(_connectionString);
            }
            
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskProj>().HasOne(t => t.Owner).WithMany(u => u.OwnedTasks).HasForeignKey(t => t.OwnerId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<TaskProj>().HasOne(t => t.Assignee).WithMany(u => u.AssignedTasks).HasForeignKey(t => t.AssigneeId).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<TaskProj>().HasOne(t => t.Project).WithMany(p => p.Tasks).HasForeignKey(t => t.ProjectId).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<WorkLog>().HasOne(w => w.Task).WithMany(t => t.WorkLogs).HasForeignKey(t => t.TaskId).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<TeamUser>().HasKey(tu => new { tu.TeamId, tu.UserId });
            modelBuilder.Entity<ProjectTeam>().HasKey(pt => new { pt.ProjectId, pt.TeamId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
