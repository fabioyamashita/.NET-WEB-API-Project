using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using SPX_WEBAPI.Infra.Data;
using SPX_WEBAPI.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SPX_WEBAPI.Infra.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly InMemoryContext _context;

        public BaseRepository(InMemoryContext inMemoryContext)
        {
            _context = inMemoryContext;
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<T>> GetAsync(int offset, int limit)
        {
            var data = _context.Set<T>().AsQueryable()
                .Skip((offset - 1) * limit)
                .Take(limit);

            return await data.AnyAsync() ? data : new List<T>().AsQueryable();
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

    }
}
