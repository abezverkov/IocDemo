﻿using System;
using System.Linq;
using StructureMap;
using StructureMap.Graph;

namespace IocDemoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container(new IocRegistry());
            //var logger = new FileLogger("Bob.log");
            //container.Configure(x => x.For<ILogger>().Use(logger));
            
            var worker = container.GetInstance<Worker>();

            string dlNumber = "12345678";
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
