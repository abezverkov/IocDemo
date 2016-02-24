using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IocDemoConsole
{
    public class DemoWebResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Language { get; set; }
        public string DLState { get; set; }
        public string DLNumber { get; set; }
        public int Points { get; set; }
    }

    public interface IDemoWebService
    {
        DemoWebResponse GetDLPoints(string dlNumber);
    }

    public class DemoWebService : IDemoWebService
    {
        private ILogger _logger;
        public DemoWebService(ILogger logger)
        {
            _logger = logger;
        }

        public DemoWebResponse GetDLPoints(string dlNumber)
        {
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
                    _logger.WriteLine(e.Message);
                    return null;
                }
            }
            else
            {
                _logger.WriteLine("No url defined. Returning control to caller");
            }
            return webResponse;

        }
    }
}
