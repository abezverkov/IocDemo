using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IocDemoConsole
{
    public class StringOutput
    {
        public string Format(List<DemoDbResponse> dbResponses)
        {
            var logFile = new FileLogger(string.Format("IocDemo_{0:yyyyMMdd}.log", DateTime.Now));
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
