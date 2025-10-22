namespace Pawlin.Common.Entities
{
    public class ReviewDataItem
    {
        // Keys
        public int Id { get; set; }
        public int FlashcardId { get; set; }
        public int UserId { get; set; }
        public int DeckInstanceId { get; set; }

        // Review related data
        public int Repeats { get; set; } = 0;
        public double EasinessFactor { get; set; } = 2.5d;
        public int InvervalDays { get; set; } = 0;
        public int Quality { get; set; } = 0;

        // Review result
        public DateTime NextReviewDateUtc { get; set; }
        public DateTime ReviewDateUtc { get; set; }

        // Navigation
        public virtual Flashcard? Flashcard { get; set; }
        public virtual User? User { get; set; }
        public virtual DeckInstance? DeckInstance { get; set; }
    }
}
