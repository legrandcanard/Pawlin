using Microsoft.EntityFrameworkCore;
using Pawlin.Common.Entities;
using Pawlin.Common.Repositories;

namespace Pawlin.Data.Repositories
{
    public class FlashcardRepository(ApplicationDbContext dbContext) : IFlashcardRepository
    {
        public Task<Flashcard?> GetByIdAsync(int id)
            => dbContext.Flashcards
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

        public Task<Flashcard[]> GetAllAsync()
            => dbContext.Flashcards
                .AsNoTracking()
                .ToArrayAsync();

        public Task<Flashcard[]> GetByDeckIdAsync(int deckId)
            => dbContext.Flashcards
                .Where(f => f.DeckId == deckId)
                .AsNoTracking()
                .ToArrayAsync();

        public async Task AddAsync(Flashcard flashcard)
        {
            await dbContext.Flashcards.AddAsync(flashcard);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Flashcard flashcard)
        {
            dbContext.Flashcards.Update(flashcard);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await dbContext.Flashcards.FindAsync(id);
            if (entity is null)
                return;

            dbContext.Flashcards.Remove(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}