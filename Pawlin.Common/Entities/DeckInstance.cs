namespace Pawlin.Common.Entities
{
    public class DeckInstance
    {
        public int Id { get; set; }
        public int DeckId { get; set; }
        public int UserId { get; set; }

        public Deck? Deck { get; set; }
        public User? User { get; set; }

        public virtual ICollection<ReviewDataItem>? ReviewHistory { get; set; }
    }
}
