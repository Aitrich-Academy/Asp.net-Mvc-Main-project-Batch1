//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class USER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USER()
        {
            this.ORDERS = new HashSet<ORDER>();
        }
    
        public int USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string USER_EMAIL { get; set; }
        public string USER_PASSWORD { get; set; }
        public string USER_PHONE { get; set; }
        public string USER_ADDRESS { get; set; }
        public byte[] USER_IMAGE { get; set; }
        public string USER_ROLE { get; set; }
        public string USER_STATUS { get; set; }
        public string USER_CREATEBY { get; set; }
        public string USER_CREATEDATE { get; set; }
        public string USER_MODIBY { get; set; }
        public string USER_MODIDATE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ORDER> ORDERS { get; set; }
    }
}
