using System;
using System.Threading.Tasks;
using IE.CommonSrc.Configuration;
using IE.CommonSrc.IEIntegration;
using NUnit.Framework;

namespace IEUnitTest.IEUnitTestSrc
{
	[TestFixture]
	public class ProfileTest : ILogging
	{
        //[Test]
        public async Task RunProfileTest()
		{
			IESession session = new IESession(this);
			Assert.False(session.LoggedIn, "State of LoggedIn on IESession was not the expected value");

			bool result = await session.Login("we should be together", "twenty1%fat");
			Assert.True(result, "Result result of login was not the expected value");
			Assert.True(session.LoggedIn, "State of LoggedIn on IESession was not the expected value");


            //var searchResults = await session.Search(true, 20, 99, "33,50");
            IEMember member = new IEMember()
            {
                ProfileId = "1145973"
            };
            var profileResults = await session.GetProfile(member);
			result = await session.Logout();
			Assert.False(session.LoggedIn, "State of LoggedIn on IESession was not the expected value");

			return;
		}

		public void LogError(string msg)
		{
			Console.WriteLine("Error:" + msg);
		}

		public void LogWarning(string msg)
		{
			Console.WriteLine("Warn:" + msg);
		}

		public void LogInfo(string msg)
		{
			Console.WriteLine("Info:" + msg);
		}


	}
}
