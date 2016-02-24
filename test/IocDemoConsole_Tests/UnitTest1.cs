using System;
using IocDemoConsole;
using Moq;
using NUnit.Framework;

namespace IocDemoConsole_Tests
{
    [TestFixture]
    public class UnitTest1
    {
        [Test, Ignore]
        public void TestMethod1()
        {
            // Arrange
            var dl = "00000000";
            var expected = "";
            var logFile = new FileLogger(string.Format("IocDemo_{0:yyyyMMdd}.log", DateTime.Now));
            var service = new DemoWebService(logFile);
            IDemoDbHelper dbHelper = new DemoDbHelper();
            var config = new DefaultConfigManager();
            var formatter = new StringOutput(logFile);
            var worker = new Worker(logFile, service, dbHelper, config, formatter);

            // Act
            var actual = worker.DoSomeStuff(dl);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
