using System.Threading.Tasks;

namespace SPX_WEBAPI.Infra.Repository
{
    public interface ILogRepository
    {
        Task Insert(string message);
    }
}
