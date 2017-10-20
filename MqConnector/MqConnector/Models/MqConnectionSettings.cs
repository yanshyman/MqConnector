using System.ComponentModel;

namespace MqConnector.Models
{
    public class MqConnectionSettings
    {
        public string QueueManagerName { get; set; }

        public string ChannelName { get; set; }

        public string Host { get; set; }

        [DefaultValue(1414)]
        public int Port { get; set; } = 1414;

        public bool EnableSSL { get; set;}

        public override string ToString()
        {
            return $"Manager: '{QueueManagerName}', Channel: '{ChannelName}', Host: '{Host}', Port: '{Port}'";
        }
    }
}