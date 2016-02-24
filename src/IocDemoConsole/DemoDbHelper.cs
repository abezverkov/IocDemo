using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IocDemoConsole
{
    public class DemoDbResponse
    {
        public string DLNumber { get; set; }
        public int PersonID { get; set; }
        public int Points { get; set; }
    }

    public class DemoDbHelper
    {
        public static List<DemoDbResponse> GetNewPoints(string dlNumber, int points)
        {
            using (var logFile = new FileLogger(string.Format("IocDemo_{0:yyyyMMdd}.log", DateTime.Now)))
            {
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
                            updateCommand.Parameters.Add("Points", SqlDbType.Int).Value = points;

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
                }
                return dbResponses;
            }
        }
    }
}
