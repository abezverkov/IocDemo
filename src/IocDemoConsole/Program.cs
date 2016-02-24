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
            using (var logFile = new FileLogger(string.Format("IocDemo_{0:yyyyMMdd}.log", DateTime.Now)))
            {
                // Get some config data
                var enabled = ConfigurationManager.AppSettings["Enabled"];
                if (enabled != "yes")
                {
                    logFile.WriteLine("Feature is not enabled. Returning control to caller");
                    return "Feature not enabled";
                }

                // Pull some information from a service
                DemoWebResponse webResponse = null;
                var requestUrl = ConfigurationManager.AppSettings["ServiceUrl"];
                if (!string.IsNullOrWhiteSpace(requestUrl))
                {
                    try
                    {
                        requestUrl = string.Format(requestUrl, dlNumber);
                        HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                        using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                        {
                            if (response.StatusCode != HttpStatusCode.OK)
                                throw new Exception(String.Format(
                                "Server error (HTTP {0}: {1}).",
                                response.StatusCode,
                                response.StatusDescription));
                            using (var reader = new StreamReader(response.GetResponseStream()))
                            {
                                using (var jreader = new JsonTextReader(reader))
                                {
                                    var serializer = JsonSerializer.CreateDefault();
                                    webResponse = serializer.Deserialize<DemoWebResponse>(jreader);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        logFile.WriteLine(e.Message);
                        return null;
                    }
                }
                else
                {
                    logFile.WriteLine("No url defined. Returning control to caller");
                    return "No url defined";
                }

                // Update/Pull some data from the DB
                var dbResponses = new List<DemoDbResponse>();
                var iocDemoDb = ConfigurationManager.ConnectionStrings["IocDemo"];
                if (iocDemoDb != null)
                {
                    try
                    {
                        using (var connection = new SqlConnection(iocDemoDb.ConnectionString))
                        {
                            var updateCommand = connection.CreateCommand();
                            updateCommand.CommandType = CommandType.StoredProcedure;
                            updateCommand.CommandText = "UpdateDLPoints";
                            updateCommand.Parameters.Add("DLNumber", SqlDbType.VarChar).Value = dlNumber;
                            updateCommand.Parameters.Add("Points", SqlDbType.Int).Value = webResponse.Points;

                            connection.Open();
                            var dbreader = updateCommand.ExecuteReader();
                            while (dbreader.Read())
                            {
                                dbResponses.Add(new DemoDbResponse()
                                {
                                    DLNumber = dbreader[0].ToString(),
                                    PersonID = (int)dbreader[1],
                                    Points = (int)dbreader[2],
                                });

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logFile.WriteLine(ex.ToString());
                        return null;
                    }
                }
                else
                {
                    logFile.WriteLine("No connectionstring defined. Returning control to caller");
                    return "No connectionstring defined";
                }

                // Output the data
                var sb = new StringBuilder(4096);
                sb.AppendLine("========================");
                sb.AppendLine("These are my responses:");
                sb.AppendLine("========================");
                for (int i = 0; i < dbResponses.Count; i++)
                {
                    var dbresponse = dbResponses[i];
                    sb.AppendLine(string.Format("{0} - PersonID:{1}  NewPoints:{2}", i, dbresponse.PersonID, dbresponse.Points));
                }

                logFile.WriteLine("Success");
                return sb.ToString();
            }
        }
    }

    public class DemoWebResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Language { get; set; }
        public string DLState { get; set; }
        public string DLNumber { get; set; }
        public int Points { get; set; }
    }

    public class DemoDbResponse
    {
        public string DLNumber { get; set; }
        public int PersonID { get; set; }
        public int Points { get; set; }
    }
}
