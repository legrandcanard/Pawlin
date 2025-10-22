using Pawlin.Common.Entities;

namespace Pawlin.Common
{
    public interface IFlashcardReviewer
    {
        ReviewDataItem Review(ReviewDataItem prevReviewData, int quality);
    }

    public class FlashcardReviewer : IFlashcardReviewer
    {
        private const double minEf = 1.3d;

        /// <summary>
        /// Interval i(1) in days
        /// </summary>
        private const int i1 = 1;

        /// <summary>
        /// Interval i(2) in days
        /// </summary>
        private const int i2 = 6;

        public ReviewDataItem Review(ReviewDataItem prevReviewData, int quality)
        {
            if (quality < 0 || quality > 5)
                throw new ArgumentOutOfRangeException(nameof(quality));

            var lastReviewDate = DateTime.UtcNow;

            var (n, ef, i) = CalculateSm2(quality, prevReviewData.Repeats, prevReviewData.EasinessFactor, prevReviewData.InvervalDays);

            var newReviewData = new ReviewDataItem
            {
                Repeats = n,
                EasinessFactor = ef,
                InvervalDays = i,
                Quality = quality,
                ReviewDateUtc = lastReviewDate,
                NextReviewDateUtc = lastReviewDate.AddDays(i),
                FlashcardId = prevReviewData.FlashcardId,
                UserId = prevReviewData.UserId,
                DeckInstanceId = prevReviewData.DeckInstanceId
            };

            return newReviewData;
        }

        private static (int n, double ef, int i) CalculateSm2(int q, int n, double ef, int i)
        {
            if (q >= 3)
            {
                if (n == 0)
                    i = i1;
                else if (n == 1)
                    i = i2;
                else
                    i = (int)Math.Round(i * ef);

                n++;
            }
            else
            {
                n = 0;
                i = i1;
            }

            ef = ef + (0.1 - (5 - q) * (0.08 + (5 - q) * 0.02));
            if (ef < minEf)
                ef = minEf;

            return (n, ef, i);
        }
    }
}
