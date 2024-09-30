using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Seido.Utilities.SeedGenerator;
using Configuration;
using Models;
using Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace DbModels
{
    [Index(nameof(Comment))]
    public class csReviewDbM : csReview, ISeed<csReviewDbM>
    {
        [Key]
        public override Guid ReviewId { get; set; } = Guid.NewGuid();

        [Required]
        public override string Comment { get; set; }

        #region User relation
        [NotMapped]
        public override IUser User { get => UserDbM; set => new NotImplementedException(); }

        [Required]
        [JsonIgnore]
        [ForeignKey("UserId")]
        public virtual csUserDbM UserDbM { get; set; } = null;

        [JsonIgnore]
        public Guid? UserId { get; set; }

        #endregion

        #region Attraction relation
        [NotMapped]
        public override IAttraction Attraction { get => AttractionDbM; set => new NotImplementedException(); }

        [Required]
        [JsonIgnore]
        [ForeignKey("AttractionId")]
        public virtual csAttractionDbM AttractionDbM { get; set; } = null;

        [JsonIgnore]
        public Guid? AttractionId { get; set; }
        #endregion
  
        public override csReviewDbM Seed(csSeedGenerator _seeder)
        {
            base.Seed(_seeder);
            return this;
        }

        public csReviewDbM UpdateFromDTO(csReviewCUdto org)
        {
            Comment = org.Comment;
            return this;
        }

        public csReviewDbM()
        {

        }
        public csReviewDbM(csReviewCUdto org)
        {
            ReviewId = Guid.NewGuid();
            UpdateFromDTO(org);
        }
    }
}