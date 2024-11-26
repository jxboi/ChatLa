using Microsoft.EntityFrameworkCore;
using chatlaapp.Backend.Models;

namespace chatlaapp.Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Follow> Follows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Username);
                entity.Property(e => e.Username).IsRequired();
            });

            // Post configuration
            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.PostId);
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.ImageUrl).IsRequired(false);
                entity.Property(e => e.Timestamp).IsRequired();
                entity.Property(e => e.Username).IsRequired();

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(p => p.Username)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Comment configuration
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.CommentId);
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.Timestamp).IsRequired();
                entity.Property(e => e.Username).IsRequired();

                entity.HasOne<Post>()
                    .WithMany()
                    .HasForeignKey(c => c.PostId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(c => c.Username)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Like configuration
            modelBuilder.Entity<Like>(entity =>
            {
                entity.HasKey(e => e.LikeId);
                entity.Property(e => e.Username).IsRequired();

                entity.HasOne<Post>()
                    .WithMany()
                    .HasForeignKey(l => l.PostId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(l => l.Username)
                    .OnDelete(DeleteBehavior.Cascade);

                // Ensure a user can only like a post once
                entity.HasIndex(l => new { l.PostId, l.Username }).IsUnique();
            });

            // Follow configuration
            modelBuilder.Entity<Follow>(entity =>
            {
                entity.HasKey(e => new { e.FollowerUsername, e.FollowingUsername });

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(f => f.FollowerUsername)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(f => f.FollowingUsername)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
} 