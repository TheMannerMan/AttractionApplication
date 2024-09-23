using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Models.DTO;

namespace Services
{
    public interface IUserAttractionService
    {

        public Task<adminInfoDbDto> RemoveSeedAsync(bool seeded);

        public Task<adminInfoDbDto> SeedAsync();

        public Task<csRespPageDTO<IUser>> ReadUsersAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);

        public Task<csRespPageDTO<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string category, string attractionName, string description, string city, string country, int pageNumber, int pageSize);

        public Task<csRespPageDTO<IReview>> ReadReviewsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);

        public Task<csRespPageDTO<ILocation>> ReadLocationsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
    }
}