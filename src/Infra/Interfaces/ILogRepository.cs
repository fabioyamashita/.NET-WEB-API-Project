using System.Threading.Tasks;

namespace SPX_WEBAPI.Infra.Interfaces
{
    public interface ILogRepository
    {
        Task Insert(string message);
    }
}
