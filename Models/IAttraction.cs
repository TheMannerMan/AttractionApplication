using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace Models
{
    public interface IAttraction
    {
        public Guid AttractionId { get; set; }
        public string AttractionName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public List<IReview> Reviews { get; set; }

        // TODO: Add when Location is implemented.
        // public ILocation Location {get; set;}

    }
}