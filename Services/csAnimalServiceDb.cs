using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using DbRepos;

namespace Services
{
    public class csAnimalServiceDb : IAnimalService
    {
        
        private IAnimalRepo _repo = null;

        public List<IAnimal> Read(int count) => _repo.Read(count).ToList<IAnimal>();

        public void Seed(int count) => _repo.Seed(count);

        public csAnimalServiceDb(IAnimalRepo repo)
        {
            _repo = repo;
        }
    }
}