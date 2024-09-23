using Configuration;
using DbModels;
using Models;
using DbContext;
using Seido.Utilities.SeedGenerator;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Models.DTO;
using Microsoft.AspNetCore.Identity.Data;
using SQLitePCL;
using System.Net.Http.Headers;

namespace DbRepos;

public class csUserAttractionRepo : IUserAttractionRepo
{
    #region Admin repos methods
    public async Task<adminInfoDbDto> RemoveSeedAsync(bool seeded)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            db.Users.RemoveRange(db.Users.Where(f => f.Seeded == seeded));
            db.Locations.RemoveRange(db.Locations.Where(f => f.Seeded == seeded));
            db.Reviews.RemoveRange(db.Reviews.Where(f => f.Seeded == seeded));
            db.Attractions.RemoveRange(db.Attractions.Where(f => f.Seeded == seeded));

            var _info = new adminInfoDbDto();

            if (seeded)
            {
                //Explore the changeTrackerNr of items to be deleted
                _info.nrSeededUsers = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csUserDbM) && entry.State == EntityState.Deleted);
                _info.nrSeededLocations = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csLocationDbM) && entry.State == EntityState.Deleted);
                _info.nrSeededReviews = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csReviewDbM) && entry.State == EntityState.Deleted);
                _info.nrSeededAttractions = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csAttractionDbM) && entry.State == EntityState.Deleted);
            }
            else
            {
                //Explore the changeTrackerNr of items to be deleted
                _info.nrUnseededUsers = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csUserDbM) && entry.State == EntityState.Deleted);
                _info.nrUnseededLocations = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csLocationDbM) && entry.State == EntityState.Deleted);
                _info.nrUnseededReviews = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csReviewDbM) && entry.State == EntityState.Deleted);
                _info.nrUnseededAttractions = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csAttractionDbM) && entry.State == EntityState.Deleted);
            }
            await db.SaveChangesAsync();
            return _info;
        }

    }

    public async Task<adminInfoDbDto> SeedAsync()
    {

        await RemoveSeedAsync(true); // Clears the DB of seeded data.

        var _seeder = new csSeedGenerator();
        var _info = new adminInfoDbDto();

        using (var db = csMainDbContext.DbContext("sysadmin"))
        {

            var users = _seeder.ItemsToList<csUserDbM>(50);
            var attractions = _seeder.ItemsToList<csAttractionDbM>(1000);

            //a list to hold all reviews
            var allReviews = new List<csReviewDbM>();
            var locations = new List<csLocationDbM>();


            // Creating and adding Locations and Reviews for each Attraction.
            foreach (var attraction in attractions)
            {
                // Create a random location.
                var newLocation = new csLocationDbM().Seed(_seeder);

                // Check if the randomlocation already exists in the in-memory list or in the database
                var existingLocation = locations.FirstOrDefault(l => l.City == newLocation.City && l.Country == newLocation.Country && l.StreetAddress == newLocation.StreetAddress)
                ?? db.Locations.FirstOrDefault(l => l.City == newLocation.City && l.Country == newLocation.Country && l.StreetAddress == newLocation.StreetAddress);

                //If random location doesnt exist, add it to the in-memory list of locations and assign it to the attraction.
                if (existingLocation == null)
                {
                    locations.Add(newLocation);
                    attraction.LocationDbM = newLocation;
                }
                // If a instance of a random location already exist, assign the existing instance to the attraction.
                else
                {
                    attraction.LocationDbM = existingLocation;
                }

                int nrOfReviews = _seeder.Next(0, 21); // Randomizes a number between 0 and 20.
                if (nrOfReviews > 0)
                {
                    // Create a number of reviews of the random number and assign each review a user and assign it to the current attraction.
                    for (int i = 0; i < nrOfReviews; i++)
                    {
                        var review = new csReviewDbM().Seed(_seeder);
                        review.UserDbM = _seeder.FromList(users);
                        review.AttractionDbM = attraction;

                        allReviews.Add(review); // Add the review to the collection
                    }
                }
            }

            foreach (var loc in locations)
            {
                System.Console.WriteLine($"{loc.City} {loc.Country} ID: {loc.LocationId}");
            }

            db.Locations.AddRange(locations);
            db.Users.AddRange(users);
            db.Attractions.AddRange(attractions);
            db.Reviews.AddRange(allReviews);

            _info.nrSeededUsers = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csUserDbM) && entry.State == EntityState.Added);
            _info.nrSeededLocations = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csLocationDbM) && entry.State == EntityState.Added);
            _info.nrSeededReviews = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csReviewDbM) && entry.State == EntityState.Added);
            _info.nrSeededAttractions = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csAttractionDbM) && entry.State == EntityState.Added);

            await db.SaveChangesAsync();
            return _info;
        }

    }

    #endregion


    public async Task<csRespPageDTO<IUser>> ReadUsersAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            filter ??= "";
            IQueryable<csUserDbM> _query;
            if (flat)
            {
                _query = db.Users.AsNoTracking();
            }
            else
            {
                _query = db.Users.AsNoTracking()
                    .Include(i => i.ReviewsDbM)
                    .ThenInclude(r => r.AttractionDbM)
                    .ThenInclude(a => a.LocationDbM);
            }

            var _ret = new csRespPageDTO<IUser>()
            {
                DbItemsCount = await _query

            //Adding filter functionality
            .Where(i => (i.Seeded == seeded) &&
                        i.UserName.ToLower().Contains(filter)).CountAsync(),

                PageItems = await _query

            // Adding filter functionality
            .Where(i => (i.Seeded == seeded) &&
                        i.UserName.ToLower().Contains(filter))

            // Adding paging
            .Skip(pageNumber * pageSize)
            .Take(pageSize)

            .ToListAsync<IUser>(),

                PageNr = pageNumber,
                PageSize = pageSize

            };
            return _ret;
        }
    }

    public async Task<csRespPageDTO<IReview>> ReadReviewsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            filter ??= "";
            IQueryable<csReviewDbM> _query;
            if (flat)
            {
                _query = db.Reviews.AsNoTracking();
            }
            else
            {
                _query = db.Reviews.AsNoTracking()
                    .Include(i => i.UserDbM)
                    .Include(i => i.AttractionDbM)
                    .ThenInclude(a => a.LocationDbM);
            }

            var _ret = new csRespPageDTO<IReview>()
            {
                DbItemsCount = await _query

            //Adding filter functionality to find specific comments
            .Where(i => (i.Seeded == seeded) &&
                        i.Comment.ToLower().Contains(filter)).CountAsync(),

                PageItems = await _query

            // Adding filter functionality
            .Where(i => (i.Seeded == seeded) &&
                        i.Comment.ToLower().Contains(filter))

            // Adding paging
            .Skip(pageNumber * pageSize)
            .Take(pageSize)

            .ToListAsync<IReview>(),

                PageNr = pageNumber,
                PageSize = pageSize

            };
            return _ret;
        }
    }

    public async Task<csRespPageDTO<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string category, string attractionName, string description, string city, string country, int pageNumber, int pageSize)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            IQueryable<csAttractionDbM> _query;
            if (flat)
            {
                _query = db.Attractions.AsNoTracking()
                .Include(i => i.LocationDbM);
            }
            else
            {
                _query = db.Attractions.AsNoTracking()
                    .Include(i => i.LocationDbM)
                    .Include(i => i.ReviewsDbM)
                    .ThenInclude(r => r.UserDbM);
            }

            _query = _query.Where(i => i.Seeded == seeded);


            if (!string.IsNullOrEmpty(category))
            {
                _query = _query.Where(i => i.Category.ToLower().Contains(category.ToLower()));
            }


            if (!string.IsNullOrEmpty(attractionName))
            {
                _query = _query.Where(i => i.AttractionName.ToLower().Contains(attractionName.ToLower()));
            }


            if (!string.IsNullOrEmpty(description))
            {
                _query = _query.Where(i => i.Description.ToLower().Contains(description.ToLower()));
            }


            if (!string.IsNullOrEmpty(city))
            {
                _query = _query.Where(i => i.LocationDbM.City.ToLower().Contains(city.ToLower()));
            }


            if (!string.IsNullOrEmpty(country))
            {
                _query = _query.Where(i => i.LocationDbM.Country.ToLower().Contains(country.ToLower()));
            }

            var _ret = new csRespPageDTO<IAttraction>()
            {
                DbItemsCount = await _query.CountAsync(),

                PageItems = await _query
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync<IAttraction>(),

                PageNr = pageNumber,
                PageSize = pageSize

            };
            return _ret;
        }
    }
    // Sparar för eventuellt senare bruk
    /*public async Task<csRespPageDTO<ILocation>> ReadAttractionsAsync(bool seeded, bool flat, string category, string attractionName, string description, string city, string country, int pageNumber, int pageSize)
        {
            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                category ??= "";
                attractionName ??= "";
                description ??= "";
                city ??= "";
                country ??= "";

                IQueryable<csAttractionDbM> _query;
                if (flat)
                {
                    _query = db.Attractions.AsNoTracking()
                    .Include(i => i.LocationDbM);
                }
                else
                {
                    _query = db.Attractions.AsNoTracking()
                        .Include(i => i.LocationDbM)
                        .Include(i => i.ReviewsDbM)
                        .ThenInclude(r => r.UserDbM);
                }

                var _ret = new csRespPageDTO<IAttraction>()
                {
                    DbItemsCount = await _query

                //Adding filter functionality
                .Where(i => (i.Seeded == seeded) &&
                            i.AttractionName.ToLower().Contains(filter)).CountAsync(),

                    PageItems = await _query

                // Adding filter functionality
                .Where(i => (i.Seeded == seeded) &&
                            i.AttractionName.ToLower().Contains(filter))

                // Adding paging
                .Skip(pageNumber * pageSize)
                .Take(pageSize)

                .ToListAsync<IAttraction>(),

                    PageNr = pageNumber,
                    PageSize = pageSize

                };
                return _ret;
            }
        }

        */
    public async Task<csRespPageDTO<ILocation>> ReadLocationsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            filter ??= "";
            IQueryable<csLocationDbM> _query;
            if (flat)
            {
                _query = db.Locations.AsNoTracking();
            }
            else
            {
                _query = db.Locations.AsNoTracking()
                    .Include(l => l.AttractionsDbM)
                    .ThenInclude(a => a.ReviewsDbM)
                    .ThenInclude(r => r.UserDbM);
            }

            var _ret = new csRespPageDTO<ILocation>()
            {
                DbItemsCount = await _query

            //Adding filter functionality to find specific comments
            .Where(i => (i.Seeded == seeded) &&
                        (i.City.ToLower().Contains(filter) || i.Country.ToLower().Contains(filter))).CountAsync(),

                PageItems = await _query

            // Adding filter functionality
            .Where(i => (i.Seeded == seeded) &&
                        (i.City.ToLower().Contains(filter) || i.Country.ToLower().Contains(filter)))

            // Adding paging
            .Skip(pageNumber * pageSize)
            .Take(pageSize)

            .ToListAsync<ILocation>(),

                PageNr = pageNumber,
                PageSize = pageSize

            };
            return _ret;
        }
    }
}