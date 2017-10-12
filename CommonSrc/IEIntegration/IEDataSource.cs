using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using IE.CommonSrc.Configuration;
using IE.CommonSrc.ExternalDB;
using IE.CommonSrc.Utils;
using IE.Helpers;
using SQLite;
using Xamarin.Forms;

namespace IE.CommonSrc.IEIntegration
{
	static class Extensions
	{
		public static void Sort<T>(this ObservableCollection<T> collection) where T : IComparable
		{
			List<T> sorted = collection.OrderBy(x => x).ToList();
            for (int i = 0; i < sorted.Count(); i++)
            {
                collection.Move(collection.IndexOf(sorted[i]), i);
            }
		}
	}

    public class IEDataSource
    {
        //private SQLiteAsyncConnection _database;
        private readonly ExternalDBService _externalDb;
        private readonly IESession _session;
        private readonly ILogging _logger;
        public event Action OnNewMembersFound;
        private const int INITIAL_DELAY = 5;
        private const int POST_LOGIN_DELAY = 10;
        private const int NO_LOGIN_DETAILS_DELAY = 60;
        private const string DS_TASK_NAME = "IEDataSource";
        private static IEDataSource _instance;

        public static IEDataSource GetInstance()
        {
            if ( _instance == null ) {
                _instance = new IEDataSource();
            }
            return _instance;
        }

        private IEDataSource()
        {
			_logger = DependencyService.Get<ILogging>();

			RejectedItems = new ObservableCollection<IEMember>();
			ActiveItems = new ObservableCollection<IEMember>();
			NewItems = new ObservableCollection<IEMember>();
            SearchResults = new ObservableCollection<IEMember>();

			Members = new Dictionary<string, IEMember>();
            //string dbPath = DependencyService.Get<IFileHelper>().GetLocalFilePath("iedb.db3");
            //_database = new SQLiteAsyncConnection(dbPath);
            //_database.CreateTableAsync<IEMember>( CreateFlags.None ).Wait();
            _externalDb = new ExternalDBService();

            _logger.LogInfo("Reading database table for existing members");

			/*
            var table = _database.Table<IEMember>().ToListAsync();
            table.Wait();
            */

			/*
            var table = _externalDb.FetchAllMembers(Settings.UserName);
            table.Wait();

            _logger.LogInfo("Found " + table.Result.Count + " members in databse");
            foreach( var member in table.Result)
            {
                Members.Add(member.ProfileId, member);
                switch(member.Status) 
                {
                    case IEMember.MEMBER_NEW:
                        NewItems.Add(member);
                        break;
                    case IEMember.MEMBER_REJECTED:
                        RejectedItems.Add(member);
                        break;
                    default:
                        ActiveItems.Add(member);
                        break;
                }
            }

            NewItems.Sort();
            RejectedItems.Sort();
            ActiveItems.Sort();
            */

			_session = new IESession(_logger);
            Running = false;
        }

        private async Task<TimeSpan> LoadFromExternalDatabase()
        {
			var table = await _externalDb.FetchAllMembers(Settings.UserName);

			_logger.LogInfo("Found " + table.Count + " members in databse");
			foreach (var member in table)
			{
				Members.Add(member.ProfileId, member);
				switch (member.Status)
				{
					case IEMember.MEMBER_NEW:
						NewItems.Add(member);
						break;
					case IEMember.MEMBER_REJECTED:
						RejectedItems.Add(member);
						break;
					default:
						ActiveItems.Add(member);
						break;
				}
			}

			NewItems.Sort();
			RejectedItems.Sort();
			ActiveItems.Sort();

			TimeScheduler.GetTimeScheduler().AddTask(DS_TASK_NAME, TimeSpan.FromSeconds(INITIAL_DELAY), () => OnTimedEvent());
			return TimeScheduler.STOP_TIMER;                // This stops us being re-scheduled
		}


		public bool Running { get; private set; }
        protected Dictionary<string, IEMember> Members { get; private set; }

		public ObservableCollection<IEMember> RejectedItems { get; private set; }
		public ObservableCollection<IEMember> ActiveItems { get; private set; }
		public ObservableCollection<IEMember> NewItems { get; private set; }
		public ObservableCollection<IEMember> SearchResults { get; private set; }

		public void StartDataSource() 
        {
			//
			// Start our main scheduler....
			//
			TimeScheduler.GetTimeScheduler().AddTask(DS_TASK_NAME, TimeSpan.FromSeconds(INITIAL_DELAY), () => LoadFromExternalDatabase());
            Running = true;
		}


        public void StopDataSource() 
        {
            TimeScheduler.GetTimeScheduler().RemoveTask(DS_TASK_NAME);
            Running = false;
        }

        public void ChangeMemberStatus(IEMember member, int newStatus) {
            switch(member.Status)
            {
                case IEMember.MEMBER_NEW:
                    NewItems.Remove(member);
                    break;
                case IEMember.MEMBER_REJECTED:
                    RejectedItems.Remove(member);
                    break;
                default:
                    ActiveItems.Remove(member);
                    break;
            }
            member.Status = newStatus;

			switch (member.Status)
			{
				case IEMember.MEMBER_NEW:
					NewItems.Add(member);
					break;
				case IEMember.MEMBER_REJECTED:
					RejectedItems.Add(member);
					break;
				default:
					ActiveItems.Add(member);
					break;
			}

			NewItems.Sort();
			RejectedItems.Sort();
			ActiveItems.Sort();

            SaveMember(member);
        }

        public async void SaveMember(IEMember member) {

            _logger.LogInfo("Saving member " + member.Id + " profile=" + member.Username + " Status=" + member.Status);

            if ( member.Id == 0 ) {
                IEMember newMember = await _externalDb.AddNewMember(Settings.UserName, member);
                if (newMember != null)
                {

                    member.Id = newMember.Id;
                    Members.Add(member.ProfileId, member);
                }
            }
            else
            {
                IEMember newMember = await _externalDb.ModifyMember(Settings.UserName, member);
				if (Members.TryGetValue(member.ProfileId, out IEMember prevMember))
				{
					Members.Remove(member.ProfileId);
				}
				Members.Add(member.ProfileId, member);
			}
            /*
            if ( member.Id == 0 ) 
            {
                _database.InsertAsync(member).Wait(); 
                Members.Add(member.ProfileId, member);
            }
            else
            {
                _database.UpdateAsync(member).Wait();
                if ( Members.TryGetValue(member.ProfileId, out IEMember prevMember)) {
                    Members.Remove(member.ProfileId);
                }
                Members.Add(member.ProfileId, member);
            }
            */
        }

		public void DoNameSearch(string username)
		{
			TimeScheduler.GetTimeScheduler().AddTask(DS_TASK_NAME + username, TimeSpan.FromSeconds(0), () => InternalDoNameSearch(username));
		}
		
        private async Task<TimeSpan> InternalDoNameSearch(string username)
		{

            if (Settings.LoginDetailsSupplied())
            {
                // Not logged in currently - we should try to login now....
                bool state = await _session.Login(Settings.UserName, Settings.Password);
                if (state)
                {
                    SearchResults.Clear();
                    List<IEProfile> profiles = await _session.SearchByName(Settings.SearchForFemales, username, Settings.FetchCount);

                    _logger.LogInfo("Found " + profiles.Count() + " results for search of " + username);
					foreach (var profile in profiles)
					{
						// Do we know this profile already?
						IEMember member = GetMember(profile.ProfileId);
						if (member == null)
						{
							// We should add them....
							member = new IEMember()
							{
								ProfileId = profile.ProfileId,
								Region = profile.Location,
								Status = IEMember.MEMBER_NEW
							};
						}
						member.Username = profile.Name;
						member.Age = profile.Age;
						member.ThumbnailUrl = profile.ThumbnailUrl;
						member.PartialSummary = profile.PartialSummary;
                        SearchResults.Add(member);
                    }
					await _session.Logout();
                    SearchResults.Sort();

				}
            }
			return TimeScheduler.STOP_TIMER;                // This stops us being re-scheduled
		}

		public void GetMemberProfile(IEMember member) {
            TimeScheduler.GetTimeScheduler().AddTask(DS_TASK_NAME + member.ProfileId, TimeSpan.FromSeconds(0), () => InternalGetMemberProfile(member));
		}

        private async Task<TimeSpan> InternalGetMemberProfile(IEMember member) {

			if ((member.FetchedExtraData == null) || ((DateTime.Now - member.FetchedExtraData).TotalDays > 500))
		    //if (member.FetchedExtraData == null)
			{
                // Never got extra profile data - need to fetch it...or it was over 10 days ago...
                if (Settings.LoginDetailsSupplied())
                {
                    // Not logged in currently - we should try to login now....
                    bool state = await _session.Login(Settings.UserName, Settings.Password);
                    if ( state ) {
                        await _session.GetProfile(member);

                        await _session.Logout();
                        SaveMember(member);                                     // Remember the changes
                        member.FirePropertyChangeEvent();                       // Tell anyone who cares
                    }
                }
            }
            return TimeScheduler.STOP_TIMER;                // This stops us being re-scheduled
        }

        public IEMember GetMember(string profileId)
        {
            if (Members.TryGetValue(profileId, out IEMember member))
            {
                return member;
            }
            return null;
        }

        public async Task<TimeSpan> OnTimedEvent() 
        {

            if ( _session.LoggedIn ) 
            {
                // We are currently logged in - therefore we should try to pull in some useful data - looking for new people
                //
                await DoMatchFinder(false);

                // Now we have everything we need - we should logoff.
                //
                await _session.Logout();
            }
            else
            {
                if (Settings.LoginDetailsSupplied())
                {
                    // Not logged in currently - we should try to login now....
                    bool state = await _session.Login(Settings.UserName, Settings.Password);
                    if (state)
                    {
                        // This means we are now logged in - we want to come back to the timed event quicker - rather than
                        // wait for the polling interval again...
                        return TimeSpan.FromSeconds(POST_LOGIN_DELAY);                // Wait 10 seconds - then we can look for some data.
                    }
					return TimeSpan.FromSeconds(NO_LOGIN_DETAILS_DELAY);
				}
                else
                {
                    // We don't have any login details yet - wait a bit and try again...
                    return TimeSpan.FromSeconds(NO_LOGIN_DETAILS_DELAY);
                }
            }
            return TimeSpan.FromMinutes(Settings.PollingRate);
        }

        /*
         * This method is used to search for users who are currently logged in
         */
        private async Task<bool> DoMatchFinder( bool byLogin) 
        {
            List<IEProfile> profiles;
			bool newMemberFound = false;

			if (byLogin)
            {
                profiles = await _session.OnLine(Settings.SearchForFemales, Settings.MinAge, Settings.MinAge, Settings.Regions, Settings.FetchCount);
            }
            else
            {
                profiles = await _session.MatchFinder(Settings.SearchForFemales, Settings.MinAge, Settings.MaxAge, Settings.Regions, Settings.FetchCount);
            }
            if (profiles != null)
            {

                foreach (var profile in profiles)
                {
                    // Do we know this profile already?
                    IEMember member = GetMember(profile.ProfileId);
                    if (member == null)
                    {
                        // We should add them....
                        member = new IEMember()
                        {
                            ProfileId = profile.ProfileId,
							Region = profile.Location,
						    Status = IEMember.MEMBER_NEW
                        };
                        NewItems.Add(member);
						NewItems.Sort();
						newMemberFound = true;
                    }
                    member.Username = profile.Name;
                    member.Age = profile.Age;
                    member.ThumbnailUrl = profile.ThumbnailUrl;
                    member.PartialSummary = profile.PartialSummary;
                    SaveMember(member);
                }

                if (newMemberFound && OnNewMembersFound != null)
                {
                    OnNewMembersFound();
                }
            }

            return newMemberFound;
		}

	}
}
