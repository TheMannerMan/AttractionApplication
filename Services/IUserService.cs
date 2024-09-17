using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace Services
{
    public interface IUserService
    {
        public List<IUser> ReadUsersAsync(int count);

        public void Seed();
    }
}