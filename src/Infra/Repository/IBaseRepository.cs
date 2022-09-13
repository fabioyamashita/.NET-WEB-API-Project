using Microsoft.AspNetCore.JsonPatch;
using System.Linq;
using System.Threading.Tasks;

namespace SPX_WEBAPI.Infra.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        Task Delete(int key);
        Task<IQueryable<T>> Get(int offset, int limit);
        Task<T> GetById(int key);
        Task Insert(T entity);
        Task Update(T entity);
        Task UpdatePatch(T entity, JsonPatchDocument entityUpdated);
    }
}