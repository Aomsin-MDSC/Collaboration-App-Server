using System.ComponentModel.DataAnnotations;
using CollaborationAppAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CollaborationAppAPI.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Announce> Announces { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // One-to-Many Relationship between User and Projects
            modelBuilder.Entity<Project>()
                .HasOne(p => p.User)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.User_id);

            // One-to-One Relationship between Project and Member
            modelBuilder.Entity<Member>()
                .HasOne(p => p.Project)
                .WithOne(m => m.Member)
                .HasForeignKey<Member>(p => p.Project_id);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Tag)
                .WithOne(m => m.Project)
                .HasForeignKey<Project>(p => p.Tag_id);

            modelBuilder.Entity<Project>()
                .HasMany(t => t.Tasks)
                .WithOne(p => p.Project)
                .HasForeignKey(p => p.Project_id);

            modelBuilder.Entity<Announce>()
                .HasOne(p => p.Project)
                .WithOne(a => a.Announce)
                .HasForeignKey<Announce>(p => p.Project_id);

            modelBuilder.Entity<Comment>()
                .HasOne(u => u.User)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.User_id);

            modelBuilder.Entity<Comment>()
                .HasOne(t => t.Task)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.Task_id);

            modelBuilder.Entity<Task>()
                .HasOne(t => t.Tag)
                .WithOne(tg => tg.Task)
                .HasForeignKey<Task>(t => t.Tag_id);

            modelBuilder.Entity<Task>()
                .HasOne(u => u.User)
                .WithMany(t => t.Tasks)
                .HasForeignKey(u => u.User_id);


            base.OnModelCreating(modelBuilder);
        }

    }







}

