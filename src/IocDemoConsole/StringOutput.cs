using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IocDemoConsole
{
    public interface IOutputFormatter
    {
        string Format(List<DemoDbResponse> dbResponses);
    }

    public class HtmlFormatter : IOutputFormatter
    {
        public string Format(List<DemoDbResponse> dbResponses)
        {
            var sb = new StringBuilder(4096);
            sb.AppendLine(@"<div><p>");
            sb.AppendLine(@"========================");
            sb.AppendLine(@"<h2>These are my responses:</h2>");
            sb.AppendLine(@"========================");
            sb.AppendLine(@"</p></div><ul>");
            for (int i = 0; i < dbResponses.Count; i++)
            {
                var dbresponse = dbResponses[i];
                sb.AppendLine(string.Format("<li>{0} - PersonID:{1}  NewPoints:{2}</li>", i, dbresponse.PersonID, dbresponse.Points));
            }
            sb.AppendLine(@"</ul>");
            return sb.ToString();
        }
    }

    public class StringOutput : IOutputFormatter
    {
        private ILogger _logger;
        
        public StringOutput(ILogger logger)
        {
            _logger = logger;
        }
        
        public string Format(List<DemoDbResponse> dbResponses)
        {
            var sb = new StringBuilder(4096);
            sb.AppendLine("========================");
            sb.AppendLine("These are my responses:");
            sb.AppendLine("========================");
            for (int i = 0; i < dbResponses.Count; i++)
            {
                var dbresponse = dbResponses[i];
                sb.AppendLine(string.Format("{0} - PersonID:{1}  NewPoints:{2}", i, dbresponse.PersonID, dbresponse.Points));
            }

            _logger.WriteLine("Success");
            return sb.ToString();
        }
    }
}
