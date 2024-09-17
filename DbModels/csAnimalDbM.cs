using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Seido.Utilities.SeedGenerator;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace DbModels
{
    public class csAnimalDbM : csAnimal, ISeed<csAnimalDbM>
    {
        [Key]
        public override Guid AnimalId { get; set; }
        
        
        public override csAnimalDbM Seed(csSeedGenerator _seeder)
        {
            base.Seed(_seeder);
            return this;
        }


    }
}