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

        #region Seeding data for attractions
        private List<string> attractionCategories = new List<string>(){
            "Restaurant",
            "Shopping Mall",
            "Museum",
            "Park",
            "Beach",
            "Zoo",
            "Cinema",
            "Amusement Park",
            "Historical Landmark",
            "Concert Venue",
            "Aquarium",
            "Art Gallery",
            "Botanical Garden",
            "Sports Stadium",
            "Theater"
        };

        private string namePrefix = "Harbor, Greenfield, Riverside, Oakwood, Grand, Sunny, Maple, Blue Sky, Golden Gate, Silver Birch, City, Morningstar, Meadow, High Street, Willow";
        private string nameSuffix = "Cafe, Hotel, Diner, Lodge, Grill, House, Bakery, Inn, Bistro, Pub, Tavern, Eatery, Restaurant, Bar, Lounge, Restaurant, Mall, Museum, Park, Beach, Zoo, Cinema, Amusement Park, Landmark, Venue, Aquarium, Gallery, Garden, Stadium, Theater";

        private List<string> descriptions = new List<string>
{
    "A popular destination known for its lively atmosphere.",
    "Perfect for a relaxing day out with friends or family.",
    "A place that offers something for everyone.",
    "Famous for its welcoming ambiance and great service.",
    "Known for its picturesque views and charming location.",
    "A must-visit spot for both locals and tourists.",
    "Offers a unique experience you won’t find anywhere else.",
    "Ideal for spending quality time with loved ones.",
    "A favorite destination for those looking to unwind.",
    "Great for an afternoon escape from the busy city life.",
    "Features an impressive selection of attractions and activities.",
    "Offers a mix of entertainment and relaxation.",
    "Perfect for those looking to explore something new.",
    "A hidden gem that surprises every visitor.",
    "A highly recommended spot for a memorable visit.",
    "Known for its friendly staff and inviting atmosphere.",
    "Offers a perfect blend of tradition and modernity.",
    "A place where every visit feels special.",
    "Perfect for visitors of all ages.",
    "Features a rich history and a vibrant environment.",
    "A well-known local landmark that never disappoints.",
    "A peaceful spot to relax and enjoy the surroundings.",
    "A popular choice for locals and visitors alike.",
    "Known for its beautiful design and unique charm.",
    "A place where culture meets entertainment.",
    "Offers a diverse range of activities for everyone.",
    "A destination that is sure to leave a lasting impression.",
    "A vibrant spot with plenty to see and do.",
    "Renowned for its outstanding atmosphere.",
    "A place where great memories are made.",
    "Offers a perfect balance of fun and relaxation.",
    "A well-maintained location that’s perfect for a day out.",
    "A lively spot with something happening around every corner.",
    "A great place to visit, whether you're just passing through or staying a while.",
    "Perfect for those who enjoy a blend of culture and excitement.",
    "A place that seamlessly blends history with modern attractions.",
    "A peaceful retreat in the middle of the city.",
    "Offers stunning views and a relaxing environment.",
    "Known for its clean and welcoming environment.",
    "A vibrant and energetic location that offers endless fun.",
    "A charming spot that locals are proud of.",
    "A tranquil location with plenty to explore.",
    "Perfect for a casual day out, whether you're exploring or just relaxing.",
    "A well-known landmark that draws visitors from all over.",
    "An enjoyable and enriching experience for visitors of all kinds.",
    "Offers a variety of activities that appeal to a wide audience.",
    "A popular spot with a great atmosphere and friendly staff.",
    "Known for its unique character and inviting atmosphere.",
    "A destination full of surprises at every turn.",
    "A memorable place to visit, no matter the occasion."
};

        #endregion

        public virtual Guid AttractionId { get; set; } = Guid.NewGuid();
        public virtual string AttractionName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public virtual ILocation Location { get; set; }
        public virtual List<IReview> Reviews { get; set; }

        public bool Seeded { get; set; } = false;

        public virtual csAttraction Seed(csSeedGenerator _seeder)
        {
            Seeded = true;
            AttractionName = $"{_seeder.FromString(namePrefix)} {_seeder.FromString(nameSuffix)}";
            Description = _seeder.FromList<string>(descriptions);
            Category = _seeder.FromList<string>(attractionCategories); // TODO: refactor this part into seeder later
            return this;
        }
    }
}