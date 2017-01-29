using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models.IslerModels
{
    public class Kullanici
    {
        public string Id { get; set; }
        public string AdSoyad { get; set; }
    }
    public class GecenZaman
    {
        public DateTime? ZamanBasTarih { get; set; }
        public long? GecenZamanSaniye { get; set; }
    }
    public class YorumIs
    {
        public string Aciklama { get; set; }
        public DateTime Tarih { get; set; }
        public string AdSoyad { get; set; }
        public string KullaniciId { get; set; }
    }
    public class DomainIs
    {
        public int IsId { get; set; }
        public string IsAd { get; set; }
        public string IsAciklama { get; set; }
        public int FirmaId { get; set; }
        public List<Kullanici> IsiYapacakKullanicilar { get; set; }
        public Kullanici IsiVerenKullanici { get; set; }
        public int IsDurum { get; set; }
        public int SiraNo { get; set; }
        public bool BitisTarihiVarmi { get; set; }
        public DateTime? BitisTarihi { get; set; }
        public DateTime? Tarih { get; set; }
        public GecenZaman IsGecenZaman { get; set; }
        public List<YorumIs> Yorumlar { get; set; }
        public DomainIs()
        {
            IsiYapacakKullanicilar = new List<Kullanici>();
            Yorumlar = new List<YorumIs>();
        }
    }
}