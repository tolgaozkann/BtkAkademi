using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtkAkademi.Services.Contracts
{
    public interface ILoggerService
    {
        public void LogInfo(string message);
        public void LogWarning(string message);
        public void LogError(string message);
        public void LogDebug(string message);
    }
}
