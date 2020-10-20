using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessPlatform.ViewModel
{
    public class tBookingVM
    {
        public int RestID { get; set; }
        public int OrderID { get; set; }

        public int CoachId { get; set; }
        public int GymId { get; set; }
        public string OrderStatus { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime Endtime { get; set; }
        public System.DateTime Applytime { get; set; }
        public Nullable<System.DateTime> BirthDay { get; set; }
        public string TrainingName { get; set; }
        public string GymName { get; set; }
        public string CoachName { get; set; }
        public string Type { get; set; }



        ////////////////////////////////////
        public Nullable<byte> fMemberAge { get; set; }
        public string fMemberCID { get; set; }
        public Nullable<System.DateTime> fMemberBirthday { get; set; }
        public string fMemberPhone { get; set; }
        public string MemberAddress { get; set; }
        public string fMemberEmail { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string fMemberGender { get; set; }

        public string fCorpName { get; set; }
    }

    
}