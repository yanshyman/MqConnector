using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using IBM.WMQ;
using log4net;
using MqConnector.Models;


namespace MqConnector
{
    public static class MqDownloader
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const int MqEncoding = MQC.CODESET_819; // "iso-8859-1" 
        private static readonly Encoding OutputEncoding = Encoding.GetEncoding(28591);// "iso-8859-1" 

        public static MQQueue GetMqQueue(string queueName, bool browse, MQQueueManager queueManager)
        {
            var openOptions = browse ? MQC.MQOO_BROWSE : MQC.MQOO_INPUT_EXCLUSIVE + MQC.MQOO_FAIL_IF_QUIESCING;
            var mqQueue = queueManager.AccessQueue(queueName, openOptions);

            return mqQueue;
        }

        public static string GetMessage(MqConnectionSettings mqConnectionSettings, string queueName, bool browse, bool firstMessage = true)
        {
            var queueManager = MqManagerFactory.Create(mqConnectionSettings);
            var mqQueue = GetMqQueue(queueName, browse, queueManager);
            var messageContent = string.Empty;
            try
            {
                var mqMsg = MqUploader.CreateNewMqMessage();
                messageContent = mqQueue.GetMessageContent(mqMsg, browse, firstMessage);
                mqMsg.ClearMessage();
                queueManager.Commit();
            }
            catch (Exception)
            {
                queueManager.Disconnect();
                mqQueue?.Close();
            }
            return messageContent;
        }

        public static IEnumerable<string> GetAllMessages(MqConnectionSettings mqConnectionSettings, string queueName, bool browse)
        {
            string messageContent;
            var count = 0;
            do
            {
                messageContent = GetMessage(mqConnectionSettings, queueName, browse, count == 0);
                count++;
                yield return messageContent;

            } while (messageContent != string.Empty);
        }

        private static string GetMessageContent(this MQDestination mqQueue, MQMessage mqMsg, bool browse, bool isFirstMessage)
        {
            var mqGetMsgOpts = new MQGetMessageOptions
            {
                Options = GetMqGetOptions(browse, isFirstMessage)
            };

            mqQueue.Get(mqMsg, mqGetMsgOpts);
            Logger.Info($"Message CCSID: {mqMsg.CharacterSet} ({MqEncoding} expected), Queue:{mqQueue.Name}");
            var messageContent = new byte[0];
            mqMsg.ReadFully(ref messageContent);
            return OutputEncoding.GetString(messageContent);
        }

        private static int GetMqGetOptions(bool browse, bool isFirstMessage)
        {
            if (!browse) return MQC.MQGMO_WAIT + MQC.MQGMO_SYNCPOINT;
            var result = MQC.MQGMO_FAIL_IF_QUIESCING + MQC.MQGMO_WAIT;
            result += isFirstMessage ? MQC.MQGMO_BROWSE_FIRST : MQC.MQGMO_BROWSE_NEXT;
            return result;
        }
    }
}