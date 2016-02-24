using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IocDemoConsole
{
    public interface ILogger
    {
        void WriteLine(string message);
    }

    public class FileLogger: ILogger
    {
        private string _filename;
        public FileLogger(string filename)
        {
            _filename = filename;
        }

        public void WriteLine(string message)
        {
            using (var _logFile = File.CreateText(_filename))
            {
                _logFile.WriteLine(message);
            }
        }
    }
}
