using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Seido.Utilities.SeedGenerator;

namespace Models
{
    public class csAnimal : IAnimal, ISeed<csAnimal>
    {
        public virtual Guid AnimalId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }

        public bool Seeded {get; set; }= false;

        public virtual csAnimal Seed(csSeedGenerator _seeder)
        {
            AnimalId = Guid.NewGuid();
            Seeded = true;
            Name = _seeder.PetName;
            return this;
        }

    }
}