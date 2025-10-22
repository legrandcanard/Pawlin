using Microsoft.EntityFrameworkCore;
using Pawlin.Common.Entities;
using Pawlin.Common.Repositories;

namespace Pawlin.Data.Repositories
{
    public class DeckRepository(ApplicationDbContext dbContext) : IDeckRepository
    {
        public async Task<Deck> AddDeck(Deck deck)
        {
            await dbContext.Decks.AddAsync(deck);
            await dbContext.SaveChangesAsync();
            return deck;
        }

        public async Task<Deck> GetDeck(int deckId)
        {
            var deck = await dbContext.Decks
                .Include(d => d.Flashcards)
                .FirstOrDefaultAsync(d => d.Id == deckId);

            if (deck is null)
                throw new KeyNotFoundException($"Deck with id {deckId} was not found.");

            return deck;
        }

        public async Task<Deck[]> GetDecksByUserId(int userId)
        {
            var decks = await dbContext.Decks
                .Include(d => d.Flashcards)
                .Where(d => d.CreatorUserId == userId)
                .ToArrayAsync();
            return decks;
        }

        public async Task UpdateDeck(Deck deck)
        {
            dbContext.Decks.Update(deck);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteDeck(int deckId)
        {
            var deck = await dbContext.Decks.FindAsync(deckId);
            if (deck is null)
                throw new KeyNotFoundException($"Deck with id {deckId} was not found.");

            dbContext.Decks.Remove(deck);
            await dbContext.SaveChangesAsync();
        }

        public async Task AddDeckInstance(DeckInstance deckInstance)
        {
            await dbContext.DeckInstances.AddAsync(deckInstance);
            await dbContext.SaveChangesAsync();
        }

        public async Task<DeckInstance> GetDeckInstance(int deckInstanceId)
        {
            var instance = await dbContext.DeckInstances
                .Include(di => di.Deck)
                    .ThenInclude(d => d.Flashcards)
                .Include(di => di.ReviewHistory)
                    .ThenInclude(r => r.Flashcard)
                .Include(di => di.User)
                .FirstOrDefaultAsync(di => di.Id == deckInstanceId);

            if (instance is null)
                throw new KeyNotFoundException($"DeckInstance with id {deckInstanceId} was not found.");

            return instance;
        }

        public async Task<DeckInstance[]> GetDeckInstancesByUserId(int userId)
        {
            var instances = await dbContext.DeckInstances
                .Include(di => di.Deck)
                    .ThenInclude(d => d.Flashcards)
                .Include(di => di.ReviewHistory)
                    .ThenInclude(r => r.Flashcard)
                .Include(di => di.User)
                .Where(di => di.UserId == userId)
                .ToArrayAsync();
            return instances;
        }

        public async Task UpdateDeckInstance(DeckInstance deckInstance)
        {
            dbContext.DeckInstances.Update(deckInstance);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteDeckInstance(int deckInstanceId)
        {
            var instance = await dbContext.DeckInstances.FindAsync(deckInstanceId);
            if (instance is null)
                throw new KeyNotFoundException($"DeckInstance with id {deckInstanceId} was not found.");

            dbContext.DeckInstances.Remove(instance);
            await dbContext.SaveChangesAsync();
        }
    }
}
