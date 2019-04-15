using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    public interface ILoggingService
    {
        void LogDebug(string message);
        void LogError(string errorMessage);
    }
    public class LoggingService : ILoggingService
    {
        public void LogDebug(string message)
        {
            Debug.WriteLine(message);
            Console.WriteLine(message);
        }

        public void LogError(string errorMessage)
        {
            Debug.WriteLine(errorMessage);
            Console.WriteLine("Error : " +errorMessage);
        }
    }
}
