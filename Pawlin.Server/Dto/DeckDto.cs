
namespace Pawlin.Server.Dto
{
    public class DeckDto
    {
        public int Id { get; set; }
        public int CreatorUserId { get; set; }
        public string Title { get; set; }

        public FlashcardDto[]? Flashcards { get; set; }
    }
}