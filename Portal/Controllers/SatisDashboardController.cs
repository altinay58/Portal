using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class SatisDashboardController : BaseController
    {
        public SatisDashboardController()
        {
            GuncelMenu = "Satis Bolumu";
        }
        // GET: SatisDashboard
        public ActionResult Dashboard()
        {
            return View();
        }
        public JsonResult SatisFirsatlariAra(int? konumId, string adSoyad, string firmaAdi, string telNo)
        {
            Db.Configuration.ProxyCreationEnabled = false;
            var query = (from s in Db.SatisFirsatis.Include(s => s.Firma).Include(s => s.Firma.Konum)


                         let tarih = SqlFunctions.DateAdd("day", (double)s.GecerlilikSuresi, s.Tarih)// s.Tarih.AddDays(s.GecerlilikSuresi)
                         let sonKayit = Db.SatisFirsatiFiyatRevizyons.Where(x => x.RefSatisFirsatiId == s.Id).OrderByDescending(x => x.Id).FirstOrDefault()
                         let ilkKayit = Db.SatisFirsatiFiyatRevizyons.Where(x => x.RefSatisFirsatiId == s.Id).OrderBy(x => x.Id).FirstOrDefault()
                         let kisi = s.Firma.FirmaKisis.FirstOrDefault()
                         orderby s.Id descending
                         select new
                         {
                             Id = s.Id,
                             Bolge = s.Firma.Konum.Konum1,
                             KonumId = s.Firma.RefKonumID,
                             Musteri = kisi.Ad + " " + kisi.Soyad,
                             DomainKategori = s.DomainKategori.DomainKategoriAdi,
                             EtiketSatisAsamaId = s.EtiketSatisAsamaId,
                             EtiketSatisFirsatDurumuId = s.EtiketSatisFirsatDurumuId,
                             SonTeklif = (sonKayit != null ? sonKayit.Fiyat : 0),
                             Tarih=s.Tarih,
                             KalanSure = DbFunctions.DiffDays(DateTime.Today, tarih),
                             SatisFirsatiFiyatRevizyons = s.SatisFirsatiFiyatRevizyons,                          
                             DosyaYolu = s.DosyaYolu,
                             FirmaAdi = s.Firma.FirmaAdi,
                             Firma = new { Id = s.Firma.FirmaID, Ad = s.Firma.FirmaAdi, Cep = kisi.Tel, Tel = kisi.Tel2},
                             IlkTeklif = (ilkKayit != null ? ilkKayit.Fiyat : 0),
                             EtiketSatisAsama = (
                                                from e in Db.Etikets
                                                where e.Kategori == "EtiketSatisAsamaId" && s.EtiketSatisAsamaId == e.Value
                                                select e
                                                ).FirstOrDefault(),
                             EtiketSatisFirsatDurumu=(
                                                from e in Db.Etikets
                                                where e.Kategori == "EtiketSatisFirsatDurumuId" && s.EtiketSatisFirsatDurumuId == e.Value
                                                select e
                                                     ).FirstOrDefault()
                         }
                     );
            var data = (from s in query
                        where
                        (konumId.HasValue ? s.KonumId == konumId : true) &&
                        (string.IsNullOrEmpty(adSoyad) ? true : s.Musteri.Contains(adSoyad)) &&
                        (string.IsNullOrEmpty(firmaAdi) ? true : s.FirmaAdi.Contains(firmaAdi)) &&
                        ((string.IsNullOrEmpty(telNo) ? true : s.Firma.Cep.Contains(telNo)) ||
                        (string.IsNullOrEmpty(telNo) ? true : s.Firma.Tel.Contains(telNo)))
                        select s
                       ).ToList();
            Db.Configuration.ProxyCreationEnabled = true;
            return Json(data.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}