//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FitnessPlatform
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    public partial class tMember
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tMember()
        {
            this.tRecord = new HashSet<tRecord>();
        }
    
        public int fMemberId { get; set; }
        public string fMemberName { get; set; }
        public string fMemberGender { get; set; }
        public Nullable<byte> fMemberAge { get; set; }
        public string fMemberCID { get; set; }
        public Nullable<System.DateTime> fMemberBirthday { get; set; }
        public string fMemberPhone { get; set; }
        public string MemberAddress { get; set; }
        public string fMemberEmail { get; set; }
        public string fMemberPassword { get; set; }
        public string fMemberPicture { get; set; }
        public string fMember_approced { get; set; }
        public string fMember_Num { get; set; }
        public Nullable<System.DateTime> fMember_time { get; set; }
        public Nullable<int> fMember_googleID { get; set; }

        public HttpPostedFileBase image { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tRecord> tRecord { get; set; }
    }
}
