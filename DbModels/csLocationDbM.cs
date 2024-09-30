using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Seido.Utilities.SeedGenerator;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace DbModels
{
    [Index(nameof(City), nameof(Country), nameof(StreetAddress), IsUnique = true)]
    [Index(nameof(City))]
    [Index(nameof(Country))]
    [Index(nameof(City), nameof(Country))]
    public class csLocationDbM : csLocation, ISeed<csLocationDbM>, IEquatable<csLocationDbM>
    {
        [Key]
        public override Guid LocationId { get; set; } = Guid.NewGuid();
        [Required]
        public override string City { get; set; }
        [Required]
        public override string Country { get; set; }
        [Required]
        public override string StreetAddress { get; set; }
        [NotMapped]
        public override List<IAttraction> Attractions { get => AttractionsDbM?.ToList<IAttraction>(); set => new NotImplementedException(); }

        [JsonIgnore]
        public virtual List<csAttractionDbM> AttractionsDbM { get; set; } = null;

        #region implementing IEquatable
/*
        public bool Equals(csLocationDbM other) => (other != null) ? ((this.City, this.Country, this.StreetAddress) ==
            (other.City, other.Country, other.StreetAddress)) : false;

        public override bool Equals(object obj) => Equals(obj as csLocationDbM);
        public override int GetHashCode() => (City, Country, StreetAddress).GetHashCode();
*/

public bool Equals(csLocationDbM other)
{
    if (other == null) return false;
    return string.Equals(City, other.City, StringComparison.OrdinalIgnoreCase) &&
           string.Equals(Country, other.Country, StringComparison.OrdinalIgnoreCase) &&
           string.Equals(StreetAddress, other.StreetAddress, StringComparison.OrdinalIgnoreCase);
}

public override bool Equals(object obj) => Equals(obj as csLocationDbM);

public override int GetHashCode() => 
    (City?.ToLower(), Country?.ToLower(), StreetAddress?.ToLower()).GetHashCode();
        #endregion

        public override csLocationDbM Seed(csSeedGenerator _seeder)
        {
            base.Seed(_seeder);
            return this;
        }
    }
}