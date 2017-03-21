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
    
    public partial class SatisFirsati
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SatisFirsati()
        {
            this.SatisFirsatiFiyatRevizyons = new HashSet<SatisFirsatiFiyatRevizyon>();
        }
    
        public int Id { get; set; }
        public Nullable<int> RefFirmaId { get; set; }
        public decimal Fiyat { get; set; }
        public Nullable<int> RefDomainKategoriId { get; set; }
        public Nullable<int> RefYetkiliId { get; set; }
        public System.DateTime Tarih { get; set; }
        public string Note { get; set; }
        public int GecerlilikSuresi { get; set; }
        public Nullable<System.DateTime> DuzeltmeTarihi { get; set; }
        public string DosyaYolu { get; set; }
        public string RefSorumluKisiId { get; set; }
        public string RefEkleyenKisiId { get; set; }
        public string ReferansNo { get; set; }
        public Nullable<int> EtiketSatisAsamaId { get; set; }
        public Nullable<int> EtiketSatisFirsatDurumuId { get; set; }
    
        public virtual DomainKategori DomainKategori { get; set; }
        public virtual Firma Firma { get; set; }
        public virtual FirmaKisi FirmaKisi { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SatisFirsatiFiyatRevizyon> SatisFirsatiFiyatRevizyons { get; set; }
    }
}
