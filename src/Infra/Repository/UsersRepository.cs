using Microsoft.EntityFrameworkCore;
using SPX_WEBAPI.Domain.Models;
using SPX_WEBAPI.Infra.Data;
using SPX_WEBAPI.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SPX_WEBAPI.Infra.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ApplicationDbContext _context;

        public UsersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Users>> GetAsync(int offset, int limit)
        {
            var users = await _context.Users
                .Skip((offset - 1) * limit)
                .Take(limit)
                .ToListAsync();

            return users.Any() ? users : new List<Users>();
        }

        public async Task<Users> GetAsync(string username)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(item => item.Username.ToLower() == username.ToLower());

            return user;
        }

        public async Task InsertAsync(Users user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
