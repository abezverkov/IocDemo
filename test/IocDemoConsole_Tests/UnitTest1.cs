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

            // Act
            var actual = Worker.DoSomeStuff(dl);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
