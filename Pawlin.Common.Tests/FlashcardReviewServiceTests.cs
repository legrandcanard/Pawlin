using Moq;
using Pawlin.Common.Entities;
using Pawlin.Common.Repositories;
using Pawlin.Common.Services;

namespace Pawlin.Common.Tests
{
    public class FlashcardReviewServiceTests
    {
        Mock<IFlashcardReviewer> flashcardReviewerMock = null!;
        Mock<IReviewHistoryRepository> reviewHistoryRepositoryMock = null!;
        FlashcardReviewService service = null!;

        [SetUp]
        public void Setup()
        {
            flashcardReviewerMock = new Mock<IFlashcardReviewer>();
            reviewHistoryRepositoryMock = new Mock<IReviewHistoryRepository>();

            service = new FlashcardReviewService(
                flashcardReviewerMock.Object,
                reviewHistoryRepositoryMock.Object);
        }

        [Test]
        public async Task Review_WithEmptyHistory_CallsReviewerWithDefaultPrevAndAddsReturnedItem()
        {
            // Arrange
            var deckInstance = new DeckInstance { UserId = 5 };
            var flashcard = new Flashcard { Id = 10 };

            reviewHistoryRepositoryMock
                .Setup(r => r.GetReviewHistory(flashcard.Id, deckInstance.UserId))
                .ReturnsAsync(Array.Empty<ReviewDataItem>());

            var returned = new ReviewDataItem { Id = 99, FlashcardId = flashcard.Id, UserId = deckInstance.UserId };
            flashcardReviewerMock
                .Setup(fr => fr.Review(It.IsAny<ReviewDataItem>(), 3))
                .Returns(returned);

            ReviewDataItem? added = null;
            reviewHistoryRepositoryMock
                .Setup(r => r.AddReviewHistoryItem(It.IsAny<ReviewDataItem>()))
                .Callback<ReviewDataItem>(r => added = r)
                .Returns(Task.CompletedTask);

            // Act
            await service.Review(deckInstance, flashcard, quality: 3);

            // Assert
            reviewHistoryRepositoryMock.Verify(r => r.GetReviewHistory(flashcard.Id, deckInstance.UserId), Times.Once);
            flashcardReviewerMock.Verify(fr => fr.Review(It.Is<ReviewDataItem>(rd => rd != null && rd.Id == 0), 3), Times.Once);
            reviewHistoryRepositoryMock.Verify(r => r.AddReviewHistoryItem(It.IsAny<ReviewDataItem>()), Times.Once);

            Assert.That(added, Does.Not.Null);
            Assert.That(returned, Is.EqualTo(added));
        }

        [Test]
        public async Task Review_WithExistingHistory_PassesLastHistoryItemToReviewerAndAddsResult()
        {
            // Arrange
            var deckInstance = new DeckInstance { UserId = 7 };
            var flashcard = new Flashcard { Id = 20 };

            var older = new ReviewDataItem { Id = 1 };
            var last = new ReviewDataItem { Id = 2, FlashcardId = flashcard.Id, UserId = deckInstance.UserId };

            reviewHistoryRepositoryMock
                .Setup(r => r.GetReviewHistory(flashcard.Id, deckInstance.UserId))
                .ReturnsAsync(new[] { older, last });

            var returned = new ReviewDataItem { Id = 77, FlashcardId = flashcard.Id, UserId = deckInstance.UserId };
            flashcardReviewerMock
                .Setup(fr => fr.Review(It.IsAny<ReviewDataItem>(), 5))
                .Returns(returned);

            ReviewDataItem? added = null;
            reviewHistoryRepositoryMock
                .Setup(r => r.AddReviewHistoryItem(It.IsAny<ReviewDataItem>()))
                .Callback<ReviewDataItem>(r => added = r)
                .Returns(Task.CompletedTask);

            // Act
            await service.Review(deckInstance, flashcard, quality: 5);

            // Assert - reviewer should receive the exact last history instance
            flashcardReviewerMock.Verify(fr => fr.Review(It.Is<ReviewDataItem>(rd => object.ReferenceEquals(rd, last)), 5), Times.Once);
            reviewHistoryRepositoryMock.Verify(r => r.AddReviewHistoryItem(It.IsAny<ReviewDataItem>()), Times.Once);
            Assert.That(added, Does.Not.Null);
            Assert.That(returned, Is.EqualTo(added));
        }

        [Test]
        public async Task GetNextFlashcard_NoScheduledReview_ReturnsFirstUnreviewedFlashcard()
        {
            // Arrange
            var f1 = new Flashcard { Id = 1 };
            var f2 = new Flashcard { Id = 2 };
            var deck = new Deck { Flashcards = new List<Flashcard> { f1, f2 } };
            var deckInstance = new DeckInstance
            {
                Id = 11,
                DeckId = 100,
                ReviewHistory = new List<ReviewDataItem> { },
                Deck = deck
            };

            reviewHistoryRepositoryMock
                .Setup(r => r.GetNearestScheduledReview(deckInstance.Id))
                .ReturnsAsync((ReviewDataItem?)null);

            // Act
            var next = await service.GetNextFlashcard(deckInstance);

            // Assert - assert that the returned flashcard is contained in the deck and is the expected one
            Assert.That(deck.Flashcards, Does.Contain(next));
            Assert.That(next, Is.EqualTo(f1));
        }

        [Test]
        public async Task GetNextFlashcard_ScheduledInPast_ReturnsScheduledFlashcard()
        {
            // Arrange
            var f1 = new Flashcard { Id = 1 };
            var deck = new Deck { Flashcards = new List<Flashcard> { f1 } };
            var deckInstance = new DeckInstance
            {
                Id = 12,
                Deck = deck,
                ReviewHistory = new List<ReviewDataItem>()
            };

            var scheduled = new ReviewDataItem
            {
                FlashcardId = f1.Id,
                Flashcard = f1,
                NextReviewDateUtc = DateTime.UtcNow.AddHours(-1)
            };

            reviewHistoryRepositoryMock
                .Setup(r => r.GetNearestScheduledReview(deckInstance.Id))
                .ReturnsAsync(scheduled);

            // Act
            var next = await service.GetNextFlashcard(deckInstance);

            // Assert - equality of the returned single item
            Assert.That(next, Is.EqualTo(f1));
        }

        [Test]
        public async Task GetNextFlashcard_ScheduledInFuture_ReturnsUnreviewedFlashcard()
        {
            // Arrange
            var f1 = new Flashcard { Id = 1 };
            var f2 = new Flashcard { Id = 2 };
            var deck = new Deck { Flashcards = new List<Flashcard> { f1, f2 } };
            var deckInstance = new DeckInstance
            {
                Id = 13,
                Deck = deck,
                ReviewHistory = new List<ReviewDataItem> { new ReviewDataItem { FlashcardId = f1.Id } }
            };

            var scheduled = new ReviewDataItem
            {
                FlashcardId = f1.Id,
                Flashcard = f1,
                NextReviewDateUtc = DateTime.UtcNow.AddHours(1) // in future
            };

            reviewHistoryRepositoryMock
                .Setup(r => r.GetNearestScheduledReview(deckInstance.Id))
                .ReturnsAsync(scheduled);

            // Act
            var next = await service.GetNextFlashcard(deckInstance);

            // Assert - returned flashcard should be in the deck and be the unreviewed one
            Assert.That(deck.Flashcards, Does.Contain(next));
            Assert.That(next, Is.EqualTo(f2));
        }
    }
}