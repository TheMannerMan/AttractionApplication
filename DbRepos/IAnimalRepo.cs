using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbModels;

namespace DbRepos
{
    public interface IAnimalRepo
    {
        public List<csAnimalDbM> Read(int _count);
        public void Seed(int _count);
    }
}