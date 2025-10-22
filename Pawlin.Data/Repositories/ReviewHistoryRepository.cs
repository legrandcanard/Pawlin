
using Microsoft.EntityFrameworkCore;
using Pawlin.Common.Entities;
using Pawlin.Common.Repositories;

namespace Pawlin.Data.Repositories
{
    public class ReviewHistoryRepository(ApplicationDbContext dbContext) : IReviewHistoryRepository
    {
        public async Task AddReviewHistoryItem(ReviewDataItem reviewDataHistoryItem)
        {
            await dbContext.ReviewDataItems.AddAsync(reviewDataHistoryItem);
            await dbContext.SaveChangesAsync();
        }

        public Task<ReviewDataItem[]> GetReviewHistory(int flashcardId, int userId)
            => dbContext.ReviewDataItems
                .Where(e => e.FlashcardId == flashcardId && e.UserId == userId)
                .AsNoTracking()
                .ToArrayAsync();

        public Task<ReviewDataItem?> GetNearestScheduledReview(int deckInstanceId)
            => dbContext.ReviewDataItems
                .Include(e => e.Flashcard)
                .Where(e => e.DeckInstanceId == deckInstanceId)
                .OrderBy(e => e.NextReviewDateUtc)
                .AsNoTracking()
                .FirstOrDefaultAsync();
    }
}
