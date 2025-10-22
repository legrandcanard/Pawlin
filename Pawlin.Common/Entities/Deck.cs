using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pawlin.Common.Entities
{
    public class Deck
    {
        public int Id { get; set; }
        public int CreatorUserId { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Flashcard>? Flashcards { get; set; }
    }
}
