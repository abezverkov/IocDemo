using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IocDemoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string dlNumber = "12345678";

            var logFile = new FileLogger(string.Format("IocDemo_{0:yyyyMMdd}.log", DateTime.Now));
            var service = new DemoWebService();

            var worker = new Worker(logFile, service);
            string output = worker.DoSomeStuff(dlNumber);
            Console.WriteLine(output);

            Console.WriteLine("Press any key.");
            Console.Read();
        }
    }

    public class Worker
    {
        private FileLogger _logFile;
        private DemoWebService _service;
        
        public Worker(FileLogger logFile, DemoWebService service)
        {
            _logFile = logFile;
            _service = service;
        }

        public string DoSomeStuff(string dlNumber)
        {
            // Get some config data
            var enabled = ConfigurationManager.AppSettings["Enabled"];
            if (enabled != "yes")
            {
                _logFile.WriteLine("Feature is not enabled. Returning control to caller");
                return "Feature not enabled";
            }

            // Pull some information from a service
            DemoWebResponse webResponse = _service.GetDLPoints(dlNumber);

            // Update/Pull some data from the DB
            var dbResponses = DemoDbHelper.GetNewPoints(dlNumber, webResponse.Points);

            // Output the data
            return new StringOutput().Format(dbResponses);
        }
    }
}
