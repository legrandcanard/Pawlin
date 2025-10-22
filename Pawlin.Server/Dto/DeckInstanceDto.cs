namespace Pawlin.WebApi.Dto
{
    public class DeckInstanceDto
    {
        public int Id { get; set; }
        public int DeckId { get; set; }
        public int UserId { get; set; }

        public DeckDto? Deck { get; set; }

        public ReviewDataItemDto[]? ReviewHistory { get; set; }
    }
}