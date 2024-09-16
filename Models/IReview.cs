using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public interface IReview
    {
        public Guid ReviewId { get; set; }
        public string Comment { get; set; }
        public IUser User { get; set; }

    }
}