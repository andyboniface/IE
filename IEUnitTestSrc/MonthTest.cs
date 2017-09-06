using NUnit.Framework;
using System;
using IE.CommonSrc.Configuration;

namespace IEUnitTest
{
    [TestFixture]
    public class MonthTest
    {
        [Test]
        public void TestCase()
        {
            Months months = new Months();

            Assert.AreNotEqual( months.Count, 0 );

            Assert.AreEqual( months.CountyById(50), "Kent");
            Assert.AreEqual( months.CountyByName("East Sussex"), 33);
        }
    }
}
