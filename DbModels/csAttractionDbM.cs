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
        public override Guid AttractionId { get;  set; } = Guid.NewGuid();

        [NotMapped]
        public override List<IReview> Reviews { get => ReviewDbMs?.ToList<IReview>(); set => throw new NotImplementedException(); }

        [JsonIgnore]
        public virtual List<csReviewDbM> ReviewDbMs { get; set; } = null;

        csAttractionDbM ISeed<csAttractionDbM>.Seed(csSeedGenerator _seeder)
        {
            base.Seed(_seeder);
            return this;
        }
    }
}