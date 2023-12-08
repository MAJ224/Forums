using Forum.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum
{
    public class dbContext : DbContext
    {
        public dbContext(DbContextOptions options) : base(options) { }
        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.discussion)
                .WithMany(d => d.Comments)
                .HasForeignKey(c => c.DiscussionId);

            modelBuilder.Entity<Discussion>()
                .HasOne(d => d.User)
                .WithMany(u => u.Discussions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
