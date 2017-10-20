using System;
using IBM.WMQ;
using MqConnector.Models;


namespace MqConnector
{
    public class MqUploader
    {
        private const int MqEncoding = MQC.CODESET_819; // "iso-8859-1" 

        public static string SendMessage(MqConnectionSettings mqConnectionSettings, string queueName, string message)
        {

            var queueManager = MqManagerFactory.Create(mqConnectionSettings);

          var queue = queueManager.AccessQueue(queueName, MQC.MQOO_OUTPUT + MQC.MQOO_FAIL_IF_QUIESCING);
            

            var queueMessage = CreateNewMqMessage();
            var queuePutMessageOptions = new MQPutMessageOptions();
            AddMessageContent(message, queueMessage);
           
                queue.Put(queueMessage, queuePutMessageOptions);
                queue.Close();
                queueManager.Commit();
                queueManager.Disconnect();
   
            var msgId = Convert.ToBase64String(queueMessage.MessageId ?? new byte[0]);
            return  msgId;
           
        }

        private static void AddMessageContent(string message, MQMessage queueMessage)
        {
            var messageContent = message ?? string.Empty;
            queueMessage.WriteString(messageContent);
        }

        public static MQMessage CreateNewMqMessage()
        {
            return new MQMessage
            {
                Format = MQC.MQFMT_STRING,
                CharacterSet = MqEncoding,
            };
        }
    }
}