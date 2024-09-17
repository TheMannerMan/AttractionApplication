using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbContext;
using DbModels;
using Seido.Utilities.SeedGenerator;
using Microsoft.EntityFrameworkCore;

namespace DbRepos
{
    public class csAnimalRepo : IAnimalRepo
    {
        public List<csAnimalDbM> Read(int _count)
        {
            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                List<csAnimalDbM> animals = db.Animals.Take(_count).ToList();
                return animals;
            }
        }

        public void Seed(int _count)
        {
            var _seeder = new csSeedGenerator();
            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                var animals = _seeder.ItemsToList<csAnimalDbM>(_count);

                db.Animals.AddRange(animals);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    var innerException = ex.InnerException?.Message;
                    throw new Exception($"Error during SaveChanges: {innerException}");
                }
            }
        }
    }
}