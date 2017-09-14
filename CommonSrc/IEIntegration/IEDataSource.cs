using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IE.CommonSrc.Configuration;
using IE.CommonSrc.Utils;
using IE.Helpers;
using SQLite;
using Xamarin.Forms;

namespace IE.CommonSrc.IEIntegration
{
    public class IEDataSource
    {
        private SQLiteAsyncConnection _database;
        private Dictionary<string, IEMember> _allMembers = new Dictionary<string, IEMember>();
        private IESession _session;
        public event Action OnNewMembersFound;
        private const int INITIAL_DELAY = 5;
        private const int POST_LOGIN_DELAY = 10;

        public IEDataSource()
        {
            string dbPath = DependencyService.Get<IFileHelper>().GetLocalFilePath("iedb.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<IEMember>( CreateFlags.None ).Wait();

            var table = _database.Table<IEMember>().ToListAsync();
            table.Wait();

            foreach( var member in table.Result)
            {
                _allMembers.Add(member.ProfileId, member);
            }

            ILogging logger = DependencyService.Get<ILogging>();
            _session = new IESession(logger);

            //
            // Start our main scheduler....
            //
            TimeScheduler.GetTimeScheduler().AddTask("main", TimeSpan.FromSeconds(INITIAL_DELAY), () => OnTimedEvent());
        }

        public void SaveMember(IEMember member) {
            if ( member.Id == 0 ) 
            {
                _database.InsertAsync(member).Wait(); 
                _allMembers.Add(member.ProfileId, member);
            }
            else
            {
                _database.UpdateAsync(member).Wait();
                IEMember prevMember;

                if ( _allMembers.TryGetValue(member.ProfileId, out prevMember)) {
                    _allMembers.Remove(member.ProfileId);
                }
                _allMembers.Add(member.ProfileId, member);
            }
        }

        public IEMember GetMember(string profileId)
        {
            IEMember member;

            if (_allMembers.TryGetValue(profileId, out member))
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
                DoSearch(false);

                // Now we have everything we need - we should logoff.
                //
                await _session.Logout();
            }
            else
            {
                // Not logged in currently - we should try to login now....
                bool state = await _session.Login(Settings.UserName, Settings.Password);
                if ( state ) {
                    // This means we are now logged in - we want to come back to the timed event quicker - rather than
                    // wait for the polling interval again...
                    return TimeSpan.FromSeconds(POST_LOGIN_DELAY);                // Wait 10 seconds - then we can look for some data.
                }
            }
            return TimeSpan.FromMinutes(Settings.PollingRate);
        }

        /*
         * This method is used to search for users who are currently logged in
         */
        private async void DoSearch( bool byLogin) 
        {
            List<IEProfile> profiles;

            if (byLogin)
            {
                profiles = await _session.OnLine(Settings.SearchForFemales, Settings.MinAge, Settings.MinAge, Settings.Regions);
            }
            else
            {
                profiles = await _session.Search(Settings.SearchForFemales, Settings.MinAge, Settings.MaxAge, Settings.Regions);
            }
            if (profiles != null)
            {
                bool newMemberFound = false;

                foreach (var profile in profiles)
                {
                    // Do we know this profile already?
                    IEMember member = GetMember(profile.ProfileId);
                    if (member == null)
                    {
                        // We should add them....
                        member = new IEMember();
                        member.ProfileId = profile.ProfileId;
                        member.Status = IEMember.MEMBER_NEW;
                        newMemberFound = true;
                    }
                    member.Username = profile.Name;
                    member.Region = profile.Location;
                    member.Age = profile.Age;
                    member.ThumbnailUrl = profile.ThumbnailUrl;
                    member.PartialSummary = profile.PartialSummary;
                    SaveMember(member);
                }

                if (newMemberFound)
                {
                    if (OnNewMembersFound != null)
                    {
                        OnNewMembersFound();
                    }
                }
            }
		}

	}
}
