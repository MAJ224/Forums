using Forum.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum
{
    public class dbContext : DbContext
    {
        public dbContext(DbContextOptions options) : base(options) { }
        public DbSet<Models.Thread> Threads { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasMany(u => u.Threads)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Models.Thread>()
                .HasMany(t => t.Comments)
                .WithOne(c => c.Thread)
                .HasForeignKey(c => c.ThreadId);
        }
    }
}
