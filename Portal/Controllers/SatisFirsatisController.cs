using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Portal.Models;
using System.IO;
using Microsoft.AspNet.Identity;
using System.Data.Entity.SqlServer;

namespace Portal.Controllers
{
    public class SatisFirsatisController : BaseController
    {
        public SatisFirsatisController()
        {
            GuncelMenu = "Satis Bolumu";          
        }
        //private PortalEntities db = new PortalEntities();

        // GET: SatisFirsatis
        public ActionResult List()
        {          
            //CacheManagement.SetCache(CacheKeys.ETIKETS, Db.Etikets.ToList());           
      
            var satisFirsatis = Db.SatisFirsatis.Include(s => s.DomainKategori).Include(s => s.Firma).Include(s => s.FirmaKisi);
            return View(satisFirsatis.ToList());
        }
        #region details
        // GET: SatisFirsatis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Db.Configuration.ProxyCreationEnabled = false;
            SatisFirsati satisFirsati = Db.SatisFirsatis.AsNoTracking().SingleOrDefault(x=>x.Id==id);
            decimal? sonFiyat = null;
            var sonKayit = Db.SatisFirsatiFiyatRevizyons.Where(x=>x.RefSatisFirsatiId==id).OrderByDescending(x => x.Id).FirstOrDefault();
            var tarih = satisFirsati.Tarih;
            tarih = tarih.AddDays(satisFirsati.GecerlilikSuresi);
            int kalanGun = (int)(tarih - satisFirsati.Tarih).TotalDays;
            if (sonKayit != null)
            {
                sonFiyat = sonKayit.Fiyat;
            }
            else
            {
                sonFiyat = satisFirsati.Fiyat;
            }
          
            var data = (from s in Db.SatisFirsatis.Include(s=>s.Firma) 
                        where s.Id==id.Value
                        let fiyat=sonFiyat
                        select new
                        {
                            Id=s.Id,
                            Musteri=s.Firma.YetkiliAdi +" "+s.Firma.YetkiliSoyAdi,
                            DomainKategori = s.DomainKategori.DomainKategoriAdi,
                            EtiketSatisAsamaId=s.EtiketSatisAsamaId,
                            EtiketSatisFirsatDurumuId=s.EtiketSatisFirsatDurumuId,
                            SonTeklif=fiyat,
                            KalanSure=kalanGun,
                            SatisFirsatiFiyatRevizyons=s.SatisFirsatiFiyatRevizyons,
                            FirmaKisiler=s.Firma.FirmaKisis,
                            DosyaYolu=s.DosyaYolu,
                            FirmaAdi=s.Firma.FirmaAdi,
                            Firma=new {Id=s.Firma.FirmaID,Ad=s.Firma.FirmaAdi}
                        }
                        ).FirstOrDefault();
         
            if (satisFirsati == null)
            {
                return HttpNotFound();
            }
            Db.Configuration.ProxyCreationEnabled = true;
            return View(data);
        }
        public JsonResult FirmaTeklifleri(int firmaId,int guncelTeklifId)
        {
            Db.Configuration.ProxyCreationEnabled = false;
            var data = (from s in Db.SatisFirsatis.Include(s => s.Firma)
                        where s.RefFirmaId ==firmaId && s.Id!=guncelTeklifId
                      
                        let tarih= SqlFunctions.DateAdd("day",(double)s.GecerlilikSuresi,s.Tarih)// s.Tarih.AddDays(s.GecerlilikSuresi)
                        let sonKayit = Db.SatisFirsatiFiyatRevizyons.Where(x => x.RefSatisFirsatiId == s.Id).OrderByDescending(x => x.Id).FirstOrDefault()
                        select new
                        {
                            Id = s.Id,
                            Musteri = s.Firma.YetkiliAdi + " " + s.Firma.YetkiliSoyAdi,
                            DomainKategori = s.DomainKategori.DomainKategoriAdi,
                            EtiketSatisAsamaId = s.EtiketSatisAsamaId,
                            EtiketSatisFirsatDurumuId = s.EtiketSatisFirsatDurumuId,
                            SonTeklif = (sonKayit!=null ? sonKayit.Fiyat :s.Fiyat),
                            KalanSure = DbFunctions.DiffDays(tarih,s.Tarih),
                            SatisFirsatiFiyatRevizyons = s.SatisFirsatiFiyatRevizyons,
                            FirmaKisiler = s.Firma.FirmaKisis,
                            DosyaYolu = s.DosyaYolu,
                            FirmaAdi = s.Firma.FirmaAdi,
                            Firma = new { Id = s.Firma.FirmaID, Ad = s.Firma.FirmaAdi },
                            Teklif=s.Fiyat
                        }
                     ).ToList();
            Db.Configuration.ProxyCreationEnabled = true;
            return Json(data.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DegistirEtiketSatisAsama(int satisId,int yeniDurum)
        {
            JsonCevap jsn = new JsonCevap();
            var entity = Db.SatisFirsatis.SingleOrDefault(x => x.Id == satisId);
            entity.EtiketSatisAsamaId = yeniDurum;
            Db.SaveChanges();
            jsn.Basarilimi = true;
            return Json(jsn, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DegistirEtiketFirsatDuru(int satisId, int yeniDurum)
        {
            JsonCevap jsn = new JsonCevap();
            var entity = Db.SatisFirsatis.SingleOrDefault(x => x.Id == satisId);
            entity.EtiketSatisFirsatDurumuId = yeniDurum;
            Db.SaveChanges();
            jsn.Basarilimi = true;
            return Json(jsn, JsonRequestBehavior.AllowGet);
        }
        #endregion details

        // GET: SatisFirsatis/Create
        public ActionResult Kaydet(int? id)
        {
            var butunEtiketler = CacheManagement.Get<Etiket>(CacheKeys.ETIKETS);
            SatisFirsati model = new SatisFirsati();
            if (id.HasValue)
            {
                model = Db.SatisFirsatis.SingleOrDefault(x => x.Id == id);
            }
            ViewBag.RefDomainKategoriId = new SelectList(Db.DomainKategoris, "DomainKategoriID", "DomainKategoriAdi",model.RefDomainKategoriId);
            //ViewBag.RefFirmaId = new SelectList(Db.Firmas, "FirmaID", "FirmaAdi");
            ViewBag.RefYetkiliId = new SelectList(Db.FirmaKisis, "Id", "Ad",model.RefYetkiliId);
            ViewBag.EtiketSatisAsamaId = new SelectList(butunEtiketler.Where(x=>x.Kategori== "EtiketSatisAsamaId").OrderBy(x=>x.Sira), "Value", "Text",model.EtiketSatisAsamaId);
            ViewBag.EtiketSatisFirsatDurumuId = new SelectList(butunEtiketler.Where(x => x.Kategori == "EtiketSatisFirsatDurumuId").OrderBy(x=>x.Sira), "Value", "Text", model.EtiketSatisAsamaId);
            return View(model);
        }

        // POST: SatisFirsatis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet([Bind(Include = "Id,RefFirmaId,EtiketSatisAsamaId,EtiketSatisFirsatDurumuId,Fiyat,RefDomainKategoriId,RefYetkiliId,Tarih,Note,GecerlilikSuresi,DosyaYolu,RefSorumluKisiId")]
        SatisFirsati satisFirsati,int? id)
        {
          
            ViewBag.Title = "Kaydet";
            int cnt = 0;
            if (Db.SatisFirsatis.Count() == 0)
            {
                cnt = 1;
            }
            else
            {
                cnt = Db.SatisFirsatis.Max(item => item.Id);
            }
            var file = Request.Files["dosya"];
            if (file != null && file.ContentLength > 0)
            {
                string filePath = Path.Combine(Server.MapPath("~/upload/"), file.FileName);
                file.SaveAs(filePath);
                satisFirsati.DosyaYolu = "/upload/" + file.FileName;
            }
            if (id == null)
            {
                satisFirsati.ReferansNo = ("D" + satisFirsati.Tarih.Date.ToShortDateString() + "F" + satisFirsati.RefFirmaId + "T" + ++cnt).Replace(".", "");
                satisFirsati.RefEkleyenKisiId = User.Identity.GetUserId();
                Db.SatisFirsatis.Add(satisFirsati);              
            }else
            {
             
                SatisFirsati entity = Db.SatisFirsatis.AsNoTracking().SingleOrDefault(x => x.Id == id);
                satisFirsati.RefEkleyenKisiId = entity.RefEkleyenKisiId;
                satisFirsati.ReferansNo = entity.ReferansNo;
                if (string.IsNullOrEmpty(satisFirsati.DosyaYolu))
                {
                    satisFirsati.DosyaYolu = entity.DosyaYolu;
                }
                Db.Entry(satisFirsati).State = EntityState.Modified;
                satisFirsati.DuzeltmeTarihi = DateTime.Now;
            }

            try
            {
               Db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Diagnostics.Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
            }


            return RedirectToAction("List");
 
        }


        // GET: SatisFirsatis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SatisFirsati satisFirsati = Db.SatisFirsatis.Find(id);
            if (satisFirsati == null)
            {
                return HttpNotFound();
            }
            return View(satisFirsati);
        }

        // POST: SatisFirsatis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SatisFirsati satisFirsati = Db.SatisFirsatis.Find(id);
            Db.SatisFirsatis.Remove(satisFirsati);
            Db.SaveChanges();
            return RedirectToAction("List");
        }



    }
}
