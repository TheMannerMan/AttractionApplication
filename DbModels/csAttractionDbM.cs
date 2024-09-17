using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Seido.Utilities.SeedGenerator;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace DbModels
{
    public class csAttractionDbM : csAttraction, ISeed<csAttractionDbM>
    {
        [Key]
        public override Guid AttractionId { get; set; } = Guid.NewGuid();

        [NotMapped]
        public override List<IReview> Reviews { get => ReviewsDbM?.ToList<IReview>(); set => new NotImplementedException(); }

        [NotMapped]
        public override ILocation Location { get => LocationDbM; set => new NotImplementedException(); }

        [JsonIgnore]
        public virtual List<csReviewDbM> ReviewsDbM { get; set; } = null;

        [JsonIgnore]
        public virtual csLocationDbM LocationDbM { get; set; } = null;

        csAttractionDbM ISeed<csAttractionDbM>.Seed(csSeedGenerator _seeder)
        {
            base.Seed(_seeder);
            return this;
        }
    }
}