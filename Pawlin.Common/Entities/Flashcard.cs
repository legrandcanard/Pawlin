namespace Pawlin.Common.Entities
{
    public class Flashcard
    {
        public int Id { get; set; }
        public int DeckId { get; set; }

        public string Answer { get; set; } = null!;
        public string Question { get; set; } = null!;
    }
}
