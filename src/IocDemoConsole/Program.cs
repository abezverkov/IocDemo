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
            IDemoDbHelper dbHelper = new DemoDbHelper();

            var worker = new Worker(logFile, service, dbHelper);
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
        private IDemoDbHelper _dbHelper;
        
        public Worker(FileLogger logFile, DemoWebService service, IDemoDbHelper helper)
        {
            _logFile = logFile;
            _service = service;
            _dbHelper = helper;
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
            var dbResponses = _dbHelper.GetNewPoints(dlNumber, webResponse.Points);

            // Output the data
            return new StringOutput().Format(dbResponses);
        }
    }
}
