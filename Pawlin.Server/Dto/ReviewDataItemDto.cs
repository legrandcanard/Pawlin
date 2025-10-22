
namespace Pawlin.Server.Dto
{
    public class ReviewDataItemDto
    {
        public int Id { get; set; }
        public int FlashcardId { get; set; }
        public int UserId { get; set; }

        public int Repeats { get; set; }
        public double EasinessFactor { get; set; }
        public int InvervalDays { get; set; }
        public int Quality { get; set; }

        public DateTime NextReviewDateUtc { get; set; }
        public DateTime ReviewDateUtc { get; set; }
    }
}