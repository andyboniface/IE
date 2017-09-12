using NUnit.Framework;
using System;
using IE.CommonSrc.Configuration;

namespace IEUnitTest
{
    [TestFixture]
    public class RegionsTest
    {
        [Test]
        public void TestCase()
        {
            Regions regions = new Regions();

            Assert.AreNotEqual( regions.Count, 0 );

            Assert.AreEqual( regions.CountyById(50), "Kent");
            Assert.AreEqual( regions.CountyByName("East Sussex"), 33);
        }
    }
}
