using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Configuration;
using Models;
namespace DbModels
{
    public class csReviewDbM : csReview, ISeed<csReviewDbM>
    {
        [Key]
        public override Guid ReviewId { get; set; } = Guid.NewGuid();

        [NotMapped]
        public override IUser User { get => UserDbM; set => new NotImplementedException(); }

        [JsonIgnore]
        //[ForeignKey("UserId")] //create own Foreign Key step 2
        public virtual csUserDbM UserDbM { get; set; } = null;

        


        public override csReviewDbM Seed(csSeedGenerator _seeder)
        {
            base.Seed(_seeder);
            return this;
        }
    }
}