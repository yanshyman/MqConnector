using System;
using System.Reflection;
using IBM.WMQ;
using log4net;


namespace MqConnector
{
    public class MqExceptionHandler
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static bool DirectlyRethrow2033(Exception ex)
        {
            var mqex = ex as MQException;
            if (mqex?.ReasonCode == 2033)
                throw ex;
            return false;
        }

        }
    }
