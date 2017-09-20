using System;
using System.Collections.Generic;
using IE.CommonSrc.IEIntegration;
using IE.Helpers;
using Xamarin.Forms;

namespace IE.CommonSrc.Pages
{
    public partial class ProfilePage : ContentPage
    {
        private static ProfilePage _instance;

        public static ProfilePage GetInstance(IEMember member) 
        {
            if ( _instance == null ) {
                _instance = new ProfilePage();
            }
            _instance.Initialise(member);
            return _instance;
        }

        public ProfilePage()
        {
            InitializeComponent();

			if (DesignTimeHelper.DesignModeOn)
			{
                IEMember fakeMember = new IEMember()
                {
                    Username = "fakeuser",
                    About = "Well here goes the complicated part. Have a great sense of humour and also a great outlook on life.... because hey we only get one shot at this, so life's for living and enjoying. I work hard and want to play hard as well...so will you help me???? I love my family they mean more to me than anything, and my friends play an important part in my life as well.  I like nothing more than a cosy nite in snuggled up on the sofa, drinking a glass of bubbly..... and much more besides....                              ",
                    Age = "50",
                    Build = "Average",
                    Drinking = "Light / Social Drinker",
                    Education = "High/Secondary school",
                    EmailAddress = "fake@faked.com",
                    EyeColour = "Blue",
                    HairColour = "Blonde",
                    Height = "5'2''-5'6'' (157-169cm)",
                    IdealPartner = "Anyone with a pulse",
                    Interests = "Cycling,Gym / Aerobics,Movies / Cinema,Food and Wine",
                    KIKAccountName = "fakekik",
                    Location = "Tunbridge Wells",
                    LookingFor = "Someone tall",
                    MaritalStatus = "Married",
                    Notes = "My Notes",
                    Occupation = "Retail/Consumer",
                    PartialSummary = "I'm a fun girl, who enjoys socialising with friends. I love shopping and eating out. My friends are important to me and I enjoy spending time with themI'm 5:7 slim build. I like nice clothes be it casual at the weekends to smart and sexy during work time...(more)",
                    PhotoPassword = "password",
                    Race = "Caucasian/White",
                    RealName = "Fake Fake",
                    Region = "Kent",
                    RelationshipType = "FWB",
                    Religion = "None",
                    Smoking = "Non-Smoker",
                    TelephoneNumber = "01234 5678901",
                    WhatsAppNumber = "01234 5677890",
                    ThumbnailUrl = "https://www.illicitencounters.com/img/avatars/female_Avatar46.jpg"
                };
                Initialise(fakeMember);
			}
		}

        protected void Initialise(IEMember member) 
        {
            this.Title = "Profile for " + member.Username + " (" + member.Age + ")";
			
            IEDataSource ds = IEDataSource.GetInstance();

            ds.GetMemberProfile(member);

			BindingContext = member;
        }
    }
}
