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
        public Task<IUser> DeleteUserAsync(Guid id);
        public Task<csRespPageDTO<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string category, string attractionName, string description, string city, string country, int pageNumber, int pageSize, bool noComments);
        public Task<IAttraction> ReadAttractionAsync(Guid id, bool flat);
        public Task<IAttraction> UpdateAttractionAsync(csAttractionCUdto itemDto);
        public Task<IAttraction> CreateAttractionAsync(csAttractionCUdto itemDto);
        public Task<csRespPageDTO<IReview>> ReadReviewsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
        public Task<IReview> DeleteReviewAsync(Guid id);
        public Task<IAttraction> DeleteAttractionAsync(Guid id);
        public Task<csRespPageDTO<ILocation>> ReadLocationsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
    }
}