
using Pawlin.Common.Entities;

namespace Pawlin.Common.Repositories
{
    public interface IDeckRepository
    {
        // Deck
        Task<Deck> AddDeck(Deck deck);
        Task<Deck> GetDeck(int deckId);
        Task<Deck[]> GetDecksByUserId(int userId);
        Task UpdateDeck(Deck deck);
        Task DeleteDeck(int deckId);

        // Deck instance
        Task AddDeckInstance(DeckInstance deckInstance);
        Task<DeckInstance> GetDeckInstance(int deckInstanceId);
        Task<DeckInstance[]> GetDeckInstancesByUserId(int userId);
        Task UpdateDeckInstance(DeckInstance deckInstance);
        Task DeleteDeckInstance(int deckInstanceId);
    }
}
