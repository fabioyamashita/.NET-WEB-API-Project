using Microsoft.EntityFrameworkCore;
using SPX_WEBAPI.Domain.Models;
using SPX_WEBAPI.Infra.Data;
using SPX_WEBAPI.Infra.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPX_WEBAPI.Infra.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly InMemoryContext _context;

        public UsersRepository(InMemoryContext context)
        {
            _context = context;
        }

        public Task<List<Users>> Get(int offset, int limit)
        {
            return Task.Run(() =>
            {
                var users = _context.Users
                    .Skip((offset - 1) * limit)
                    .Take(limit)
                    .ToList();

                return users.Any() ? users : new List<Users>();
            });
        }

        public Task<Users> Get(string username, string password)
        {
            return Task.Run(() =>
            {
                var user = _context.Users
                    .FirstOrDefault(item => item.Username.Equals(username) && item.Password.Equals(password));
                return user;
            });
        }

        public Task Insert(Users user)
        {
            return Task.Run(() =>
            {
                _context.Add(user);
                _context.SaveChanges();
            });
        }
    }
}
