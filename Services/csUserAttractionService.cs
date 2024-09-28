using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Models.DTO;
using DbRepos;
using Seido.Utilities.SeedGenerator;

namespace Services
{
    public class csUserAttractionService : IUserAttractionService
    {
        private IUserAttractionRepo _repo = null;
        public Task<adminInfoDbDto> SeedAsync() => _repo.SeedAsync();
        public Task<adminInfoDbDto> RemoveSeedAsync(bool seeded) => _repo.RemoveSeedAsync(seeded);

        public Task<csRespPageDTO<IUser>> ReadUsersAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
            => _repo.ReadUsersAsync(seeded, flat, filter, pageNumber, pageSize);
        public Task<IUser> DeleteUserAsync(Guid id) => _repo.DeleteUserAsync(id);
        public Task<csRespPageDTO<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string category, string attractionName, string description, string city, string country, int pageNumber, int pageSize, bool noComments)
            => _repo.ReadAttractionsAsync(seeded, flat, category, attractionName, description, city, country, pageNumber, pageSize, noComments);

        public Task<IAttraction> ReadAttractionAsync(Guid id, bool flat) => _repo.ReadAttractionAsync(id, flat);

        public Task<IAttraction> DeleteAttractionAsync(Guid id) => _repo.DeleteAttractionAsync(id);

        public Task<IAttraction> UpdateAttractionAsync(csAttractionCUdto itemDto) => _repo.UpdateAttractionAsync(itemDto);

        public Task<IAttraction> CreateAttractionAsync(csAttractionCUdto itemDto) => _repo.CreateAttractionAsync(itemDto);
        public Task<csRespPageDTO<ILocation>> ReadLocationsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
            => _repo.ReadLocationsAsync(seeded, flat, filter, pageNumber, pageSize);
        public Task<csRespPageDTO<IReview>> ReadReviewsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
            => _repo.ReadReviewsAsync(seeded, flat, filter, pageNumber, pageSize);
        public Task<IReview> DeleteReviewAsync(Guid id) => _repo.DeleteReviewAsync(id);

        public csUserAttractionService(IUserAttractionRepo repo)
        {
            _repo = repo;
        }

    }
}