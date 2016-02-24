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
            var service = new DemoWebService();
            IDemoDbHelper dbHelper = new DemoDbHelper();
            var worker = new Worker(logFile, service, dbHelper);

            // Act
            var actual = worker.DoSomeStuff(dl);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
