using Pawlin.Common.Entities;
using Pawlin.Common.Repositories;

namespace Pawlin.Common.Services
{
    public interface IFlashcardReviewService
    {
        Task<ReviewDataItem> Review(DeckInstance deckInstance, Flashcard flashcard, int quality);
        Task<Flashcard> GetNextFlashcard(DeckInstance deckInstance);
    }

    public class FlashcardReviewService(
        IFlashcardReviewer flashcardReviewer,
        IReviewHistoryRepository reviewHistoryRepository) : IFlashcardReviewService
    {

        public async Task<ReviewDataItem> Review(DeckInstance deckInstance, Flashcard flashcard, int quality)
        {
            var history = await reviewHistoryRepository.GetReviewHistory(flashcard.Id, deckInstance.UserId);

            var prevReviewData = history.LastOrDefault() ?? new ReviewDataItem 
            { 
                UserId = deckInstance.UserId,
                FlashcardId = flashcard.Id,
                Flashcard = flashcard,
                DeckInstanceId = deckInstance.Id,
                DeckInstance = deckInstance
            };

            var newReviewData = flashcardReviewer.Review(prevReviewData, quality);

            await reviewHistoryRepository.AddReviewHistoryItem(newReviewData);

            return newReviewData;
        }

        public async Task<Flashcard> GetNextFlashcard(DeckInstance deckInstance)
        {
            var reviewData = await reviewHistoryRepository.GetNearestScheduledReview(deckInstance.Id);
            
            if (reviewData is null)
                return GetUnreviewedFlashcard(deckInstance);

            if (reviewData.NextReviewDateUtc > DateTime.UtcNow)
            {
                var unreviewed = GetUnreviewedFlashcard(deckInstance);
                if (unreviewed is null)
                    return reviewData.Flashcard!;
            }

            return reviewData.Flashcard!;
        }

        private static Flashcard GetUnreviewedFlashcard(DeckInstance deckInstance)
        {
            return deckInstance.Deck!.Flashcards
                !.FirstOrDefault(f => !deckInstance.ReviewHistory!.Any(rh => rh.FlashcardId == f.Id))!;
        }
    }
}
