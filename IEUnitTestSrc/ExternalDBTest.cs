using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IE.CommonSrc.ExternalDB;
using IE.CommonSrc.IEIntegration;
using NUnit.Framework;

namespace IEUnitTest.IEUnitTestSrc
{
    [TestFixture]
    public class ExternalDBTest
    {
        [Test]
        public async Task RunExternalDBTest()
        {
            ExternalDBService dbService = new ExternalDBService();

            List<IEMember> members = await dbService.FetchAllMembers("kiss me slowly");

            Console.WriteLine("Found " + members.Count + " members in externalDB");

            foreach(var member in members) {
				Console.WriteLine("Last Activity = " + member.LastActivityTimestamp);
				Console.WriteLine("First Activity = " + member.FirstContactTimestamp);
                Console.WriteLine("Extra Activity = " + member.FetchedExtraData);
			}
            Assert.True( members.Count != 0);
        }

		[Test]
		public async Task RunExternalDBTest2()
		{
			ExternalDBService dbService = new ExternalDBService();

            IEMember member = new IEMember();
            member.ProfileId = "8888888";
            member.Username = "Username";
            member.EyeColour = "Blue";

            IEMember reply = await dbService.AddNewMember("kiss me slowly", member);

			Console.WriteLine("Reply Found " + reply);
		}

	}
}
