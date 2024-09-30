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
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http.Features;

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

/*
                var newLocation = new csLocationDbM().Seed(_seeder);
                bool isUnique = true;

                foreach (var location in locations)
                {

//          Console.WriteLine($"Comparing {newLocation.City}, {newLocation.Country}, {newLocation.StreetAddress} with {location.City}, {location.Country}, {location.StreetAddress}");
                    if (newLocation.Equals(location))
                    {
                        attraction.LocationDbM = location;
                        isUnique = false;
                    }
                }
                foreach (var location in db.Locations)
                {
                    if (newLocation.Equals(location))
                    {
                        attraction.LocationDbM = location;
                        isUnique = false;
                    }
                }

                if (isUnique)
                {
                    locations.Add(newLocation);
                    attraction.LocationDbM = newLocation;
                } */




                
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

    #region Users repo methods
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
                    .Include(i => i.ReviewsDbM);
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
    public async Task<IUser> ReadUserAsync(Guid id, bool flat)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            IQueryable<IUser> _query;

            if (flat)
                _query = db.Users.AsNoTracking()
                        .Where(i => i.UserId == id);

            else
                _query = db.Users.AsNoTracking()
                        .Include(a => a.ReviewsDbM)
                        .Where(i => i.UserId == id);

            return await _query.FirstOrDefaultAsync<IUser>();
        }

    }
    public async Task<IUser> DeleteUserAsync(Guid id)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            var _query = db.Users.Where(a => a.UserId == id);
            var _deleteItem = await _query.FirstOrDefaultAsync<csUserDbM>();

            if (_deleteItem is null)
                throw new ArgumentException($"Item {id} is not existing in the database");

            db.Users.Remove(_deleteItem);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }



            return _deleteItem;
        }
    }
    public async Task<IUser> UpdateUserAsync(csUserCUdto itemDto)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            //Find the instance with matching id and read the navigation properties.
            var _query1 = db.Users
                .Where(i => i.UserId == itemDto.UserId);
            var _item = await _query1
                .Include(i => i.ReviewsDbM)
                .FirstOrDefaultAsync<csUserDbM>();

            //If the item does not exists
            if (_item == null) throw new ArgumentException($"Item {itemDto.UserId} is not existing");

            //transfer any changes from DTO to database objects
            //Update individual properties
            _item.UpdateFromDTO(itemDto);

            //Update navigation properties
            await navProp_csUserCUdto_to_csUserDbM(db, itemDto, _item);

            //write to database model
            db.Users.Update(_item);

            //write to database in a UoW
            await db.SaveChangesAsync();

            //return the updated item in non-flat mode            
            return await ReadUserAsync(_item.UserId, false);
        }
    }

    public async Task<IUser> CreateUserAsync(csUserCUdto itemDto)
    {

        if (itemDto.UserId != null)
            throw new ArgumentException($"{nameof(itemDto.UserId)} must be null when creating a new object");

        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            var newItem = new csUserDbM(itemDto);

            await navProp_csUserCUdto_to_csUserDbM(db, itemDto, newItem);
            db.Users.Add(newItem);
            await db.SaveChangesAsync();
            return await ReadUserAsync(newItem.UserId, false);
        }
    }
    private static async Task navProp_csUserCUdto_to_csUserDbM(csMainDbContext db, csUserCUdto _itemDtoSrc, csUserDbM _itemDst)
    {
        //update ReviewsDbM from itemDto.ReviewsId list
        List<csReviewDbM> _reviews = null;
        if (_itemDtoSrc.ReviewsId != null)
        {
            _reviews = new List<csReviewDbM>();
            foreach (var id in _itemDtoSrc.ReviewsId)
            {
                var r = await db.Reviews.FirstOrDefaultAsync(i => i.ReviewId == id);
                if (r == null)
                    throw new ArgumentException($"Item id {id} not existing");

                _reviews.Add(r);
            }
        }
        _itemDst.ReviewsDbM = _reviews;
    }

    #endregion

    #region Reviews repo methods

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

    public async Task<IReview> ReadReviewAsync(Guid id, bool flat)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            IQueryable<IReview> _query;

            if (flat)
                _query = db.Reviews.AsNoTracking()
                        .Where(i => i.ReviewId == id);

            else
                _query = db.Reviews.AsNoTracking()
                        .Include(a => a.UserDbM)
                        .Include(a => a.AttractionDbM)
                        .Where(i => i.ReviewId == id);

            return await _query.FirstOrDefaultAsync<IReview>();
        }
    }

    public async Task<IReview> DeleteReviewAsync(Guid id)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            var _query = db.Reviews.Where(a => a.ReviewId == id);
            var _deleteItem = await _query.FirstOrDefaultAsync<csReviewDbM>();

            if (_deleteItem is null)
                throw new ArgumentException($"Item {id} is not existing in the database");

            db.Reviews.Remove(_deleteItem);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return _deleteItem;
        }
    }

    public async Task<IReview> UpdateReviewAsync(csReviewCUdto itemDto)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            //Find the instance with matching id and read the navigation properties.
            var _query1 = db.Reviews
                .Where(i => i.ReviewId == itemDto.ReviewId);
            var _item = await _query1
                .Include(i => i.AttractionDbM)
                .Include(i => i.AttractionDbM)
                .FirstOrDefaultAsync<csReviewDbM>();

            //If the item does not exists
            if (_item == null) throw new ArgumentException($"Item {itemDto.UserId} is not existing");

            //transfer any changes from DTO to database objects
            //Update individual properties
            _item.UpdateFromDTO(itemDto);

            //Update navigation properties
            await navProp_csReviewCUdto_to_csReviewDbM(db, itemDto, _item);

            //write to database model
            db.Reviews.Update(_item);

            //write to database in a UoW
            await db.SaveChangesAsync();

            //return the updated item in non-flat mode            
            return await ReadReviewAsync(_item.ReviewId, false);
        }
    }
    public async Task<IReview> CreateReviewAsync(csReviewCUdto itemDto)
    {

        if (itemDto.ReviewId != null)
            throw new ArgumentException($"{nameof(itemDto.AttractionId)} must be null when creating a new object");

        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            var newItem = new csReviewDbM(itemDto);

            await navProp_csReviewCUdto_to_csReviewDbM(db, itemDto, newItem);
            db.Reviews.Add(newItem);
            await db.SaveChangesAsync();
            return await ReadReviewAsync(newItem.ReviewId, false);
        }
    }

    private static async Task navProp_csReviewCUdto_to_csReviewDbM(csMainDbContext db, csReviewCUdto _itemDtoSrc, csReviewDbM _itemDst)
    {

        _itemDst.UserDbM = (_itemDtoSrc.UserId != null) ? await db.Users.FirstOrDefaultAsync(
            a => (a.UserId == _itemDtoSrc.UserId)) : null;

        _itemDst.AttractionDbM = (_itemDtoSrc.AttractionId != null) ? await db.Attractions.FirstOrDefaultAsync(
            a => (a.AttractionId == _itemDtoSrc.AttractionId)) : null;
    }
    #endregion

    #region Attractions repo methods
    public async Task<csRespPageDTO<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string category, string attractionName, string description, string city, string country, int pageNumber, int pageSize, bool noComments)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            IQueryable<csAttractionDbM> _query;
            if (flat)
            {
                _query = db.Attractions.AsNoTracking();
            }
            else
            {
                _query = db.Attractions.AsNoTracking()
                    .Include(i => i.LocationDbM)
                    .Include(i => i.ReviewsDbM);
            }

            _query = _query.Where(i => i.Seeded == seeded);

            // Filters to only show attractions with no comments.
            if (noComments)
                _query = _query.Where(i => !i.ReviewsDbM.Any());


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

    public async Task<IAttraction> ReadAttractionAsync(Guid id, bool flat)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            IQueryable<IAttraction> _query;

            if (flat)
                _query = db.Attractions.AsNoTracking()
                        .Where(i => i.AttractionId == id);

            else
                _query = db.Attractions.AsNoTracking()
                        .Include(a => a.ReviewsDbM)
                        .ThenInclude(r => r.UserDbM)
                        .Include(a => a.LocationDbM)
                        .Where(i => i.AttractionId == id);

            return await _query.FirstOrDefaultAsync<IAttraction>();
        }

    }

    public async Task<IAttraction> DeleteAttractionAsync(Guid id)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            var _query = db.Attractions.Where(a => a.AttractionId == id);
            var _deleteItem = await _query.FirstOrDefaultAsync<csAttractionDbM>();

            if (_deleteItem is null)
                throw new ArgumentException($"Item {id} is not existing in the database");

            db.Attractions.Remove(_deleteItem);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }



            return _deleteItem;
        }
    }

    public async Task<IAttraction> UpdateAttractionAsync(csAttractionCUdto itemDto)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            //Find the instance with matching id and read the navigation properties.
            var _query1 = db.Attractions
                .Where(i => i.AttractionId == itemDto.AttractionId);
            var _item = await _query1
                .Include(i => i.LocationDbM)
                .Include(i => i.ReviewsDbM)
                .FirstOrDefaultAsync<csAttractionDbM>();

            //If the item does not exists
            if (_item == null) throw new ArgumentException($"Item {itemDto.AttractionId} is not existing");

            //transfer any changes from DTO to database objects
            //Update individual properties
            _item.UpdateFromDTO(itemDto);

            //Update navigation properties
            await navProp_csAttractionCUdto_to_csAttractionDbM(db, itemDto, _item);

            //write to database model
            db.Attractions.Update(_item);

            //write to database in a UoW
            await db.SaveChangesAsync();

            //return the updated item in non-flat mode
            return await ReadAttractionAsync(_item.AttractionId, false);
        }
    }

    public async Task<IAttraction> CreateAttractionAsync(csAttractionCUdto itemDto)
    {

        if (itemDto.AttractionId != null)
            throw new ArgumentException($"{nameof(itemDto.AttractionId)} must be null when creating a new object");

        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            var newItem = new csAttractionDbM(itemDto);

            await navProp_csAttractionCUdto_to_csAttractionDbM(db, itemDto, newItem);
            db.Attractions.Add(newItem);
            await db.SaveChangesAsync();
            return await ReadAttractionAsync(newItem.AttractionId, false);
        }
    }

    private static async Task navProp_csAttractionCUdto_to_csAttractionDbM(csMainDbContext db, csAttractionCUdto _itemDtoSrc, csAttractionDbM _itemDst)
    {
        //update LocationDbM from itemDto.LocationId
        _itemDst.LocationDbM = (_itemDtoSrc.LocationId != null) ? await db.Locations.FirstOrDefaultAsync(
            a => (a.LocationId == _itemDtoSrc.LocationId)) : null;

        //update ReviewsDbM from itemDto.ReviewsId list
        List<csReviewDbM> _reviews = null;
        if (_itemDtoSrc.ReviewsId != null)
        {
            _reviews = new List<csReviewDbM>();
            foreach (var id in _itemDtoSrc.ReviewsId)
            {
                var r = await db.Reviews.FirstOrDefaultAsync(i => i.ReviewId == id);
                if (r == null)
                    throw new ArgumentException($"Item id {id} not existing");

                _reviews.Add(r);
            }
        }
        _itemDst.ReviewsDbM = _reviews;
    }

    #endregion

    #region Locations repo methods
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
                    .Include(l => l.AttractionsDbM);
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

    #endregion
}