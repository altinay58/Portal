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
    
    public partial class Firma
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Firma()
        {
            this.Arayanlars = new HashSet<Arayanlar>();
            this.CariHarekets = new HashSet<CariHareket>();
            this.Domains = new HashSet<Domain>();
            this.Firma1 = new HashSet<Firma>();
            this.islers = new HashSet<isler>();
            this.Koordinats = new HashSet<Koordinat>();
            this.OdemeBildirimis = new HashSet<OdemeBildirimi>();
            this.Randevus = new HashSet<Randevu>();
            this.SanalPos = new HashSet<SanalPos>();
            this.Satis = new HashSet<Sati>();
            this.Satis1 = new HashSet<Sati>();
            this.Teklifs = new HashSet<Teklif>();
            this.FirmaKisis = new HashSet<FirmaKisi>();
        }
    
        public int FirmaID { get; set; }
        public string FirmaAdi { get; set; }
        public string FirmaAdres { get; set; }
        public string YetkiliAdi { get; set; }
        public string YetkiliSoyAdi { get; set; }
        public string YetkiliTelefon { get; set; }
        public string YetkiliCepTelefon { get; set; }
        public Nullable<int> RefFirmaID { get; set; }
        public string Email { get; set; }
        public Nullable<bool> Araci { get; set; }
        public Nullable<bool> Musteri { get; set; }
        public Nullable<bool> Personel { get; set; }
        public Nullable<bool> Kasa { get; set; }
        public Nullable<int> RefKonumID { get; set; }
        public string firmaKayitYapanKisiID { get; set; }
        public string firmaSehir { get; set; }
        public string firmailce { get; set; }
        public Nullable<int> firmaAraciGrupID { get; set; }
        public Nullable<bool> firmaKayitDurum { get; set; }
        public Nullable<int> firmaDomainKategoriID { get; set; }
        public Nullable<System.DateTime> firmaKayitTarih { get; set; }
        public Nullable<int> firmaSektorID { get; set; }
        public Nullable<int> firmaRefArayanID { get; set; }
        public string FirmaVergiDairesi { get; set; }
        public string FirmaVergiNumarasi { get; set; }
        public bool FirmaSilindi { get; set; }
    
        public virtual ArayanGrup ArayanGrup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Arayanlar> Arayanlars { get; set; }
        public virtual Arayanlar Arayanlar { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CariHareket> CariHarekets { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Domain> Domains { get; set; }
        public virtual DomainKategori DomainKategori { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Firma> Firma1 { get; set; }
        public virtual Firma Firma2 { get; set; }
        public virtual Konum Konum { get; set; }
        public virtual Sektorler Sektorler { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<isler> islers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Koordinat> Koordinats { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OdemeBildirimi> OdemeBildirimis { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Randevu> Randevus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SanalPos> SanalPos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sati> Satis { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sati> Satis1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Teklif> Teklifs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FirmaKisi> FirmaKisis { get; set; }
    }
}
