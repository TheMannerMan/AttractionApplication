using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DTO
{
    //DTO is a DataTransferObject, can be instanstiated by the controller logic
    //and represents a, fully instatiable, subset of the Database models
    //for a specific purpose.

    //These DTO are simplistic and used to Update and Create objects
    public class csAttractionCUdto
    {
        public virtual Guid? AttractionId { get; set; }
        public virtual string AttractionName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public virtual Guid? LocationId { get; set; }
        public virtual List<Guid> ReviewsId { get; set; }
        //public csAttractionCUdto() {}

        public csAttractionCUdto() { }
        public csAttractionCUdto(IAttraction org)
        {
            AttractionId = org.AttractionId;
            AttractionName = org.AttractionName;
            Description = org.Description;
            Category = org.Category;

            LocationId = org?.Location?.LocationId;
            ReviewsId = org?.Reviews?.Select(r => r.ReviewId).ToList();
        }
    }

    public class csUserCUdto
    {
        public virtual Guid? UserId { get; set; }
        public virtual string UserName { get; set; }
        public virtual List<Guid> ReviewsId { get; set; }
        public csUserCUdto() { }

        public csUserCUdto(IUser org)
        {
            UserId = org.UserId;
            UserName = org.UserName;
            ReviewsId = org?.Reviews?.Select(r => r.ReviewId).ToList();
        }
    }

    public class csReviewCUdto
    {
        public virtual Guid? ReviewId { get; set; }
        public virtual string Comment { get; set; }
        public virtual Guid? UserId { get; set; }
        public virtual Guid? AttractionId { get; set; }

        public csReviewCUdto() { }

        public csReviewCUdto(IReview org)
        {
            ReviewId = org.ReviewId;
            Comment = org.Comment;
            UserId = org?.User?.UserId;
            AttractionId = org?.Attraction?.AttractionId;
        }
    }
}