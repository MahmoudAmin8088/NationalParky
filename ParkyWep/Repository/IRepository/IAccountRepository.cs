using ParkyWep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWep.Repository.IRepository
{
    public interface IAccountRepository:IRepository<AppUser>
    {
        Task<AppUser> LoginAsync(string ur1, AppUser objToCreate);
        Task<AppUser> RegisterAsync(string ur1, AppUser objToCreate);
    }
}
