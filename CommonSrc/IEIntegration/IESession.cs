using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using IE.CommonSrc.Configuration;

namespace IE.CommonSrc.IEIntegration
{
    public class IESession
    {
        private HttpClient _httpClient;
        private CookieContainer _cookies;
        private ILogging _logger;

        private const string LOGIN_URL = "https://www.illicitencounters.com/auth/login/finalUrl";
        private const string LOGOUT_URL = "https://www.illicitencounters.com/auth/logout";
        private const string ONLINE_URL_START = "https://www.illicitencounters.com/search/results/mode/online/gender/";
        private const string SEARCH_URL = "https://www.illicitencounters.com/search/results/page/";

		public IESession(ILogging logger)
        {
            _logger = logger;
            LoggedIn = false;

            _cookies = new CookieContainer();
            _httpClient = new HttpClient(new HttpClientHandler
            {
                AllowAutoRedirect = false,
                CookieContainer = _cookies,
                UseCookies = true
            });
        }

        public async Task<List<IEProfile>> Search(bool female, int minAge, int maxAge, string counties, int maxProfiles = 50)
        {
			List<IEProfile> resultList = new List<IEProfile>();

            List<KeyValuePair<string, string>> rawContent = new List<KeyValuePair<string, string>>();
			rawContent.Add(new KeyValuePair<string, string>("gender", female ? "Female" : "Male"));
			rawContent.Add(new KeyValuePair<string, string>("ageFrom", "" + minAge ));
			rawContent.Add(new KeyValuePair<string, string>("ageTo", "" + maxAge));
			rawContent.Add(new KeyValuePair<string, string>("OrderBy", "last_login"));
			rawContent.Add(new KeyValuePair<string, string>("search_submit", "1"));

			string[] countyArray = counties.Split(',');

			foreach (string county in countyArray)
			{
                rawContent.Add(new KeyValuePair<string, string>("county-selectedValues[]", county));
			}

			var content = new FormUrlEncodedContent(rawContent);
			


			int currentPage = 1;
			do
			{
				var reply = await _httpClient.PostAsync(SEARCH_URL + currentPage, content).ConfigureAwait(false);
                if (reply.StatusCode == HttpStatusCode.OK)
                {
                    var replyPage = await reply.Content.ReadAsStringAsync();

                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(replyPage);


                    parseResults(htmlDoc, resultList);

    				var nextPage = htmlDoc.DocumentNode.Descendants("a").Where(x => x.Attributes.Contains("title") && x.Attributes["title"].Value == "next");
    				if ((nextPage == null) || (nextPage.Any() == false))
    				{
    					maxProfiles = -1;                       // Force an end as we can't find next button...
    				}
    				else
    				{
    					// We have found the next button....incremenet our page
    					currentPage++;
    				}
    			}
    			else
    			{
    				_logger.LogError("Unexpected status code returned " + reply.StatusCode);
    				maxProfiles = -1;
    			}
    		} while (resultList.Count() < maxProfiles);

            return resultList;
		}

	    public async Task<List<IEProfile>> OnLine( bool female, int minAge, int maxAge, string counties, int maxProfiles = 50 )
        {
            string searchUrl = ONLINE_URL_START;
            searchUrl += female ? "Female" : "Male";
            searchUrl += "/ageFrom/";
            searchUrl += minAge;
            searchUrl += "/ageTo/";
            searchUrl += maxAge;
            searchUrl += "/county/";
            searchUrl += counties;
            searchUrl += "/OrderBy/last_login/online/1/filter/1";

            List<IEProfile> resultList = new List<IEProfile>();

            int currentPage = 1;

            do
            {
                var reply = await _httpClient.GetAsync(searchUrl).ConfigureAwait(false);
                if (reply.StatusCode == HttpStatusCode.OK)
                {
                    var replyPage = await reply.Content.ReadAsStringAsync();

                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(replyPage);


                    parseResults(htmlDoc, resultList);

                    //_logger.LogInfo("replyPage=" + replyPage);

                    var nextPage = htmlDoc.DocumentNode.Descendants("a").Where(x => x.Attributes.Contains("title") && x.Attributes["title"].Value == "next");
                    if (( nextPage == null ) || (nextPage.Any() == false ))
                    {
                        maxProfiles = -1;                       // Force an end as we can't find next button...
                    }
                    else
                    {
                        // We have found the next button....incremenet our page
                        currentPage++;
						searchUrl = ONLINE_URL_START;
						searchUrl += female ? "Female" : "Male";
						searchUrl += "/ageFrom/";
						searchUrl += minAge;
						searchUrl += "/ageTo/";
						searchUrl += maxAge;
						searchUrl += "/county/";
						searchUrl += counties;
						searchUrl += "/OrderBy/last_login/online/1/page/";
                        searchUrl += currentPage;
					}
                }
                else
                {
                    _logger.LogError("Unexpected status code returned " + reply.StatusCode);
                    maxProfiles = -1;
                }
            } while (resultList.Count() < maxProfiles);

            return resultList;
        }

        private void parseResults(HtmlDocument htmlDoc, List<IEProfile> resultList) 
        {
			var container = htmlDoc.DocumentNode.Descendants("div").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "result-item");
			if (container != null && container.Count() > 0)
			{
				foreach (var item in container)
				{
					string profileName = null;
					string profileUrl = null;
					string partialSummary = null;
					string imageUrl = null;
					string location = null;
					string age = null;

					var nameSpan = item.Descendants("span").Where(x => x.Attributes.Contains("class") && (x.Attributes["class"].Value == "linkbox" || x.Attributes["class"].Value == "linkboxvisited"));
					if (nameSpan != null)
					{
						profileName = nameSpan.Select(x => x.Descendants("a")).FirstOrDefault().FirstOrDefault().InnerText;
						profileName = profileName.Trim();
						profileUrl = nameSpan.Select(x => x.Descendants("a")).FirstOrDefault().FirstOrDefault().Attributes["href"].Value;

						_logger.LogInfo("Name = '" + profileName + "'");
						//_logger.LogInfo("URL = '" + profileUrl + "'");
					}
					var text = item.Descendants("p").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "partial-text");
					partialSummary = text.FirstOrDefault().InnerHtml.Trim();
					//_logger.LogInfo("Text=" + partialSummary);

					var imageA = item.Descendants("a").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "highslide");
					if (imageA != null)
					{
						var imageB = imageA.Select(x => x.Descendants("img"));//.FirstOrDefault().FirstOrDefault().Attributes["src"];
						if (imageB != null)
						{
							var imageC = imageB.FirstOrDefault();
							if (imageC != null)
							{
								var imageD = imageC.FirstOrDefault();
								if (imageD != null)
								{
									imageUrl = imageD.Attributes["src"].Value;
								}
							}
						}
					}
					if (imageUrl == null)
					{
						// No profile picture - but we can find the image they selected instead...
						var imageE = item.Descendants("img").Where(x => x.Attributes.Contains("title") && x.Attributes["title"].Value == "No profile picture");
						if (imageE != null)
						{
							imageUrl = "https://www.illicitencounters.com" + imageE.FirstOrDefault().Attributes["src"].Value;
						}
					}

					var divs = item.Descendants("div").Where(x => x.Attributes.Contains("style") && x.Attributes["style"].Value.Contains("margin-left") && x.Attributes["style"].Value.Contains("margin-bottom"));
					if (divs != null)
					{

						char[] delimiterChars = { '&', ';' };
						string[] parts = divs.FirstOrDefault().InnerText.Split(delimiterChars);
						if (parts.Count() > 6)
						{
							age = parts[2];
							location = parts[6];
						}
					}

					if ((profileName != null) && (profileUrl != null))
					{
						IEProfile profile = new IEProfile();
						profile.Name = profileName;
						profile.PartialSummary = partialSummary;
						profile.ThumbnailUrl = imageUrl;
						profile.Location = location;
						profile.Age = age;

						char[] delimiterChars = { '/' };
						string[] parts = profileUrl.Split(delimiterChars);
						if ((parts != null) && (parts.Count() > 5))
						{
							profile.ProfileId = parts[4];
							resultList.Add(profile);
						}
					}
				}
			}
		}

        public async Task<bool> Login( string username, string password ) 
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "username_login", username },
                { "password_login", password }
            });
            var loginReply = await _httpClient.PostAsync(LOGIN_URL, content).ConfigureAwait(false);
            if ( loginReply.StatusCode == HttpStatusCode.Redirect ) {
                // So far - so good....
                //var replyPage = await loginReply.Content.ReadAsStringAsync();
                //_logger.LogInfo( "replyPage=" + replyPage);

                //if (replyPage.IndexOf("text-error", StringComparison.CurrentCulture) < 0 )
                //{
					LoggedIn = true;                    // No errors found....
				//}
                return LoggedIn;
            }
            else
            {
                _logger.LogError("Unexpected status code returned " + loginReply.StatusCode);
            }
            return false;
        }

        public async Task<bool> Logout()
        {
            var reply = await _httpClient.GetAsync(LOGOUT_URL).ConfigureAwait(false);
            if (reply.StatusCode == HttpStatusCode.Redirect)
            {
                LoggedIn = false;
            }
            return LoggedIn;
        }

        public bool LoggedIn { get; private set; }
    }
}
