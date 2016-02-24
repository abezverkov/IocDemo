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

            string output = Worker.DoSomeStuff(dlNumber);
            Console.WriteLine(output);


            Console.WriteLine("Press any key.");
            Console.Read();
        }
    }

    public static class Worker
    {
        public static string DoSomeStuff(string dlNumber)
        {
            var logFile = new FileLogger(string.Format("IocDemo_{0:yyyyMMdd}.log", DateTime.Now));
            // Get some config data
            var enabled = ConfigurationManager.AppSettings["Enabled"];
            if (enabled != "yes")
            {
                logFile.WriteLine("Feature is not enabled. Returning control to caller");
                return "Feature not enabled";
            }

            // Pull some information from a service
            var service = new DemoWebService();
            DemoWebResponse webResponse = service.GetDLPoints(dlNumber);

            // Update/Pull some data from the DB
            var dbResponses = DemoDbHelper.GetNewPoints(dlNumber, webResponse.Points);

            // Output the data
            return new StringOutput().Format(dbResponses);
        }
    }
}
