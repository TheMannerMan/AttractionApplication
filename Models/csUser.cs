using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Seido.Utilities.SeedGenerator;

namespace Models;

public class csUser : IUser, ISeed<csUser>
{
    public virtual Guid UserId { get; set; } = Guid.NewGuid();
    public virtual string UserName { get; set; }

    public virtual List<IReview> Reviews { get; set; } = null;

    public bool Seeded { get; set; } = false;

    public virtual csUser Seed(csSeedGenerator _seeder)
    {
        this.UserName = $"{_seeder.FirstName}{_seeder.Next(0,1000)}";
        return this;
    }
}
