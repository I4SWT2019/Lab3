using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class Test
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void PassableTest()
        {
            Assert.Pass();
        }
    }
}