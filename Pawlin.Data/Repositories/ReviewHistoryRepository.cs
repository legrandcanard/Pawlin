
using Microsoft.EntityFrameworkCore;
using Pawlin.Common.Entities;
using Pawlin.Common.Repositories;
using System.Collections.Immutable;

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

        public async Task<ReviewDataItem?> GetNearestScheduledReview(int deckInstanceId)
        {
            var items = await dbContext.ReviewDataItems
                .Include(e => e.Flashcard)
                .Where(e => e.DeckInstanceId == deckInstanceId)
                .GroupBy(e => e.FlashcardId, (id, reviewDataItems) => reviewDataItems.OrderByDescending(e => e.ReviewDateUtc).FirstOrDefault())
                .ToArrayAsync();

            DateTime nearestDate = DateTime.MaxValue;
            ReviewDataItem? nearestReview = null;
            foreach (var item in items)
            {
                if (item == null)
                    continue;

                if (item.NextReviewDateUtc < nearestDate)
                {
                    nearestDate = item.NextReviewDateUtc;
                    nearestReview = item;
                }
            }

            return nearestReview;
        }
    }
}
