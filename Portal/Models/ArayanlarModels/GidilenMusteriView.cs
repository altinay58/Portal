using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models.ArayanlarModels
{
    public class GidilenMusteriView
    {
        public int GidilenMusteriID { get; set; }
        public string GidilenMusteriFirmaAdi { get; set; }
        public string GidilenMusteriAdi { get; set; }
        public string GidilenMusteriSoyadi { get; set; }
        public string GidilenMusteriTelefon { get; set; }
        public string GidilenMusteriCepTelNo { get; set; }
        public string GidilenMusteriAdres { get; set; }
        public string GidilenMusteriMailAdres { get; set; }
        public string GidilenMusteriSehir { get; set; }
        public string GidilenMusteriilce { get; set; }
        public string GidilenMusteriWebAdresi { get; set; }
        public string GidilenMusteriKonu { get; set; }
        public Nullable<System.DateTime> GidilenMusteriTarih { get; set; }
        public string GidilenMusteriGorusenKullaniciID { get; set; }
        public Nullable<int> GidilenMusteriKonumID { get; set; }
        public Nullable<int> GidilenMusteriDomainKategoriID { get; set; }
        public Nullable<int> GidilenMusteriSektorID { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
        public virtual DomainKategori DomainKategori { get; set; }
        public virtual Konum Konum { get; set; }
        public virtual Sektorler Sektorler { get; set; }

    }
}