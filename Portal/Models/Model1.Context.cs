﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PortalEntities : DbContext
    {
        public PortalEntities()
            : base("name=PortalEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Ayar> Ayars { get; set; }
        public virtual DbSet<Konum> Konums { get; set; }
        public virtual DbSet<isler> islers { get; set; }
        public virtual DbSet<IsiYapacakKisi> IsiYapacakKisis { get; set; }
        public virtual DbSet<StandartProjeIsleri> StandartProjeIsleris { get; set; }
        public virtual DbSet<Arama> Aramas { get; set; }
        public virtual DbSet<ArayanGrup> ArayanGrups { get; set; }
        public virtual DbSet<Arayanlar> Arayanlars { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<CariHareket> CariHarekets { get; set; }
        public virtual DbSet<Chat> Chats { get; set; }
        public virtual DbSet<Domain> Domains { get; set; }
        public virtual DbSet<DomainKategori> DomainKategoris { get; set; }
        public virtual DbSet<Firma> Firmas { get; set; }
        public virtual DbSet<GelinmeyenGun> GelinmeyenGuns { get; set; }
        public virtual DbSet<GidilenMusteri> GidilenMusteris { get; set; }
        public virtual DbSet<Hosting> Hostings { get; set; }
        public virtual DbSet<Istekler> Isteklers { get; set; }
        public virtual DbSet<IstekTipi> IstekTipis { get; set; }
        public virtual DbSet<isKategori> isKategoris { get; set; }
        public virtual DbSet<isOncelikSira> isOncelikSiras { get; set; }
        public virtual DbSet<isYorum> isYorums { get; set; }
        public virtual DbSet<KayitliFirma> KayitliFirmas { get; set; }
        public virtual DbSet<Koordinat> Koordinats { get; set; }
        public virtual DbSet<MailKontrol> MailKontrols { get; set; }
        public virtual DbSet<MailSablonu> MailSablonus { get; set; }
        public virtual DbSet<Makale> Makales { get; set; }
        public virtual DbSet<MakaleKategori> MakaleKategoris { get; set; }
        public virtual DbSet<Notlar> Notlars { get; set; }
        public virtual DbSet<OdemeBildirimi> OdemeBildirimis { get; set; }
        public virtual DbSet<Proje> Projes { get; set; }
        public virtual DbSet<ProjeIsi> ProjeIsis { get; set; }
        public virtual DbSet<Randevu> Randevus { get; set; }
        public virtual DbSet<SanalPos> SanalPos { get; set; }
        public virtual DbSet<Sati> Satis { get; set; }
        public virtual DbSet<SatisNotu> SatisNotus { get; set; }
        public virtual DbSet<Sektorler> Sektorlers { get; set; }
        public virtual DbSet<Seo> Seos { get; set; }
        public virtual DbSet<Sifre> Sifres { get; set; }
        public virtual DbSet<SiteKontrol> SiteKontrols { get; set; }
        public virtual DbSet<SiteKontrolDomainBirlestir> SiteKontrolDomainBirlestirs { get; set; }
        public virtual DbSet<Teklif> Teklifs { get; set; }
        public virtual DbSet<TeklifDetay> TeklifDetays { get; set; }
        public virtual DbSet<TeklifSonuc> TeklifSonucs { get; set; }
        public virtual DbSet<TeklifTuru> TeklifTurus { get; set; }
        public virtual DbSet<Tema> Temas { get; set; }
        public virtual DbSet<WebLog> WebLogs { get; set; }
        public virtual DbSet<YorumDurum> YorumDurums { get; set; }
    }
}
