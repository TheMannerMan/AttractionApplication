using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class csAttractionCUdto
    {
        public virtual Guid? AttractionId { get; set; }
        public virtual string AttractionName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public virtual Guid? LocationId { get; set; }
        public virtual List<Guid> ReviewsId { get; set; }
        //public csAttractionCUdto() {}

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
}
