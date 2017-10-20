using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using MqConnector.Models;


namespace MqConnector
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = new MqConnectionSettings
            {
                QueueManagerName = args[0],
                ChannelName = args[1],
                Host = args[2],
                Port = int.Parse(args[3]),
                EnableSSL = int.Parse(args[4]) > 0
            };
            var traceDirectory = MqTracing.GetTraceDirectory();
            //traceDirectory.Empty();

            try
            {
                Console.WriteLine(MqClient.TestConnection(settings));
                Console.WriteLine("Press any key to close app...");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message};{e.InnerException?.Message}");
                //Thread.Sleep(5000);//white until trace updated
                //MqTracing.DisplayTrace(traceDirectory);
            }
        }

    }
    }

