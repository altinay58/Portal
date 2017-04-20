using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;
using Microsoft.AspNet.Identity;

namespace Portal.Controllers
{
    public class SatisBolumuController : BaseController
    {
        public SatisBolumuController()
        {
            ViewBag.guncelMenu = "Satis Bolumu";
        }
        // GET: SatisBolumu
        public ActionResult Index()
        {
            return View();
        }
        #region Notlar
        public ActionResult Notlar()
        {
            return View(Db.SatisNotus.OrderByDescending(x=>x.SatisNotlariTarih).ToList());
        }
        public ActionResult NotKaydet(int?id)
        {
            SatisNotu model = new SatisNotu();
            if (id.HasValue)
            {
                model = Db.SatisNotus.SingleOrDefault(x=>x.SatisNotlariID==id);
            }
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult NotKaydet(SatisNotu model)
        {
            SatisNotu entity = new SatisNotu();
            if (model.SatisNotlariID>0)
            {
                entity = Db.SatisNotus.SingleOrDefault(x => x.SatisNotlariID == model.SatisNotlariID);
            }
            entity.SatisNotu1 = model.SatisNotu1;
            entity.SatisNotlariTarih = DateTime.Now;
            //todo: DEFAULT_USER_ID login ekranından sonra kaldırılacak
            entity.RefUserID = User.Identity.GetUserId();
            if (model.SatisNotlariID ==0)
            {
                Db.SatisNotus.Add(entity);
            }
            Db.SaveChanges();
            TempData[SUCESS] = "Kaydedildi";
            return RedirectToAction("Notlar") ;
        }
        public ActionResult Sil(int id)
        {
            var satisSoru = Db.SatisNotus.SingleOrDefault(x=>x.SatisNotlariID==id);
            Db.SatisNotus.Remove(satisSoru);
            TempData[SUCESS] = "Kayıt Silindi";
            Db.SaveChanges();
            return RedirectToAction("Notlar");
        }
        #endregion Notlar
        #region RootBilgisi
        public ActionResult RootBilgisi()
        {
            return View();
        }
        #endregion rootbilgisi
        #region randevular
        public ActionResult Randevular()
        {
            //IEnumerable<Randevu> randevular = Db.Randevus.Where(m => m.RandevuSilDurum == false).OrderByDescending(m => m.RandevuTarihi)
            //    .Skip(baslanacakSira).Take(10);
            //ViewBag.ToplamEleman = Db.Randevus.ToList().Count;
            return View();
        }
        public JsonResult RandevuAra(string basTarih, string bitisTarih,int sayfaNo)
        {
            JsonCevap jsn = new JsonCevap();
            int baslangic = (sayfaNo - 1) * PagerCount;
            DateTime tBas = DateTime.Parse(basTarih);
            DateTime tBit = DateTime.Parse(bitisTarih).AddHours(23).AddMinutes(59);
            jsn.ToplamSayi = Db.RandevularViews.Where(x => x.RandevuTarihi >= tBas && x.RandevuTarihi <= tBit          
            ).OrderByDescending(x => x.RandevuTarihi).Count();

            var query = Db.RandevularViews.Where(x => x.RandevuTarihi >= tBas && x.RandevuTarihi <= tBit
           ).OrderByDescending(x => x.RandevuTarihi).Skip(baslangic).Take(PagerCount);
            jsn.Data = query.ToList();
            return Json(jsn, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RandevuKaydet(int?id)
        {
            Randevu randevu = new Randevu();
            if (id.HasValue)
            {
                randevu = Db.Randevus.SingleOrDefault(x=>x.RandevuID==id);
            }
            ViewBag.RandevuYetkiliKisiID = new SelectList(Db.AspNetUsers.TumKullanicilar(), "Id", "UserName");
            return View(randevu);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult RandevuKaydet(Randevu model)
        {
            Randevu randevu = new Randevu();
            if (model.RandevuID>0)
            {
                randevu = Db.Randevus.SingleOrDefault(x => x.RandevuID == model.RandevuID);
            }
            randevu.RandevuSilDurum = false;
            randevu.RandevuDetay = model.RandevuDetay;
            randevu.RandevuKayitTarihi = DateTime.Now;
            randevu.RandevuEkleyenID = User.Identity.GetUserId();
            randevu.RandevuRefFirmaID = model.RandevuRefFirmaID;
            randevu.RandevuRefArayanID = model.RandevuRefArayanID;
            if (model.RandevuRefFirmaID != null)
            {
                randevu.RandevuKonumID = Fonksiyonlar.FirmaBolgeIDGetir(model.RandevuRefFirmaID ?? 0);
            }
            else
            {
                randevu.RandevuKonumID = Fonksiyonlar.KonumIDGetir(model.RandevuRefArayanID ?? 0);
            }
            randevu.RandevuTarihi = new DateTime(model.RandevuTarihi.Value.Year, model.RandevuTarihi.Value.Month,
                       model.RandevuTarihi.Value.Day, 0, 0, 0);
            if (!string.IsNullOrEmpty(Request["SaatDakika"]))
            {
                string[] ary = Request["SaatDakika"].Split(':');
                randevu.RandevuTarihi = new DateTime(model.RandevuTarihi.Value.Year, model.RandevuTarihi.Value.Month, 
                    model.RandevuTarihi.Value.Day,
                    Convert.ToInt32(ary[0]), Convert.ToInt32(ary[1]), 0);
            }
            if (model.RandevuID > 0)
            {
                Db.Entry(randevu).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                Db.Randevus.Add(randevu);
            }
            randevu.RandevuYetkiliKisiID = model.RandevuYetkiliKisiID;
            randevu.RandevuYeri = model.RandevuYeri;
            Db.SaveChanges();
            TempData[SUCESS] = "Randevu Kaydedildi";
            return RedirectToAction("Randevular");
        }
        public ActionResult RandevuSil(int id)
        {
            var randevu = Db.Randevus.SingleOrDefault(x => x.RandevuID == id);
            Db.Randevus.Remove(randevu);
            TempData[SUCESS] = "Kayıt Silindi";
            Db.SaveChanges();
            return RedirectToAction("Randevular");
        }
        #endregion randevular
        #region konum
        public ActionResult Konum()
        {           
            return View();
        }
        [HttpPost]
        public ActionResult Konum(Koordinat konum)
        {
            Koordinat entity = new Koordinat();
            if (konum.KoordinatID <= 0)
            {                
                Db.Koordinats.Add(entity);
            }else
            {
                entity = Db.Koordinats.SingleOrDefault(x => x.KoordinatID == konum.KoordinatID);
            }
            entity.RefFirmaID = konum.RefFirmaID;
            entity.KoordinatBoylam = konum.KoordinatBoylam;
            entity.KoordinatEnlem = konum.KoordinatEnlem;
            Db.SaveChanges();
            TempData[SUCESS] = $"Kaydedildi <a href='#'> id:  {entity.KoordinatID}</a>";
            return View();
        }
        public JsonResult GetirFirmaKonum(int firmaId)
        {
            var konum = Db.Koordinats.SingleOrDefault(x => x.RefFirmaID == firmaId);
            object obj = null;
            if (konum != null)
            {
                obj = new { boylam = konum.KoordinatBoylam, enlem = konum.KoordinatEnlem ,id=konum.KoordinatID};
            }
          
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        #endregion konum
        public ActionResult SatisRouteBilgisi(int? konumID)
        {
            SatisRouteView satisRoute = new SatisRouteView();
            ViewBag.RandevuKonumID = new SelectList(Db.Konums, "KonumID", "Konum1");

            if (konumID != null)
            {
                ViewBag.BolgeAdi = Fonksiyonlar.BolgeAdiGetir(konumID);
                satisRoute.arayanlar = Db.Arayanlars.Where(m => m.arayanKayitliMusterimi == false && m.arayanRefKonumID == konumID).OrderByDescending(a => a.arayanID).ToList();
                satisRoute.firmalar = Db.Firmas.Where(m => m.RefKonumID == konumID && m.FirmaSilindi == false).OrderByDescending(a => a.FirmaID).ToList();
                satisRoute.randevular = Db.Randevus.Where(m => m.RandevuKonumID == konumID && m.RandevuSilDurum == false).OrderBy(a => a.RandevuTarihi).ToList();
            }
            else
            {
                ViewBag.BolgeAdi = Fonksiyonlar.BolgeAdiGetir(1005);
                satisRoute.arayanlar = Db.Arayanlars.Where(m => m.arayanKayitliMusterimi == false && m.arayanRefKonumID == 1005).OrderByDescending(a => a.arayanID).ToList();
                satisRoute.firmalar = Db.Firmas.Where(m => m.RefKonumID == 1005 && m.FirmaSilindi == false).OrderByDescending(a => a.FirmaID).ToList();
                satisRoute.randevular = Db.Randevus.Where(m => m.RandevuKonumID == 1005).OrderBy(a => a.RandevuTarihi).ToList();
            }


            return View(satisRoute);
        }
    }
}