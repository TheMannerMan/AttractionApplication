using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Seido.Utilities.SeedGenerator;

namespace Models
{
    public class csLocation : ILocation, ISeed<csLocation>, IEquatable<csLocation>
    {
        public virtual Guid LocationId { get; set; } = Guid.NewGuid();
        public string City { get; set; }
        public string Country { get; set; }

        public virtual List<IAttraction> Attractions { get; set; }

        public bool Seeded { get; set; } = false;

        #region implementing IEquatable

        public bool Equals(csLocation other) => (other != null) ? ((City, Country) ==
            (other.City, other.Country)) : false;

        public override bool Equals(object obj) => Equals(obj as csLocation);
        public override int GetHashCode() => (City, Country).GetHashCode();

        #endregion

        public virtual csLocation Seed(csSeedGenerator _seeder)
        {
            Seeded = true;
            Country = _seeder.Country;
            City = _seeder.City(Country);

            return this;
        }
    }
}