using System;
using IE.CommonSrc.Configuration;
using Xamarin.Forms;

[assembly: Dependency(typeof(IE.Droid.src.Configuration.Logger))]
namespace IE.Droid.src.Configuration
{
    public class Logger : ILogging
    {
        public Logger() 
        {
            
        }

        public void LogError(string msg)
        {
            Console.WriteLine("IE:Error: " + msg);
        }

        public void LogInfo(string msg)
        {
            Console.WriteLine("IE:Info: " + msg);
		}

        public void LogWarning(string msg)
        {
            Console.WriteLine("IE:Warn: " + msg);
		}
    }
}
