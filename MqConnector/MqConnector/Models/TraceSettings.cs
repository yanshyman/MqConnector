using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MqConnector.Models
{
    public class TraceSettings : ConfigurationSection
    { 
        [ConfigurationProperty("MQTRACELEVEL")]
        public MQTRACELEVEL MQTRACELEVEL
        {
            get { return base["MQTRACELEVEL"] as MQTRACELEVEL; }
            set { base["MQTRACELEVEL"] = value; }
        }

        [ConfigurationProperty("MQTRACEPATH")]
        public MQTRACEPATH MQTRACEPATH => base["MQTRACEPATH"] as MQTRACEPATH;

        [ConfigurationProperty("MQERRORPATH")]
        public MQERRORPATH MQERRORPATH => base["MQERRORPATH"] as MQERRORPATH;
    }

    public class MQERRORPATH : ConfigurationElement
    {
        protected override void DeserializeElement(XmlReader reader, bool s)
        {
            Value = reader.ReadElementContentAs(typeof(string), null) as string;
        }
        public string Value { get; private set; }
    }


    public class MQTRACEPATH : ConfigurationElement
    {
        protected override void DeserializeElement(XmlReader reader, bool s)
        {
            Value = reader.ReadElementContentAs(typeof(string), null) as string;
        }

        public string Value { get; private set; }
    }

    public class MQTRACELEVEL : ConfigurationElement
    {
        protected override void DeserializeElement(XmlReader reader, bool s)
        {
            Value = reader.ReadElementContentAs(typeof(string), null) as string;
        }

        public string Value { get; set; }

    }

}

