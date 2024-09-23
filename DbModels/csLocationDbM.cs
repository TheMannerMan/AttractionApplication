using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Seido.Utilities.SeedGenerator;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace DbModels
{
    public class csLocationDbM : csLocation, ISeed<csLocationDbM>, IEquatable<csLocationDbM>
    {
        [Key]
        public override Guid LocationId { get; set; } = Guid.NewGuid();
        [NotMapped]
        public override List<IAttraction> Attractions { get => AttractionsDbM?.ToList<IAttraction>(); set => new NotImplementedException(); }

        [JsonIgnore]
        public virtual List<csAttractionDbM> AttractionsDbM { get; set; } = null;

        #region implementing IEquatable

        public bool Equals(csLocationDbM other) => (other != null) ? ((City, Country) ==
            (other.City, other.Country)) : false;

        public override bool Equals(object obj) => Equals(obj as csLocationDbM);
        public override int GetHashCode() => (City, Country).GetHashCode();

        #endregion

        public override csLocationDbM Seed(csSeedGenerator _seeder)
        {
            base.Seed(_seeder);
            return this;
        }
    }
}