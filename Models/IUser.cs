using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Seido.Utilities.SeedGenerator;

namespace Models
{
    public interface IUser
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }

        public List<IReview> Reviews{ get; set; }
    }
}