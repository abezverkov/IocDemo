using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IocDemoConsole
{
    public class DefaultConfigManager
    {
        public string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public string GetConnectionString(string key)
        {
            var collection = ConfigurationManager.ConnectionStrings[key];
            return collection == null ? null : collection.ConnectionString;
        }
    }
}
