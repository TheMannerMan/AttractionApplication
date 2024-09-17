using Configuration;
using DbModels;
using Models;
using DbContext;
using Seido.Utilities.SeedGenerator;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

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

        public void Seed()
        {
            var _seeder = new csSeedGenerator();

            using (var db = csMainDbContext.DbContext("sysadmin"))
            {

                var users = _seeder.ItemsToList<csUserDbM>(50);
                var attractions = _seeder.ItemsToList<csAttractionDbM>(1000);
                
                //a list to hold all reviews
                var allReviews = new List<csReviewDbM>();

                // For each attraction, generate between 0 and 20 reviews
                foreach (var attraction in attractions)
                {
                    int nrOfReviews = _seeder.Next(0, 21); // Randomizes a number between 0 and 20.
                    if (nrOfReviews > 0)
                    {
                        // For each review, assign a random user and the current attraction
                        for (int i = 0; i < nrOfReviews; i++)
                        {
                            var review = new csReviewDbM().Seed(_seeder);
                            review.UserDbM = _seeder.FromList(users);
                            review.AttractionDbM = attraction;

                            allReviews.Add(review); // Add the review to the collection
                        }
                    }
                }

                db.Users.AddRange(users);
                db.Attractions.AddRange(attractions);
                db.Reviews.AddRange(allReviews);

                db.SaveChanges();
                /*var reviews = _seeder.ItemsToList<csReviewDbM>(100);
                var users = _seeder.ItemsToList<csUserDbM>(50);
                var attractions = _seeder.ItemsToList<csAttractionDbM>(1000);

                foreach (var r in reviews)
                {
                    r.UserDbM = _seeder.FromList(users);
                    r.AttractionDbM = _seeder.FromList(attractions);
                }

                
                foreach (var r in reviews)
                {
                    System.Console.WriteLine(r.ReviewId);
                }

                //db.Users.AddRange(users);
                db.Reviews.AddRange(reviews);

                db.SaveChanges();
                */

            }

        }

    }
}