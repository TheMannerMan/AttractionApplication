using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Seido.Utilities.SeedGenerator;

namespace Models
{
    public class csAttraction : IAttraction, ISeed<csAttraction>
    {

        // TODO: att to seeder and edit implementation in Seed()
        private List<string> attractionCategories = new List<string>(){
            "Museum",
            "Park",
            "Historical Site",
            "Beach",
            "Amusement Park",
            "Zoo",
            "Aquarium",
            "Monument",
            "Botanical Garden",
            "Art Gallery"
        };

        public virtual Guid AttractionId { get; set; } = Guid.NewGuid();
        public string AttractionName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public virtual ILocation Location { get; set; }
        public virtual List<IReview> Reviews { get; set; }

        // TODO: Add when Location is implemented.
        // public virtual ILocation Location {get; set;}

        public bool Seeded { get; set; } = false;

        public virtual csAttraction Seed(csSeedGenerator _seeder)
        {
            Seeded = true;
            AttractionName = $"{_seeder.LastName} Attraction";
            Description = _seeder.LatinSentence;
            Category = _seeder.FromList<string>(attractionCategories); // TODO: refactor this part into seeder later
            return this;
        }
    }
}