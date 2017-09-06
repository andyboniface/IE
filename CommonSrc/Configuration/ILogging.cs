using System;
namespace IE.CommonSrc.Configuration
{
    public interface ILogging
    {
        void LogError(string msg);
        void LogWarning(string msg);
        void LogInfo(string msg);
    }
}
