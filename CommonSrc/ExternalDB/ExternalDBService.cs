using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IE.CommonSrc.Configuration;
using IE.CommonSrc.IEIntegration;
using IE.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xamarin.Forms;

namespace IE.CommonSrc.ExternalDB
{
    public class ExternalDBService
    {
        private const string FETCH_ALL_URL = "http://www.dinokits.co.uk//IE/IEListMembers.php";
		private const string ADD_NEW_MEMBER_URL = "http://www.dinokits.co.uk/IE/IEAddMember.php";
		private const string MODIFY_MEMBER_URL = "http://www.dinokits.co.uk/IE/IEModifyMember.php";
		private readonly ILogging _logger;

		public ExternalDBService()
        {
			_logger = DependencyService.Get<ILogging>();
		}

        public async Task<IEMember> AddNewMember(string myUsername, IEMember member) {
			ExternalDBCommand cmd = new ExternalDBCommand();
			cmd.requestingUsername = myUsername;
            cmd.member = member;

			var dateTimeConverter = new IsoDateTimeConverter();
            string json = JsonConvert.SerializeObject(cmd, dateTimeConverter);

			var httpClient = new HttpClient();
			HttpContent contents = new StringContent(json, Encoding.UTF8, "application/json");

			HttpResponseMessage resp = await httpClient.PostAsync(ADD_NEW_MEMBER_URL, contents);
			if ((resp != null) && (resp.IsSuccessStatusCode))
			{
				//
				// We need to parse the results - somehow convert back into an array of IEMembers....
				//
				string jsonReply = await resp.Content.ReadAsStringAsync();

                IEMember replyMember = JsonConvert.DeserializeObject<IEMember>(jsonReply, dateTimeConverter);
                return replyMember;
			}
            return null;
		}

        public async Task<IEMember> ModifyMember(string myUsername, IEMember member)
		{
			ExternalDBCommand cmd = new ExternalDBCommand();
			cmd.requestingUsername = myUsername;
			cmd.member = member;

			var dateTimeConverter = new IsoDateTimeConverter();
			string json = JsonConvert.SerializeObject(cmd, dateTimeConverter);

			var httpClient = new HttpClient();
			HttpContent contents = new StringContent(json, Encoding.UTF8, "application/json");

			HttpResponseMessage resp = await httpClient.PostAsync(MODIFY_MEMBER_URL, contents);
			if ((resp != null) && (resp.IsSuccessStatusCode))
			{
				//
				// We need to parse the results - somehow convert back into an array of IEMembers....
				//
				string jsonReply = await resp.Content.ReadAsStringAsync();

                _logger.LogInfo("ModifyMember reply= " + jsonReply );

                try
                {
                    IEMember replyMember = JsonConvert.DeserializeObject<IEMember>(jsonReply, dateTimeConverter);
                    return replyMember;
                } catch(Exception e) {
                    _logger.LogError("ModifyMember error=" + e.Message);
                    _logger.LogError("Stack=" + e.StackTrace );
                }
			}
			return null;
		}

		public async Task<List<IEMember>> FetchAllMembers(string myUsername) {
            ExternalDBCommand cmd = new ExternalDBCommand();
            cmd.requestingUsername = myUsername;

            string json = JsonConvert.SerializeObject(cmd);

            var httpClient = new HttpClient();
            HttpContent contents = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage resp = await httpClient.PostAsync(FETCH_ALL_URL, contents);
            if (( resp != null ) && ( resp.IsSuccessStatusCode )) {
				//
				// We need to parse the results - somehow convert back into an array of IEMembers....
				//
				var dateTimeConverter = new IsoDateTimeConverter();

				string jsonReply = await resp.Content.ReadAsStringAsync();
                ExternalDBReply reply = JsonConvert.DeserializeObject<ExternalDBReply>( jsonReply, dateTimeConverter );

                return reply.members.ToList<IEMember>();
            }
            return new List<IEMember>();
        }
    }
}
