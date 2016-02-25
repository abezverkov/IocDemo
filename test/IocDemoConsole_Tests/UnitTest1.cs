using System;
using System.Collections.Generic;
using IocDemoConsole;
using Moq;
using NUnit.Framework;
using StructureMap;
using StructureMap.AutoMocking.Moq;
using StructureMap.Graph;
using StructureMap.Pipeline;

namespace IocDemoConsole_Tests
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void TestDiContainer()
        {
            var container = new Container(new IocRegistry());
            container.AssertConfigurationIsValid();

            Assert.IsInstanceOf<SingletonLifecycle>(container.Model.For<ILogger>().Default.Lifecycle);
        }

        [Test]
        public void TestWorkerDi()
        {
            var container = new Container(new IocRegistry());
            var worker = container.GetInstance<Worker>();
        }

        [Test]
        [TestCase(null, "yes")]
        [TestCase("", "yes")]
        [TestCase("00000000", "yes")]
        [TestCase("12345678", "yes")]
        public void TestWorker(string dl, string enabled)
        {
            // Arrange
            var a = new MoqAutoMocker<Worker>();
           

            Mock.Get(a.Get<IDemoWebService>())
                .Setup(s => s.GetDLPoints(It.IsAny<string>()))
                .Returns(new DemoWebResponse()
                {

                });
            Mock.Get(a.Get<IConfigManager>())
                .Setup(c => c.GetAppSetting("Enabled"))
                .Returns(enabled);

            var worker = a.ClassUnderTest;

            // Act
            var actual = worker.DoSomeStuff(dl);

            // Assert
            Mock.Get(a.Get<IDemoWebService>()).Verify(s => s.GetDLPoints(dl));
            Mock.Get(a.Get<IDemoDbHelper>()).Verify(d => d.GetNewPoints(dl, It.IsAny<int>()));
            Mock.Get(a.Get<IOutputFormatter>()).Verify(f => f.Format(It.IsAny<List<DemoDbResponse>>()));

            Mock.Get(a.Get<ILogger>()).Verify(l => l.WriteLine(It.IsAny<string>()), Times.Never);
        }
    }
}
