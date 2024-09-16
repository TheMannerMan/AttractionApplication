using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbModels;
using Models;
using DbContext;
using Seido.Utilities.SeedGenerator;

namespace DbRepos
{
    public interface IUserRepo
    {
        public List<IUser> ReadUsersAsync(int _count);

        public void Seed(int _count);
    }
}