using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Seido.Utilities.SeedGenerator;

namespace Models
{
    public class csReview : IReview, ISeed<csReview>
    {
        public virtual Guid ReviewId { get; set; } = Guid.NewGuid();
        public virtual string Comment { get; set; }
        public virtual IUser User { get; set; }
        public virtual IAttraction Attraction { get; set; }
        public bool Seeded { get; set; } = false;

        public virtual csReview Seed(csSeedGenerator _seeder)
        {
            this.Seeded = true;
            this.Comment = _seeder.LatinSentence;
            return this;
        }
    }
}