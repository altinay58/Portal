using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Portal.Filters;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Portal.Controllers
{
    public class IsPlaniController : BaseController
    {
        // GET: IsPlani
        public IsPlaniController()
        {
            GuncelMenu = "Satis Bolumu";
        }
        public ActionResult List()
        {
            var isPlanis = Db.IsPlanis.Include(i => i.AspNetUser).Include(i => i.Firma).Include(i => i.SatisFirsati);
            return View(isPlanis.ToList());
        }
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult SatisFirsatiAra(int? konumId,string adSoyad,string firmaAdi,string telNo)
        {
            Db.Configuration.ProxyCreationEnabled = false;
            var query = (from s in Db.SatisFirsatis.Include(s => s.Firma).Include(s => s.Firma.Konum)
                     
              
                        let tarih = SqlFunctions.DateAdd("day", (double)s.GecerlilikSuresi, s.Tarih)// s.Tarih.AddDays(s.GecerlilikSuresi)
                        let sonKayit = Db.SatisFirsatiFiyatRevizyons.Where(x => x.RefSatisFirsatiId == s.Id).OrderByDescending(x => x.Id).FirstOrDefault()
                        let ilkKayit = Db.SatisFirsatiFiyatRevizyons.Where(x => x.RefSatisFirsatiId == s.Id).OrderBy(x => x.Id).FirstOrDefault()
                        orderby s.Id descending
                        select new
                        {
                            Id = s.Id,
                            Bolge = s.Firma.Konum.Konum1,
                            KonumId=s.Firma.RefKonumID,
                            Musteri = s.Firma.YetkiliAdi + " " + s.Firma.YetkiliSoyAdi,
                            DomainKategori = s.DomainKategori.DomainKategoriAdi,
                            EtiketSatisAsamaId = s.EtiketSatisAsamaId,
                            EtiketSatisFirsatDurumuId = s.EtiketSatisFirsatDurumuId,
                            SonTeklif = (sonKayit != null ? sonKayit.Fiyat : 0),
                            KalanSure = DbFunctions.DiffDays(s.Tarih, tarih),
                            SatisFirsatiFiyatRevizyons = s.SatisFirsatiFiyatRevizyons,
                            FirmaKisiler = s.Firma.FirmaKisis,
                            DosyaYolu = s.DosyaYolu,
                            FirmaAdi = s.Firma.FirmaAdi,
                            Firma = new { Id = s.Firma.FirmaID, Ad = s.Firma.FirmaAdi, Cep = s.Firma.YetkiliCepTelefon, Tel = s.Firma.YetkiliTelefon },
                            Teklif = (ilkKayit != null ? ilkKayit.Fiyat :0),
                            EtiketSatisAsama = (
                                               from e in Db.Etikets
                                               where e.Kategori == "EtiketSatisAsamaId" && s.EtiketSatisAsamaId == e.Value
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
        [JsonNetFilter]
        public JsonResult BorcluFirmalarAra(int? konumId,int? page,string firmaAdi,string telNo,string cepTelNo,string yetkili)
        {
            Db.Configuration.ProxyCreationEnabled = false;
            int baslangic = ((page ?? 1) - 1) * PagerCount;
            var list2 = Db.Firmas.TumFirmalar().Include(x=>x.Konum).Include(x=>x.Firma2).Where(x => x.Musteri == true)
                     .Where(a => (a.CariHarekets.Sum(c => c.ChSatisFiyati) - a.CariHarekets.Sum(c => c.ChAlinanOdeme)) > 0)
                     .Where(x => !string.IsNullOrEmpty(firmaAdi) ? x.FirmaAdi.Contains(firmaAdi) : true
                     && konumId.HasValue ? x.Konum.KonumID==konumId : true);

            if (!User.IsInRole("Muhasebe") && User.IsInRole("Satis"))
            {
                list2 = list2.Where(x => (x.Personel != true && x.Kasa != true));
            }
        
            var query = list2.Include(x => x.Konum).Include(x => x.Firma2)
                         .Select(x => new
                         {
                             konum = x.Konum.Konum1,
                             araci = x.Firma2.FirmaAdi,
                             firma = x,
                             domainSayisi = x.Domains.Count(),
                             borcu = x.CariHarekets.Sum(q => q.ChSatisFiyati) - x.CariHarekets.Sum(q => q.ChAlinanOdeme)
                         });

            var data = (from s in query
                        where
                        (string.IsNullOrEmpty(telNo) ? true : s.firma.YetkiliCepTelefon.Contains(telNo)) &&
                        (string.IsNullOrEmpty(cepTelNo) ? true : s.firma.YetkiliTelefon.Contains(cepTelNo)) &&
                        (string.IsNullOrEmpty(yetkili) ? true : s.firma.YetkiliAdi.Contains(yetkili))
                        select s)
                .OrderByDescending(x => x.firma.CariHarekets.Sum(c => c.ChSatisFiyati) - x.firma.CariHarekets.Sum(c => c.ChAlinanOdeme));
             
            var qTotal2 = data;
            int totalCount = qTotal2.Count();
           
         
            var jsn = new JsonCevap();
            jsn.Basarilimi = true;
            jsn.Data = data.Skip(baslangic).Take(PagerCount).ToList();
            jsn.ToplamSayi = totalCount;
            Db.Configuration.ProxyCreationEnabled = true;
            return Json(jsn, JsonRequestBehavior.AllowGet);
        }
        [JsonNetFilter]
        public JsonResult FirmaAra(int? konumId, int? page, string firmaAdi, string telNo, string cepTelNo, string yetkili)
        {
            Db.Configuration.ProxyCreationEnabled = false;
            int baslangic = ((page ?? 1) - 1) * PagerCount;
            var list = Db.Firmas.GetirFirmalar("").Include(x => x.Konum).Include(x => x.Firma2).Where(x => x.Musteri == true).
                Where(x=> konumId.HasValue ? x.Konum.KonumID == konumId : true);       

            if (!User.IsInRole("Muhasebe") && User.IsInRole("Satis"))
            {
                list = list.Where(x => (x.Personel != true && x.Kasa != true));
            }
            var query = list.Include(x => x.Konum).Include(x => x.Firma2)
                  .Select(x => new
                  {
                      konum = x.Konum.Konum1,
                      araci = x.Firma2.FirmaAdi,
                      firma = x,
                      domainSayisi = x.Domains.Count(),
                      borcu = x.CariHarekets.Sum(q => q.ChSatisFiyati) - x.CariHarekets.Sum(q => q.ChAlinanOdeme)
                  });
            var data = (from s in query
                        where                       
                        (string.IsNullOrEmpty(firmaAdi) ? true : s.firma.FirmaAdi.Contains(firmaAdi)) &&
                        (string.IsNullOrEmpty(telNo) ? true : s.firma.YetkiliCepTelefon.Contains(telNo)) &&
                        (string.IsNullOrEmpty(cepTelNo) ? true : s.firma.YetkiliTelefon.Contains(cepTelNo)) &&
                        (string.IsNullOrEmpty(yetkili) ? true : s.firma.YetkiliAdi.Contains(yetkili))
                        select s)
             .OrderByDescending(x => x.firma.FirmaID);
            var qTotal2 = data;
            int totalCount = qTotal2.Count();


            var jsn = new JsonCevap();
            jsn.Basarilimi = true;
            jsn.Data = data.Skip(baslangic).Take(PagerCount).ToList();
            jsn.ToplamSayi = totalCount;
            Db.Configuration.ProxyCreationEnabled = true;
            return Json(jsn, JsonRequestBehavior.AllowGet);

        }
        [ValidateInput(false)]
        public JsonResult IsPlaniKaydet(string jsnIsPlani)
        {
            IsPlani isPlani= JsonConvert.DeserializeObject<IsPlani>(jsnIsPlani);
            JsonCevap jsn = new JsonCevap();
            if (isPlani.Id == 0)
            {
                Db.IsPlanis.Add(isPlani);
            }
            else
            {          

                Db.Entry(isPlani).State = EntityState.Modified;
            }
         
            var dahaOnceVarmi = Db.IsPlanis.Where(x => DbFunctions.TruncateTime(x.Tarih) == DbFunctions.TruncateTime(isPlani.Tarih)
               && x.RefFirmaId == isPlani.RefFirmaId && x.RefIsId == isPlani.RefIsId && x.RefSatisFirsatiId == isPlani.RefSatisFirsatiId &&
               x.RefSorumluKisiId == isPlani.RefSorumluKisiId).ToList().Count > 0;
            if (dahaOnceVarmi)
            {
                jsn.Basarilimi = false;
            }
            else
            {
                Db.SaveChanges();
                jsn.Basarilimi = true;
            }
         
            jsn.Data = isPlani.Id;
            return Json(jsn, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BuguneAitIsPlanlari()
        {
            Db.Configuration.ProxyCreationEnabled = false;
            string userId = User.Identity.GetUserId();
            var data = Db.IsPlanis.AsNoTracking().Where(x => DbFunctions.TruncateTime(x.Tarih) == DateTime.Today && x.RefSorumluKisiId == userId
            ).OrderBy(x=>x.Tarih).ToList();
            Db.Configuration.ProxyCreationEnabled = true;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsPlaniDurumDegistirme(int planId,int yeniDurum)
        {
            JsonCevap jsn = new JsonCevap();
            var entity = Db.IsPlanis.SingleOrDefault(x => x.Id == planId);
            entity.EtiketIsPlaniDurum = yeniDurum;
            Db.SaveChanges();
            jsn.Basarilimi = true;
            return Json(jsn, JsonRequestBehavior.AllowGet);
        }
    }
}