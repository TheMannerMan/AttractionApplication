using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using DbRepos;
using Seido.Utilities.SeedGenerator;

namespace Services
{
    public class csUserService1 : IUserService
    {
        private IUserRepo _repo = null;
        public List<IUser> ReadUsersAsync(int count) => _repo.ReadUsersAsync(count).ToList<IUser>();

        public void Seed(int count) => _repo.Seed(count);

        public csUserService1(IUserRepo repo)
        {
            _repo = repo;
        }
    }
}