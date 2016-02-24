using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IocDemoConsole
{
    public class FileLogger: IDisposable
    {
        private StreamWriter _logFile;
        public FileLogger(string filename)
        {
            _logFile = File.CreateText(filename);
        }

        ~FileLogger()
        {
            this.Dispose(false);
        }

        public void WriteLine(string message)
        {
            _logFile.WriteLine(message);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _logFile.Flush();
                _logFile.Dispose();
            }
        }
    }
}
