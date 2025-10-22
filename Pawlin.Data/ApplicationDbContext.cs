using Microsoft.EntityFrameworkCore;
using Pawlin.Common.Entities;

namespace Pawlin.Data
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<Flashcard> Flashcards { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Deck> Decks { get; set; }
        public virtual DbSet<DeckInstance> DeckInstances { get; set; }
        public virtual DbSet<ReviewDataItem> ReviewDataItems { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Users
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("Users");
                e.HasKey(u => u.Id);
                e.Property(u => u.Name).HasMaxLength(200).IsRequired(false);

                e.HasMany(u => u.DeckInstances)
                    .WithOne(di => di.User)
                    .HasForeignKey(di => di.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Decks
            modelBuilder.Entity<Deck>(e =>
            {
                e.ToTable("Decks");
                e.HasKey(d => d.Id);
                e.Property(d => d.Title).IsRequired();

                e.HasMany(d => d.Flashcards)
                    .WithOne()
                    .HasForeignKey(f => f.DeckId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(d => d.CreatorUserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(d => d.CreatorUserId);
            });

            // Flashcards
            modelBuilder.Entity<Flashcard>(e =>
            {
                e.ToTable("Flashcards");
                e.HasKey(f => f.Id);
                e.Property(f => f.Question).IsRequired();
                e.Property(f => f.Answer).IsRequired();

                e.HasIndex(f => f.DeckId);
            });

            // DeckInstances
            modelBuilder.Entity<DeckInstance>(e =>
            {
                e.ToTable("DeckInstances");
                e.HasKey(di => di.Id);

                e.HasOne(di => di.Deck)
                    .WithMany()
                    .HasForeignKey(di => di.DeckId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(di => di.User)
                    .WithMany(u => u.DeckInstances)
                    .HasForeignKey(di => di.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(di => di.ReviewHistory)
                    .WithOne(r => r.DeckInstance)
                    .HasForeignKey(r => r.DeckInstanceId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(di => di.DeckId);
                e.HasIndex(di => di.UserId);
            });

            // ReviewDataItems
            modelBuilder.Entity<ReviewDataItem>(e =>
            {
                e.ToTable("ReviewDataItems");
                e.HasKey(r => r.Id);

                e.Property(r => r.Repeats).HasDefaultValue(0);
                e.Property(r => r.EasinessFactor).HasDefaultValue(2.5d);
                e.Property(r => r.InvervalDays).HasDefaultValue(0);
                e.Property(r => r.Quality).HasDefaultValue(0);

                e.Property(r => r.NextReviewDateUtc).IsRequired();
                e.Property(r => r.ReviewDateUtc).IsRequired();

                e.HasOne(r => r.Flashcard)
                    .WithMany()
                    .HasForeignKey(r => r.FlashcardId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(r => r.User)
                    .WithMany()
                    .HasForeignKey(r => r.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(r => r.DeckInstance)
                    .WithMany(di => di.ReviewHistory)
                    .HasForeignKey(r => r.DeckInstanceId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(r => r.FlashcardId);
                e.HasIndex(r => r.UserId);
                e.HasIndex(r => r.DeckInstanceId);
                e.HasIndex(r => new { r.DeckInstanceId, r.NextReviewDateUtc });
            });
        }
    }
}
