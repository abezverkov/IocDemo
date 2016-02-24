using System;
using System.Collections.Generic;
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

            var logFile = new Mock<ILogger>();
            var service = new Mock<IDemoWebService>();
            service.Setup(s => s.GetDLPoints(It.IsAny<string>()))
                .Returns(new DemoWebResponse()
                {

                });
            var dbHelper = new Mock<IDemoDbHelper>();
            var config = new Mock<IConfigManager>();
            config.Setup(c => c.GetAppSetting("Enabled")).Returns("yes");

            var formatter = new Mock<IOutputFormatter>();

            var worker = new Worker(logFile.Object, service.Object, dbHelper.Object, config.Object, formatter.Object);

            // Act
            var actual = worker.DoSomeStuff(dl);

            // Assert
            service.Verify(s => s.GetDLPoints(dl));
            dbHelper.Verify(d => d.GetNewPoints(dl, It.IsAny<int>()));
            formatter.Verify(f => f.Format(It.IsAny<List<DemoDbResponse>>()));
        }
    }
}
