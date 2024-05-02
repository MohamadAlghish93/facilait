using Microsoft.Extensions.Logging;

namespace FacilaIT.Helper.Shared
{
    public static class Common
    {

        public enum enumTypeService
        {
            Ping,
            Port,
            MSService,
            Process
        }

        public enum enumDatabaseType
        {
            SQL = 1,
            Oracle
        }

        public static LogLevel LogMinLevel(string args)
        {
            switch (args)
            {
                case "Warning":
                    return LogLevel.Warning;
                case "Error":
                    return LogLevel.Error;
                case "Trace":
                    return LogLevel.Trace;
                default:
                    return LogLevel.Information;
            }
        }

        public static List<string> GetComputerSystem = new List<string> { "Name", "Domain", "Workgroup", "TotalPhysicalMemory", "SystemType", "Manufacturer", "SystemFamily", "SystemSKUNumber" };

        public static List<string> GetOperatingSystem = new List<string> { "Caption", "Version", "InstallDate", "LastBootUpTime", "Manufacturer", "OSArchitecture", "SerialNumber", "TotalVirtualMemorySize", "TotalVisibleMemorySize" };
    }
}