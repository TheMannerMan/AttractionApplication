using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Seido.Utilities.SeedGenerator;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Text.Json.Serialization;
using Newtonsoft.Json; // la till denna TEST
using Microsoft.EntityFrameworkCore; // la till denna TEST
using Microsoft.AspNetCore.Mvc;
using Models.DTO;

namespace DbModels
{
    public class csAttractionDbM : csAttraction, ISeed<csAttractionDbM>
    {
        [Key]
        public override Guid AttractionId { get; set; } = Guid.NewGuid();

        [Required]
        public override string AttractionName { get; set; }

        [NotMapped]
        public override List<IReview> Reviews { get => ReviewsDbM?.ToList<IReview>(); set => new NotImplementedException(); }
        [JsonIgnore]
        public virtual List<csReviewDbM> ReviewsDbM { get; set; } = null;

        [NotMapped]
        public override ILocation Location { get => LocationDbM; set => new NotImplementedException(); }
        [JsonIgnore]
        [ForeignKey("LocationId")]
        public virtual csLocationDbM LocationDbM { get; set; } = null;
        [JsonIgnore]
        public Guid? LocationId { get; set; }  // Change this to Guid? instead of int?

        csAttractionDbM ISeed<csAttractionDbM>.Seed(csSeedGenerator _seeder)
        {
            base.Seed(_seeder);
            return this;
        }

        #region Update from DTO
        public csAttractionDbM UpdateFromDTO(csAttractionCUdto org)
        {
            AttractionName = org.AttractionName;
            Description = org.Description;
            Category = org.Category;

            return this;
        }
        #endregion

        public csAttractionDbM()
        {          
        }

        public csAttractionDbM(csAttractionCUdto org)
        {
            AttractionId = Guid.NewGuid();
            UpdateFromDTO(org);
        }

    }
}