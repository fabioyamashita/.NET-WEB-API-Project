using Microsoft.AspNetCore.JsonPatch;
using SPX_WEBAPI.Domain.Dto;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SPX_WEBAPI.Infra.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task Delete(T entity);
        Task<IQueryable<T>> Get(int offset, int limit);
        Task<IQueryable<T>> Get(Expression<Func<T, bool>> predicate, int offset, int limit);
        Task<T> GetById(Expression<Func<T, bool>> predicate);
        Task Insert(T entity);
        Task Update(T entity);
        Task UpdatePatch(T entity, JsonPatchDocument entityUpdated);
        Task<int> CountTotalRecords();
    }
}