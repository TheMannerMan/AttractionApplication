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
        public Task<csRespPageDTO<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
            => _repo.ReadAttractionsAsync(seeded, flat, filter, pageNumber, pageSize);
        public Task<csRespPageDTO<ILocation>> ReadLocationsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
            => _repo.ReadLocationsAsync(seeded, flat, filter, pageNumber, pageSize);
        public Task<csRespPageDTO<IReview>> ReadReviewsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize) 
            => _repo.ReadReviewsAsync(seeded, flat, filter, pageNumber, pageSize);

        public csUserAttractionService(IUserAttractionRepo repo)
        {
            _repo = repo;
        }

    }
}