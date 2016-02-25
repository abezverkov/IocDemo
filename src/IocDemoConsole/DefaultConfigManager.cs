using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IocDemoConsole
{
    public interface IConfigManager
    {
        string GetAppSetting(string key);
        string GetConnectionString(string key);
    }

    public class DefaultConfigManager : IConfigManager
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

    public class DatabaseConfigManager : IConfigManager
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
