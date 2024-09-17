using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public interface IAnimal
    {
        public Guid AnimalId { get; set; }
        public string Name { get; set; }
    }
}