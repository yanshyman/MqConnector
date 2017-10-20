using System;
using System.Configuration;
using System.IO;
using MqConnector.Models;

namespace MqConnector
{
  public static  class MqTracing
    {
        public static DirectoryInfo GetTraceDirectory()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var trace = config.Sections["traceSettings"] as TraceSettings;
            return new DirectoryInfo(trace.MQTRACEPATH.Value);
        }
 
        public static void Empty(this DirectoryInfo directory)
        {
            foreach (FileInfo file in directory.GetFiles()) file.Delete();
            foreach (DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }

        public static void DisplayTrace(DirectoryInfo traceDirectory)
        {
            foreach (var file in traceDirectory.GetFiles())
            {
                using (FileStream stream = File.Open(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            Console.Write(reader.ReadToEnd());
                        }
                    }
                }
            }
        }
    }
}
