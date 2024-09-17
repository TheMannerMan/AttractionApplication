using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Models;

namespace Services
{
    public interface IAnimalService
    {
        public List<IAnimal> Read(int count);

        public void Seed(int count);
    }
}