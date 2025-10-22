namespace Pawlin.WebApi.Dto
{
    public class FlashcardCreateDto
    {
        public int DeckId { get; set; }

        public required string Question { get; set; }
        public required string Answer { get; set; }
    }
}
