//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Portal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SiteKontrol
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SiteKontrol()
        {
            this.SiteKontrolDomainBirlestirs = new HashSet<SiteKontrolDomainBirlestir>();
        }
    
        public int SiteKontrolID { get; set; }
        public string SiteKontrolAdi { get; set; }
        public string SiteKontrolAciklama { get; set; }
        public Nullable<int> SiteKontrolSira { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SiteKontrolDomainBirlestir> SiteKontrolDomainBirlestirs { get; set; }
    }
}
