using Microsoft.AspNetCore.Builder;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SPX_WEBAPI.Infra.Repository
{
    public class LogTxtRepository : ILogRepository
    {
        public Task Insert(string message)
        {
            return Task.Run(() =>
            {
                var path = System.IO.Directory.GetCurrentDirectory();
                string fileName = "spxLog.txt";

                using StreamWriter file = new($@"{path}\{fileName}", append: true);
                file.WriteLine(message);
            });
        }
    }
}
