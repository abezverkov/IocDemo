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

            ILogger logFile = new FileLogger(string.Format("IocDemo_{0:yyyyMMdd}.log", DateTime.Now));
            IDemoWebService service = new DemoWebService(logFile);
            IDemoDbHelper dbHelper = new DemoDbHelper();
            IConfigManager config = new DefaultConfigManager();
            IOutputFormatter formatter = new StringOutput(logFile);

            var worker = new Worker(logFile, service, dbHelper, config, formatter);
            string output = worker.DoSomeStuff(dlNumber);
            Console.WriteLine(output);

            Console.WriteLine("Press any key.");
            Console.Read();
        }
    }

    public class Worker
    {
        private ILogger _logFile;
        private IDemoWebService _service;
        private IDemoDbHelper _dbHelper;
        private IConfigManager _config;
        private IOutputFormatter _formatter;
        
        public Worker(ILogger logFile, IDemoWebService service, IDemoDbHelper helper, IConfigManager config, IOutputFormatter formatter)
        {
            _logFile = logFile;
            _service = service;
            _dbHelper = helper;
            _config = config;
            _formatter = formatter;
        }

        public string DoSomeStuff(string dlNumber)
        {
            // Get some config data
            var enabled = _config.GetAppSetting("Enabled");
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
            return _formatter.Format(dbResponses);
        }
    }
}
