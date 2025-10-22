using Pawlin.Common.Entities;

namespace Pawlin.Common.Dtos
{
    public class FlashcardDto
    {
        public int Id { get; set; }
        public int DeckId { get; set; }

        public required string Question { get; set; }
        public required string Answer { get; set; }
    }
}