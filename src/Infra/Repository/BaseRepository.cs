using SPX_WEBAPI.Infra.Data;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Task<int> Delete(int key)
        {
            return Task.Run(() =>
            {
                var entity = _context.Find<T>(key);

                if (entity == null)
                {
                    throw new Exception("ID not found.");
                }

                _context.Remove(entity);
                _context.SaveChanges();
                return key;
            });
        }

        public Task<IQueryable<T>> Get(int offset, int limit)
        {
            return Task.Run(() =>
            {
                var data = _context.Set<T>().AsQueryable().Skip((offset - 1) * limit).Take(limit);
                return data.Any() ? data : new List<T>().AsQueryable();
            });
        }

        public Task<T> GetByKey(int key)
        {
            return Task.Run(() =>
            {
                return _context.Find<T>(key);
            });
        }

        public Task<T> Insert(T entity)
        {
            return Task.Run(() =>
            {
                _context.Add(entity);
                _context.SaveChanges();
                return entity;
            });
        }

        public Task<T> Update(T entity)
        {
            return Task.Run(() =>
            {
                _context.Update(entity);
                _context.SaveChanges();
                return entity;
            });
        }
    }
}
