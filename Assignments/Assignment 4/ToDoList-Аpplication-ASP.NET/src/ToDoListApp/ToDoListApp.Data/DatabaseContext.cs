using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDoListApp.Entities;

namespace ToDoListApp.Data
{
    public class DatabaseContext : DbContext
    {
        private static string _connectionString;

        public DatabaseContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<User> Users { get; set; }

        public DbSet<ToDoList> ToDoLists { get; set; }

        public DbSet<Task> Tasks { get; set; }

        public DbSet<AssignedTask> AssignedTasks { get; set; }

        public DbSet<SharedToDoList> SharedToDoLists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseSqlServer(_connectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetupUserConfiguration(modelBuilder);
            SetupToDoListConfiguration(modelBuilder);
            SetupTaskConfiguration(modelBuilder);
            SetupSharedListConfirguration(modelBuilder);
            SetupAssignedTaskConfirguration(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SetupUserConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Username).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<User>().Property(u => u.Password).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<User>().Property(u => u.FirstName).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<User>().Property(u => u.LastName).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<User>().HasOne(u => u.Creator).WithMany().HasForeignKey(u => u.CreatorId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<User>().HasOne(u => u.Modifier).WithMany().HasForeignKey(u => u.ModifierId).OnDelete(DeleteBehavior.NoAction);
        }

        private void SetupToDoListConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoList>().HasKey(l => l.Id);
            modelBuilder.Entity<ToDoList>().Property(l => l.Title).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<ToDoList>().HasOne(l => l.Creator).WithMany().HasForeignKey(u => u.CreatorId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ToDoList>().HasOne(l => l.Modifier).WithMany().HasForeignKey(u => u.ModifierId).OnDelete(DeleteBehavior.NoAction);
        }

        private void SetupTaskConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>().HasKey(t => t.Id);
            modelBuilder.Entity<Task>().Property(t => t.Title).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Task>().Property(t => t.Description).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<Task>().HasOne(t => t.ToDoList).WithMany(l => l.Tasks).HasForeignKey(t => t.ToDoListId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Task>().HasOne(t => t.Creator).WithMany().HasForeignKey(u => u.CreatorId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Task>().HasOne(t => t.Modifier).WithMany().HasForeignKey(u => u.ModifierId).OnDelete(DeleteBehavior.NoAction);
        }

        private void SetupSharedListConfirguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SharedToDoList>().HasKey(s => s.Id);
            modelBuilder.Entity<SharedToDoList>().HasOne(s => s.User).WithMany().HasForeignKey(s => s.UserId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<SharedToDoList>().HasOne(s => s.ToDoList).WithMany().HasForeignKey(s => s.ListId).OnDelete(DeleteBehavior.NoAction);
        }

        private void SetupAssignedTaskConfirguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssignedTask>().HasKey(t => t.Id);
            modelBuilder.Entity<AssignedTask>().HasOne(t => t.User).WithMany().HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<AssignedTask>().HasOne(t => t.Task).WithMany().HasForeignKey(t => t.TaskId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
