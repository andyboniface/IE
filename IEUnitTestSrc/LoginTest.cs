using System;
using System.Threading.Tasks;
using IE.CommonSrc.Configuration;
using IE.CommonSrc.IEIntegration;
using NUnit.Framework;

namespace IEUnitTest.IEUnitTestSrc
{
	[TestFixture]
    public class LoginTest : ILogging
    {
		//[Test]
		public async Task RunLoginTest()
        {
            IESession session = new IESession(this);
			Assert.False(session.LoggedIn, "State of LoggedIn on IESession was not the expected value");

			bool result = await session.Login("we should be together", "twenty1%fat");
			Assert.True(result, "Result result of login was not the expected value");
			Assert.True(session.LoggedIn, "State of LoggedIn on IESession was not the expected value");


            var searchResults = await session.MatchFinder(true, 20, 99, "33,50");

            result = await session.Logout();
			Assert.False(session.LoggedIn, "State of LoggedIn on IESession was not the expected value");

            LogInfo( "Found " + searchResults.Count + " results");

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
