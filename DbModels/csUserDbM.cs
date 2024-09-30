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
using Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace DbModels
{
    [Index(nameof(UserName))]
    public class csUserDbM : csUser, ISeed<csUserDbM>
    {
        [Key]
        public override Guid UserId { get; set; } = Guid.NewGuid();

        [Required]
        public override string UserName { get; set; }

        [NotMapped]
        public override List<IReview> Reviews { get => ReviewsDbM?.ToList<IReview>(); set => new NotImplementedException(); }

        [JsonIgnore]
        public virtual List<csReviewDbM> ReviewsDbM { get; set; } = null;

        public override csUserDbM Seed(csSeedGenerator _seeder)
        {
            base.Seed(_seeder);
            return this;
        }

        public csUserDbM UpdateFromDTO(csUserCUdto org)
        {
            UserName = org.UserName;
            return this;
        }

        public csUserDbM()
        {

        }
        public csUserDbM(csUserCUdto org)
        {
            UserId = Guid.NewGuid();
            UpdateFromDTO(org);
        }
    }
}