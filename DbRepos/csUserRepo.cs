using Configuration;
using DbModels;
using Models;
using DbContext;
using Seido.Utilities.SeedGenerator;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.EntityFrameworkCore;

namespace DbRepos
{
    public class csUserRepo : IUserRepo
    {
        public List<csUserDbM> ReadUsersAsync(int _count)
        {

            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                List<csUserDbM> users = db.Users.Include(a => a.ReviewsDbM).Take(_count).ToList();
                return users;
            }
        }

        public void Seed(int _count)
        {
            var _seeder = new csSeedGenerator();

            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                var reviews = _seeder.ItemsToList<csReviewDbM>(100);
                var users = _seeder.ItemsToList<csUserDbM>(100);

                foreach (var r in reviews)
                {
                    r.UserDbM = _seeder.FromList(users);
                }

                /*
                foreach (var r in reviews)
                {
                    System.Console.WriteLine(r.ReviewId);
                } */

                //db.Users.AddRange(users);
                db.Reviews.AddRange(reviews);

                db.SaveChanges();


            }

        }

    }
}