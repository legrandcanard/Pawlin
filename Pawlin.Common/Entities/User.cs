namespace Pawlin.Common.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<DeckInstance>? DeckInstances { get; set; }
    }
}
