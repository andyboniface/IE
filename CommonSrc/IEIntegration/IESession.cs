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
        private const string MATCHFINDER_URL = "https://www.illicitencounters.com/search/results/page/";
        private const string PROFILE_URL = "https://www.illicitencounters.com/member/profile/show/";
        private const string NAMESEARCH_URL = "https://www.illicitencounters.com/search/results/page/";

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

        public async Task<bool> GetProfile(IEMember member)
        {
            var reply = await _httpClient.GetAsync(PROFILE_URL + member.ProfileId);

            if (reply.StatusCode == HttpStatusCode.OK)
            {
                var replyPage = await reply.Content.ReadAsStringAsync();

                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(replyPage);

                var tables = htmlDoc.DocumentNode.Descendants("table").ToList();

                //
                // Table 1 holds info including 'Race', 'Location' and 'Marital Status'
                //
                if ((tables != null) && (tables.Count() > 5))
                {
                    var tds = tables[1].Descendants("td").ToList();
                    if ((tds != null) && (tds.Count() > 16))
                    {
                        int fieldOffset = 14;
                        if ( tds.Count() > 19 ) {
                            // This means we have a 'Message History' shown as well...we need to step over it...
                            fieldOffset += 2;               // Message History adds 2 new fields before the ones we want.
                        }
                        _logger.LogInfo("Found " + tds.Count() + " tds" );
                        var race = tds[fieldOffset++].InnerText.Split(':')[1].Trim();        // 
                        var loc = tds[fieldOffset++].InnerText.Split(':')[1].Trim(); ;         // Location (often includes town)
                        var mStatus = tds[fieldOffset++].InnerText.Split(':')[1].Trim(); ;     // Marital Status

                        _logger.LogInfo("Race = " + race);
                        _logger.LogInfo("Location = " + loc);
                        _logger.LogInfo("Marital Status = " + mStatus);

                        member.Race = race;
                        member.Region = loc;
                        member.MaritalStatus = mStatus;

                    }
                    //
                    // Table 3 holds 'Personal Info' such as Religion, Eye Colour, Hair colour etc
                    //
                    tds = tables[3].Descendants("td").ToList();
                    if ((tds != null) && (tds.Count() > 22))
                    {
                        var religion = tds[2].InnerText.Trim();
                        var eyeColour = tds[4].InnerText.Trim();
                        var drinking = tds[6].InnerText.Trim();
                        var hairColour = tds[8].InnerText.Trim();
                        var smoking = tds[10].InnerText.Trim();
                        var build = tds[12].InnerText.Trim();
                        var height = tds[14].InnerText.Trim();
                        var education = tds[16].InnerText.Trim();
                        var job = tds[18].InnerText.Trim();
                        var interests = tds[22].InnerText.Trim();

                        var subInterests = interests.Split(',');
                        interests = "";
                        foreach( var sub in subInterests ) {
                            if ( interests.Length > 0 ) {
                                interests += ",";
                            }
                            interests += sub.Trim();
                        }

                        _logger.LogInfo("religon = " + religion);
                        _logger.LogInfo("eyeColour = " + eyeColour);
                        _logger.LogInfo("drinking = " + drinking);
                        _logger.LogInfo("hairColour = " + hairColour);
                        _logger.LogInfo("smoking = " + smoking);
                        _logger.LogInfo("build = " + build);
                        _logger.LogInfo("height = " + height);
                        _logger.LogInfo("education = " + education);
                        _logger.LogInfo("job = " + job);
                        _logger.LogInfo("interests = " + interests);

                        member.Religion = religion;
                        member.EyeColour = eyeColour;
                        member.Drinking = drinking;
                        member.Smoking = smoking;
                        member.HairColour = hairColour;
                        member.Build = build;
                        member.Height = height;
                        member.Education = education;
                        member.Occupation = job;
                        member.Interests = interests;

                    }
                    //
                    // Table 4 is the 'Ideal partner' info.
                    //

                    tds = tables[4].Descendants("td").ToList();
                    if ((tds != null) && (tds.Count() > 3))
                    {
						var lookingFor = tds[1].InnerText.Trim();
						var ideal = tds[2].InnerText.Trim();
						var relationshipType = tds[3].InnerText.Trim();

                        _logger.LogInfo("orig-relationshipType = '" + relationshipType + "'");

                        const string RELATION_TYPE = "Type of Relationship";
						if (relationshipType.StartsWith(RELATION_TYPE, StringComparison.CurrentCulture))
						{
							relationshipType = relationshipType.Substring(RELATION_TYPE.Length).Trim();
						}

						var relationShipSub = relationshipType.Split(',');

                        if (relationShipSub.Count() > 0)
                        {
                            relationshipType = "";
                            foreach (var sub in relationShipSub)
                            {
                                if (relationshipType.Length > 0)
                                {
                                    relationshipType += ",";
                                }
                                relationshipType += sub.Trim();
                            }
                        }

                        const string LOOKING_FOR = "I'm looking for someone who is:";
                        if (lookingFor.StartsWith(LOOKING_FOR, StringComparison.CurrentCulture))
						{
                            lookingFor = lookingFor.Substring(LOOKING_FOR.Length).Trim();
						}

						_logger.LogInfo("lookingFor = " + lookingFor);
						_logger.LogInfo("ideal = " + ideal);
						_logger.LogInfo("relationshipType = " + relationshipType);

                        member.LookingFor = lookingFor;
                        member.IdealPartner = ideal;
                        member.RelationshipType = relationshipType;
					}
					//
					// Table 5 is the About me info
					//
					tds = tables[5].Descendants("td").ToList();
					if ((tds != null) && (tds.Count() > 2))
					{
						var about = tds[1].InnerText.Trim();

						_logger.LogInfo("about = " + about);
                        member.About = about;
					}
                    member.FetchedExtraData = DateTime.Now;
				}

                return true;
            }

            return false;
        }

		public async Task<List<IEProfile>> SearchByName(bool female, string username, int maxProfiles = 50)
		{
			List<IEProfile> resultList = new List<IEProfile>();

			List<KeyValuePair<string, string>> rawContent = new List<KeyValuePair<string, string>>
			{
                new KeyValuePair<string, string>("UserName", username),
 				new KeyValuePair<string, string>("search_submit", "1")
			};
			var content = new FormUrlEncodedContent(rawContent);

			int currentPage = 1;
			do
			{
				var reply = await _httpClient.PostAsync(NAMESEARCH_URL + currentPage, content).ConfigureAwait(false);
				if (reply.StatusCode == HttpStatusCode.OK)
				{
					var replyPage = await reply.Content.ReadAsStringAsync();

					HtmlDocument htmlDoc = new HtmlDocument();
					htmlDoc.LoadHtml(replyPage);

                    try
                    {
                        ParseResults(htmlDoc, resultList, female, maxProfiles);
                    } catch( Exception e ) {
                        _logger.LogError("SearchByName: Failed to parse results correctly" + e.Message);
                        _logger.LogError(e.StackTrace);
                    }
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
					_logger.LogError("Unexpected status code during search returned " + reply.StatusCode);
					maxProfiles = -1;
				}
			} while (resultList.Count() < maxProfiles);

			return resultList;
		}


		public async Task<List<IEProfile>> MatchFinder(bool female, int minAge, int maxAge, string counties, int maxProfiles = 50)
        {
            List<IEProfile> resultList = new List<IEProfile>();

            List<KeyValuePair<string, string>> rawContent = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("gender", female ? "Female" : "Male"),
                new KeyValuePair<string, string>("ageFrom", "" + minAge),
                new KeyValuePair<string, string>("ageTo", "" + maxAge),
                new KeyValuePair<string, string>("OrderBy", "last_login"),
                new KeyValuePair<string, string>("search_submit", "1")
            };
            string[] countyArray = counties.Split(',');

            foreach (string county in countyArray)
            {
                rawContent.Add(new KeyValuePair<string, string>("county-selectedValues[]", county));
            }

            var content = new FormUrlEncodedContent(rawContent);



            int currentPage = 1;
            do
            {
                var reply = await _httpClient.PostAsync(MATCHFINDER_URL + currentPage, content).ConfigureAwait(false);
                if (reply.StatusCode == HttpStatusCode.OK)
                {
                    var replyPage = await reply.Content.ReadAsStringAsync();

                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(replyPage);


                    ParseResults(htmlDoc, resultList, female, maxProfiles);

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
                    _logger.LogError("Unexpected status code during search returned " + reply.StatusCode);
                    maxProfiles = -1;
                }
            } while (resultList.Count() < maxProfiles);

            return resultList;
        }

        public async Task<List<IEProfile>> OnLine(bool female, int minAge, int maxAge, string counties, int maxProfiles = 50)
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


                    ParseResults(htmlDoc, resultList, female, maxProfiles);

                    //_logger.LogInfo("replyPage=" + replyPage);

                    var nextPage = htmlDoc.DocumentNode.Descendants("a").Where(x => x.Attributes.Contains("title") && x.Attributes["title"].Value == "next");
                    if ((nextPage == null) || (nextPage.Any() == false))
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
                    _logger.LogError("Unexpected status code during online search returned " + reply.StatusCode);
                    maxProfiles = -1;
                }
            } while (resultList.Count() < maxProfiles);

            return resultList;
        }

        private void ParseResults(HtmlDocument htmlDoc, List<IEProfile> resultList, bool searchForFemale, int maxProfiles)
        {
            var container = htmlDoc.DocumentNode.Descendants("div").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "result-item");
            if (container != null && container.Any())
            {
                foreach (var item in container)
                {
                    string profileName = null;
                    string profileUrl = null;
                    string partialSummary = "";
                    string imageUrl = null;// = "https://www.illicitencounters.com/img/avatars/female_Avatar3.jpg";
                    string location = null;
                    string age = null;

                    bool isFemale = true;

                    var maleImage = item.Descendants("img").Where( x => x.Attributes.Contains("src" ) && ( x.Attributes["src"].Value == "/img/ie_lexus/1.gif"));
                    if ((maleImage != null) && (maleImage.Any()))
                    {
                        isFemale = false;
                    }

                    if (isFemale == searchForFemale)
                    {
                        var nameSpan = item.Descendants("span").Where(x => x.Attributes.Contains("class") && (x.Attributes["class"].Value == "linkbox" || x.Attributes["class"].Value == "linkboxvisited"));
                        if (nameSpan != null)
                        {
                            profileName = nameSpan.Select(x => x.Descendants("a")).FirstOrDefault().FirstOrDefault().InnerText;
                            if (profileName != null)
                            {
                                profileName = profileName.Trim();
                                profileUrl = nameSpan.Select(x => x.Descendants("a")).FirstOrDefault().FirstOrDefault().Attributes["href"].Value;

                                _logger.LogInfo("Name = '" + profileName + "'");
                            }
                            else
                            {
                                _logger.LogError("Failed to locate profile name");
                            }
                            //_logger.LogInfo("URL = '" + profileUrl + "'");
                        }
                        var text = item.Descendants("p").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "partial-text");
                        if ((text != null) && ( text.Any()))
                        {
                            partialSummary = text.FirstOrDefault().InnerHtml.Trim();
							//_logger.LogInfo("Text=" + partialSummary);

							_logger.LogInfo("partialSummary = '" + partialSummary + "'");
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
                        }
                        if (imageUrl == null)
                        {
                            // No profile picture - but we can find the image they selected instead...
                            var imageE = item.Descendants("img").Where(x => x.Attributes.Contains("title") && x.Attributes["title"].Value == "No profile picture");
                            if ((imageE != null) && (imageE.Any()))
                            {
                                imageUrl = "https://www.illicitencounters.com" + imageE.FirstOrDefault().Attributes["src"].Value;
							}
							_logger.LogInfo("imageUrl = '" + imageUrl + "'");
                            if ( imageUrl == null ) {
                                imageUrl = "https://www.illicitencounters.com/img/avatars/female_Avatar3.jpg";  // Last resort
                            }
						}

                        var divs = item.Descendants("div").Where(x => x.Attributes.Contains("style") && x.Attributes["style"].Value.Contains("margin-left") && x.Attributes["style"].Value.Contains("margin-bottom"));
                        if (divs != null)
                        {

                            char[] delimiterChars = { '&', ';' };
                            string[] parts = divs.FirstOrDefault().InnerText.Split(delimiterChars);
                            if (( parts != null ) && (parts.Count() > 6))
                            {
                                age = parts[2];
                                location = parts[6];
								_logger.LogInfo("location = '" + location + "'");
							}
                        }

                        if ((profileName != null) && (profileUrl != null) && ( age != null) && (location != null) && ( imageUrl != null))
                        {
                            IEProfile profile = new IEProfile()
                            {
                                Name = profileName.Trim(),
                                PartialSummary = StripHtml(partialSummary.Trim()),
                                ThumbnailUrl = imageUrl.Trim(),
                                Location = location.Trim(),
                                Age = age.Trim()
                            };
                            char[] delimiterChars = { '/' };
                            string[] parts = profileUrl.Split(delimiterChars);
                            if ((parts != null) && (parts.Count() > 5))
                            {
                                profile.ProfileId = parts[4];
                                resultList.Add(profile);

                                if ( resultList.Count() >= maxProfiles ) {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private string StripHtml(string html) {
            return html.Replace("<br>", "\r\n").Replace("\r\n\r\n", "\r\n").Replace("\n\n", "\n");
        }

        public async Task<bool> Login(string username, string password)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "username_login", username },
                { "password_login", password }
            });
            var loginReply = await _httpClient.PostAsync(LOGIN_URL, content).ConfigureAwait(false);
            if (loginReply.StatusCode == HttpStatusCode.Redirect)
            {
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
                _logger.LogError("Unexpected status code during logon returned " + loginReply.StatusCode);
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
