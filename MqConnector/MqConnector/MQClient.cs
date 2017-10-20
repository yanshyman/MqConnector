using System.Collections.Generic;
using MqConnector.Models;


namespace MqConnector
{
    public static class MqClient
    {
        public static string TestConnection(MqConnectionSettings mqConnectionSettings)
        {
            var queueManager = MqManagerFactory.Create(mqConnectionSettings);
            var response = queueManager.IsConnected
                ? "Connected Successfully"
                : "Problem accesing WebSphere MQ";
            queueManager.Disconnect();
            return response;
        }

        public static string SendMessage(MqConnectionSettings mqConnectionSettings, string queueName,
            string messageContent)
        {
            return MqUploader.SendMessage(mqConnectionSettings, queueName, messageContent);
        }

        public static IEnumerable<string> GetAllMessages(MqConnectionSettings mqConnectionSettings, string queueName,
            bool browse)
        {
            return MqDownloader.GetAllMessages(mqConnectionSettings, queueName, browse);
        }

        public static string GetMessage(MqConnectionSettings mqConnectionSettings, string queueName,
        bool browse)
        {
            return MqDownloader.GetMessage(mqConnectionSettings, queueName, browse);
        }
    }
}
