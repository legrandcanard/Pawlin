using Pawlin.Common.Entities;

namespace Pawlin.Common.Repositories
{
    public interface IFlashcardRepository
    {
        Task<Flashcard?> GetByIdAsync(int id);
        Task<Flashcard[]> GetAllAsync();
        Task<Flashcard[]> GetByDeckIdAsync(int deckId);

        Task AddAsync(Flashcard flashcard);
        Task UpdateAsync(Flashcard flashcard);
        Task DeleteAsync(int id);
    }
}
