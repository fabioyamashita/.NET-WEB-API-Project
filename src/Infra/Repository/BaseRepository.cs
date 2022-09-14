using Microsoft.AspNetCore.JsonPatch;
using SPX_WEBAPI.Domain.Dto;
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

        public Task Delete(T entity)
        {
            return Task.Run(() =>
            {
                _context.Set<T>().Remove(entity);
                _context.SaveChanges();
            });
        }

        public Task<IQueryable<T>> Get(int offset, int limit)
        {
            return Task.Run(() =>
            {
                var data = _context.Set<T>().AsQueryable()
                    .Skip((offset - 1) * limit)
                    .Take(limit);

                return data.Any() ? data : new List<T>().AsQueryable();
            });
        }


        public Task<IQueryable<T>> Get(Expression<Func<T, bool>> predicate, int offset, int limit)
        {
            return Task.Run(() =>
            {
                var data = _context.Set<T>().AsQueryable()
                    .Where(predicate)
                    .Skip((offset - 1) * limit)
                    .Take(limit);

                return data.Any() ? data : new List<T>().AsQueryable();
            });
        }

        public Task<T> GetById(Expression<Func<T, bool>> predicate)
        {
            return Task.Run(() =>
            {
                return _context.Set<T>().SingleOrDefault(predicate);
            });
        }

        public Task Insert(T entity)
        {
            return Task.Run(() =>
            {
                _context.Set<T>().Add(entity);
                _context.SaveChanges();
            });
        }

        public Task Update(T entity)
        {
            return Task.Run(() =>
            {
                _context.Set<T>().Update(entity);
                _context.SaveChanges();
            });
        }

        public Task UpdatePatch(T entity, JsonPatchDocument entityUpdated)
        {
            return Task.Run(() =>
            {
                entityUpdated.ApplyTo(entity);
                _context.SaveChanges();
            });
        }

        public Task<int> CountTotalRecords()
        {
            return Task.Run(() =>
            {
                return _context.Set<T>().Count();
            });
        }

    }
}
