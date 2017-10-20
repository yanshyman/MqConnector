using System.Collections;
using System.Configuration;
using System.Reflection;
using IBM.WMQ;
using log4net;
using MqConnector.Models;

namespace MqConnector
{
    public class MqManagerFactory
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static MQQueueManager Create(MqConnectionSettings mqConnectionSettings)
        {
           
            var properties = new Hashtable
            {
                {MQC.HOST_NAME_PROPERTY, mqConnectionSettings.Host},
                {MQC.PORT_PROPERTY, mqConnectionSettings.Port},
                {MQC.CHANNEL_PROPERTY, mqConnectionSettings.ChannelName},
                {MQC.TRANSPORT_PROPERTY, MQC.TRANSPORT_MQSERIES_MANAGED }
            };
            if (mqConnectionSettings.EnableSSL) SetupSsl(properties);
        return new MQQueueManager(mqConnectionSettings.QueueManagerName, properties);
        }

        private static void SetupSsl(IDictionary properties)
        {
            var certFriendlyName = ConfigurationManager.AppSettings["CertLabel"];
            var mqCipherSpec =   ConfigurationManager.AppSettings["MQCipherSpec"];
            var keyStore = ConfigurationManager.AppSettings["KeyStore"];
            if (certFriendlyName != string.Empty)
            {
                MQEnvironment.CertificateLabel = certFriendlyName;
            }
            properties.Add(MQC.SSL_CERT_STORE_PROPERTY, keyStore);
            properties.Add(MQC.SSL_CIPHER_SPEC_PROPERTY, mqCipherSpec);
            Logger.Info($"MQ SSL Enbaled with settings: CipherSpec:{mqCipherSpec}, keystore:{keyStore}, cert friendly name: {certFriendlyName}.");
        }

    }
}