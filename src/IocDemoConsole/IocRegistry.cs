using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;
using StructureMap.Graph;

namespace IocDemoConsole
{
    public class IocRegistry: Registry
    {
        public IocRegistry()
        {
            Scan(s => {
                s.TheCallingAssembly();
                s.WithDefaultConventions();
            });
            For<IOutputFormatter>().Use<StringOutput>();
            For<ILogger>().Use<FileLogger>().Ctor<string>().Is(string.Format("IocDemo_{0:yyyyMMdd}.log", DateTime.Now));
            For<IConfigManager>().Use<DefaultConfigManager>();
        }
    }
}
