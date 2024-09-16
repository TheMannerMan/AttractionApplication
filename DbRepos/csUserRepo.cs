using Configuration;
using DbModels;
using Models;
using DbContext;
using Seido.Utilities.SeedGenerator;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace DbRepos
{
    public class csUserRepo : IUserRepo
    {
        public List<IUser> ReadUsersAsync(int _count)
        {

            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                var users = db.Users.Take(_count).ToList<IUser>();
                return users;
            }
        }

        public void Seed(int _count)
        {
            var _seeder = new csSeedGenerator();

            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                var users = _seeder.ItemsToList<csUserDbM>(_count);
                db.Users.AddRange(users);

                db.SaveChanges();
            }

        }

    }
}