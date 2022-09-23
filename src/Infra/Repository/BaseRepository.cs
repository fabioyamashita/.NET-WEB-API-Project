using Dapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    public class BaseRepository<T> : IBaseRepository<T> where T : Spx
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public BaseRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAsync(int offset, int limit)
        {
            //// using LINQ to retrieve data from context
            //var data = _context.Set<T>().AsQueryable()
            //    .OrderByDescending(d => d.Date)
            //    .Skip((offset - 1) * limit)
            //    .Take(limit);

            //return data.Any() ? data : new List<T>().AsQueryable();

            // Using dapper
            var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            var query =
                @"SELECT		[Id], [Date], [Close], [Open], [High], [Low]

                    FROM		Spx

                    ORDER BY	[Date] DESC

                    OFFSET		(@offset - 1) * @limit ROWS FETCH NEXT @limit ROWS ONLY";

            var data = await db.QueryAsync<T>(query, new { offset, limit });

            return data.Any() ? data : new List<T>();
        }

        public async Task<IQueryable<T>> GetAsync(Expression<Func<T, bool>> predicate, int offset, int limit)
        {
            var data = _context.Set<T>().AsQueryable()
                .Where(predicate)
                .Skip((offset - 1) * limit)
                .Take(limit);

            return await data.AnyAsync() ? data : new List<T>().AsQueryable();
        }

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(predicate);
        }

        public async Task InsertAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePatchAsync(T entity, JsonPatchDocument entityUpdated)
        {
            entityUpdated.ApplyTo(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountTotalRecordsAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        public async Task<int> GetLastIdAsync()
        {
            var lastRecord = await _context.Set<T>().OrderBy(x => x.Id).LastAsync();
            return lastRecord.Id;
        }
    }
}
