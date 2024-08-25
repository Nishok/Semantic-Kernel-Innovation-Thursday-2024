using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace CustomDatePlugin.Plugins
{
    public class TimePlugin
    {
        [KernelFunction]
        [Description("Get the current date and time in UTC.")]
        public string GetUTCDateTime()
        {
            return DateTime.UtcNow.ToString("R");
        }

        [KernelFunction]
        [Description("Get the current date and time in local timezone.")]
        public string GetTimeZone()
        {
            return DateTime.Now.ToString("R");
        }
    }
}
