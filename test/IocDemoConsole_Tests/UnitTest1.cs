using System;
using IocDemoConsole;
using Moq;
using NUnit.Framework;

namespace IocDemoConsole_Tests
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void TestMethod1()
        {
            // Arrange
            var dl = "00000000";
            var expected = "";

            var logFile = new Mock<ILogger>();
            var service = new Mock<IDemoWebService>();
            var dbHelper = new Mock<IDemoDbHelper>();
            var config = new Mock<IConfigManager>();
            var formatter = new Mock<IOutputFormatter>();

            var worker = new Worker(logFile.Object, service.Object, dbHelper.Object, config.Object, formatter.Object);

            // Act
            var actual = worker.DoSomeStuff(dl);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
