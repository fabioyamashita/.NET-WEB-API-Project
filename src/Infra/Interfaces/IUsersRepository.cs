using SPX_WEBAPI.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SPX_WEBAPI.Infra.Interfaces
{
    public interface IUsersRepository
    {
        Task<List<Users>> Get(int offset, int limit);
        Task<Users> Get(string username, string password);
        Task Insert(Users user);
    }
}
