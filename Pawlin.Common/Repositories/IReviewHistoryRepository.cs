using Pawlin.Common.Entities;

namespace Pawlin.Common.Repositories
{
    public interface IReviewHistoryRepository
    {
        Task AddReviewHistoryItem(ReviewDataItem reviewDataHistoryItem);
        Task<ReviewDataItem[]> GetReviewHistory(int flashcardId, int userId);
        Task<ReviewDataItem?> GetNearestScheduledReview(int deckInstanceId);
    }
}
