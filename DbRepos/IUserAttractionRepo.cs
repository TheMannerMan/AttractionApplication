using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbModels;
using Models;
using DbContext;
using Seido.Utilities.SeedGenerator;
using Models.DTO;

namespace DbRepos
{
    public interface IUserAttractionRepo
    {
        #region  Admin repo methods
        public Task<adminInfoDbDto> RemoveSeedAsync(bool seeded);
        public Task<adminInfoDbDto> SeedAsync();
        #endregion

        #region User repo methods
        public Task<csRespPageDTO<IUser>> ReadUsersAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
        public Task<IUser> ReadUserAsync(Guid id, bool flat);
        public Task<IUser> DeleteUserAsync(Guid id);
        public Task<IUser> UpdateUserAsync(csUserCUdto itemDto);
        public Task<IUser> CreateUserAsync(csUserCUdto itemDto);

        #endregion

        #region Review repo methods
        public Task<csRespPageDTO<IReview>> ReadReviewsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
        public Task<IReview> ReadReviewAsync(Guid id, bool flat);
        public Task<IReview> DeleteReviewAsync(Guid id);
        public Task<IReview> UpdateReviewAsync(csReviewCUdto itemDto);
        public Task<IReview> CreateReviewAsync(csReviewCUdto itemDto);
        #endregion

        #region Attraction repo methods
        public Task<csRespPageDTO<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string category, string attractionName,
                                                string description, string city, string country, int pageNumber, int pageSize, bool noComments);
        public Task<IAttraction> ReadAttractionAsync(Guid id, bool flat);
        public Task<IAttraction> DeleteAttractionAsync(Guid id);

        public Task<IAttraction> CreateAttractionAsync(csAttractionCUdto itemDto);

        public Task<IAttraction> UpdateAttractionAsync(csAttractionCUdto itemDto);
        #endregion

        #region Location repo methods
        public Task<csRespPageDTO<ILocation>> ReadLocationsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
        #endregion
    }
}