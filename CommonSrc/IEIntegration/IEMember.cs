using System;
using System.ComponentModel;
using Newtonsoft.Json;
using SQLite;

namespace IE.CommonSrc.IEIntegration
{
    [Table("members")]
    public class IEMember : IComparable, INotifyPropertyChanged
    {
        public const int MEMBER_NEW = 0;
        public const int MEMBER_KNOWN = 1;
        public const int MEMBER_ACTIVE = 2;
        public const int MEMBER_REJECTED = 3;

        [PrimaryKey, AutoIncrement, Column("_id")]
        [JsonProperty("id")]
        public int Id { get; set; }                     // Required by SQLite
        [Unique]
		[JsonProperty("profileId")]
		public string ProfileId { get; set; }           // This is the users unique code - not there login name which can change
		[JsonProperty("realName")]
		public string RealName { get; set; }            // Do we know thier real name
		[JsonProperty("photoPassword")]
		public string PhotoPassword { get; set; }       // Have we had thier photo password?
		[JsonProperty("sentMyPhotoPassword")]
		public bool SentMyPhotoPassword { get; set; }   // Have we sent our photo password?
		[JsonProperty("TelephoneNumber")]
		public string TelephoneNumber { get; set; }     // Members telephone number
		[JsonProperty("WhatsAppNumber")]
		public string WhatsAppNumber { get; set; }      // Members telephone number if using WhatsApp
		[JsonProperty("kikAccountName")]
		public string KIKAccountName { get; set; }      // Members KIK Account
		[JsonProperty("emailAddress")]
		public string EmailAddress { get; set; }        // Members Email address
		[JsonProperty("location")]
		public string Location { get; set; }            // Normally town etc
		[JsonProperty("replyRxed")]
		public bool ReplyRxed { get; set; }             // Have we had a reply
		[JsonProperty("notes")]
		public string Notes { get; set; }               // General notes
		[JsonProperty("status")]
		public int Status { get; set; }                 // 0 = new, 1 = known, 2 = active and 3 = rejected
		[JsonProperty("rejectReason")]
		public string RejectReason { get; set; }        // Why did we reject them (or they us)?
		[JsonProperty("firstContactTimestamp")]
		public DateTime FirstContactTimestamp { get; set; } // When we first sent them a message (or they sent us)
		[JsonProperty("lastActivityTimestamp")]
		public DateTime LastActivityTimestamp { get; set; } // Timestamp of last activity

		//
		// These fields are picked up during a search.
		//
		[JsonProperty("username")]
		public string Username { get; set; }
		[JsonProperty("partialSummary")]
		public string PartialSummary { get; set; }
		[JsonProperty("region")]
		public string Region { get; set; }
		[JsonProperty("thumbnailUrl")]
		public string ThumbnailUrl { get; set; }
		[JsonProperty("age")]
		public string Age { get; set; }

		//
		// These fields are picked up looking at the full profile.
		//
		[JsonProperty("fetchedExtraData")]
		public DateTime FetchedExtraData { get; set; }  // When did we last look at thier full profile?
		[JsonProperty("race")]
		public string Race { get; set; }
		[JsonProperty("maritalStatus")]
		public string MaritalStatus { get; set; }
		[JsonProperty("religion")]
		public string Religion { get; set; }
		[JsonProperty("eyeColour")]
		public string EyeColour { get; set; }
		[JsonProperty("drinking")]
		public string Drinking { get; set; }
		[JsonProperty("smoking")]
		public string Smoking { get; set; }
		[JsonProperty("hairColour")]
		public string HairColour { get; set; }
		[JsonProperty("build")]
		public string Build { get; set; }
		[JsonProperty("height")]
		public string Height { get; set; }
		[JsonProperty("education")]
		public string Education { get; set; }
		[JsonProperty("occupation")]
		public string Occupation { get; set; }
		[JsonProperty("interests")]
		public string Interests { get; set; }
		[JsonProperty("lookingFor")]
		public string LookingFor { get; set; }
		[JsonProperty("idealPartner")]
		public string IdealPartner { get; set; }
		[JsonProperty("relationshipType")]
		public string RelationshipType { get; set; }
		[JsonProperty("about")]
		public string About { get; set; }

        [Ignore]
        public string UsernameAndAge
        {
            get {
                return Username + " (" + Age + ")";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int CompareTo(object o)
		{
			IEMember a = this;
			IEMember b = (IEMember)o;
            return string.Compare(a.Username, b.Username);
		}



        public void FirePropertyChangeEvent() {
            if ( PropertyChanged != null ) {
                PropertyChanged(this, new PropertyChangedEventArgs(""));            // Empty string means everything has changed
            }
        }
	}
}
