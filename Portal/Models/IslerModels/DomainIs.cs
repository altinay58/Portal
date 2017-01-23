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
    public class DomainIs
    {
        public int IsId { get; set; }
        public string IsAd { get; set; }
        public string IsAciklama { get; set; }
        public int FirmaId { get; set; }
        public List<Kullanici> IsiYapacakKullanicilar { get; set; }
        public Kullanici IsiVerenKullanici { get; set; }
        public DomainIs()
        {
            IsiYapacakKullanicilar = new List<Kullanici>();
        }
    }
}