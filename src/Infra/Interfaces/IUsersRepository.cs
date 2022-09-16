using SPX_WEBAPI.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SPX_WEBAPI.Infra.Interfaces
{
    public interface IUsersRepository
    {
        Task<List<Users>> GetAsync(int offset, int limit);
        Task<Users> GetAsync(string username);
        Task InsertAsync(Users user);
    }
}
