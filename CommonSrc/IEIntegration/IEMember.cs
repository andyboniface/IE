using System;
using SQLite;

namespace IE.CommonSrc.IEIntegration
{
    [Table("members")]
    public class IEMember
    {
        public const int MEMBER_NEW = 0;
        public const int MEMBER_KNOWN = 1;
        public const int MEMBER_ACTIVE = 2;
        public const int MEMBER_REJECTED = 3;

        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }                     // Required by SQLite
        [Unique]
        public string ProfileId { get; set; }           // This is the users unique code - not there login name which can change
        public string RealName { get; set; }            // Do we know thier real name
        public string PhotoPassword { get; set; }       // Have we had thier photo password?
        public bool SentMyPhotoPassword { get; set; }   // Have we sent our photo password?
        public string TelephoneNumber { get; set; }     // Members telephone number
        public string WhatsAppNumber { get; set; }      // Members telephone number if using WhatsApp
        public string KIKAccountName { get; set; }      // Members KIK Account
        public string EmailAddress { get; set; }        // Members Email address
        public string Location { get; set; }            // Normally town etc
        public bool ReplyRxed { get; set; }             // Have we had a reply
        public string Notes { get; set; }               // General notes
        public int Status { get; set; }                 // 0 = new, 1 = known, 2 = active and 3 = rejected
        public string RejectReason { get; set; }        // Why did we reject them (or they us)?
        public DateTime FirstContactTimestamp { get; set; } // When we first sent them a message (or they sent us)
        public DateTime LastActivitiyTimestamp { get; set; } // Timestamp of last activity

        [Ignore]
		public string Username { get; set; }
		[Ignore]
		public string PartialSummary { get; set; }
		[Ignore]
		public string Region { get; set; }
		[Ignore]
		public string ThumbnailUrl { get; set; }
		[Ignore]
		public string Age { get; set; }
	}
}
