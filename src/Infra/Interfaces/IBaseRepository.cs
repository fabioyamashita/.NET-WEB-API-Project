using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SPX_WEBAPI.Infra.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task DeleteAsync(T entity);
        Task<IEnumerable<T>> GetAsync(int offset, int limit);
        Task<IQueryable<T>> GetAsync(Expression<Func<T, bool>> predicate, int offset, int limit);
        Task<T> GetByIdAsync(Expression<Func<T, bool>> predicate);
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task UpdatePatchAsync(T entity, JsonPatchDocument entityUpdated);
        Task<int> CountTotalRecordsAsync();
        Task<int> GetLastIdAsync();
    }
}