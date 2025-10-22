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

            // Flashcard
            modelBuilder.Entity<Flashcard>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Answer).IsRequired();
                e.Property(x => x.Question).IsRequired();
            });

            // User
            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(x => x.Id);

                e.HasMany<DeckInstance>()
                    .WithOne(di => di.User)
                    .HasForeignKey(di => di.UserId)
                    .IsRequired();
            });

            // Deck
            modelBuilder.Entity<Deck>(e =>
            {
                e.HasKey(x => x.Id);

                e.HasMany(d => d.Flashcards)
                    .WithOne()
                    .HasForeignKey(e => e.DeckId)
                    .IsRequired();

                e.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(d => d.CreatorUserId)
                    .IsRequired();
            });

            // DeckInstance
            modelBuilder.Entity<DeckInstance>(e =>
            {
                e.HasKey(di => di.Id);

                e.HasOne(di => di.Deck)
                    .WithMany()
                    .HasForeignKey(di => di.DeckId)
                    .IsRequired();
            });

            // ReviewDataHistoryItem
            modelBuilder.Entity<ReviewDataItem>(e =>
            {
                e.HasKey(x => x.Id);

                e.HasOne(r => r.Flashcard)
                    .WithMany()
                    .HasForeignKey(r => r.FlashcardId)
                    .IsRequired();

                e.HasOne(r => r.User)
                    .WithMany()
                    .HasForeignKey(r => r.UserId)
                    .IsRequired();

                e.HasOne(r => r.DeckInstance)
                    .WithMany()
                    .HasForeignKey(r => r.DeckInstanceId)
                    .IsRequired();
            });
        }
    }
}
