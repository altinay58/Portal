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
            var satisFirsatis = Db.SatisFirsatis.Include(s => s.DomainKategori).Include(s => s.Firma).Include(s => s.FirmaKisi);
            return View(satisFirsatis.ToList());
        }

        // GET: SatisFirsatis/Details/5
        public ActionResult Details(int? id)
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

        // GET: SatisFirsatis/Create
        public ActionResult Kaydet(int? id)
        {
            SatisFirsati model = new SatisFirsati();
            if (id.HasValue)
            {
                model = Db.SatisFirsatis.SingleOrDefault(x => x.Id == id);
            }
            ViewBag.RefDomainKategoriId = new SelectList(Db.DomainKategoris, "DomainKategoriID", "DomainKategoriAdi",model.RefDomainKategoriId);
            //ViewBag.RefFirmaId = new SelectList(Db.Firmas, "FirmaID", "FirmaAdi");
            ViewBag.RefYetkiliId = new SelectList(Db.FirmaKisis, "Id", "Ad",model.RefYetkiliId);
            ViewBag.RefAsamaId = new SelectList(Db.Etikets.Where(x=>x.Kategori=="RefAsamaId"), "Value", "Text",model.RefAsamaId);
            return View(model);
        }

        // POST: SatisFirsatis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet([Bind(Include = "Id,RefFirmaId,RefAsamaId,Fiyat,RefDomainKategoriId,RefYetkiliId,Tarih,Note,GecerlilikSuresi,DosyaYolu,RefSorumluKisiId")]
        SatisFirsati satisFirsati,int? id)
        {
            //if (ModelState.IsValid)
            //{      
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
      
            //}

            //ViewBag.RefDomainKategoriId = new SelectList(Db.DomainKategoris, "DomainKategoriID", "DomainKategoriAdi", satisFirsati.RefDomainKategoriId);
            //ViewBag.RefFirmaId = new SelectList(Db.Firmas, "FirmaID", "FirmaAdi", satisFirsati.RefFirmaId);
            //ViewBag.RefYetkiliId = new SelectList(Db.FirmaKisis, "Id", "Ad", satisFirsati.RefYetkiliId);
            //ViewBag.RefAsamaId = new SelectList(Db.SatisFirsatiAsamas, "Id", "Ad", satisFirsati.RefAsamaId);
            //return View(satisFirsati);
        }

        // GET: SatisFirsatis/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.RefDomainKategoriId = new SelectList(Db.DomainKategoris, "DomainKategoriID", "DomainKategoriAdi", satisFirsati.RefDomainKategoriId);
            ViewBag.RefFirmaId = new SelectList(Db.Firmas, "FirmaID", "FirmaAdi", satisFirsati.RefFirmaId);
            ViewBag.RefYetkiliId = new SelectList(Db.FirmaKisis, "Id", "Ad", satisFirsati.RefYetkiliId);
           
            ViewBag.RefAsamaId = new SelectList(Db.Etikets.Where(x => x.Kategori == "RefAsamaId"), "Value", "Text", satisFirsati.RefAsamaId);
            return View(satisFirsati);
        }

        // POST: SatisFirsatis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RefFirmaId,RefAsamaId,Fiyat,RefDomainKategoriId,RefYetkiliId,Tarih,Note,GecerlilikSuresi,DuzeltmeTarihi,DosyaYolu,RefSorumluKisiId,RefEkleyenKisiId,ReferansNo")] SatisFirsati satisFirsati)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(satisFirsati).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("List");
            }
            ViewBag.RefDomainKategoriId = new SelectList(Db.DomainKategoris, "DomainKategoriID", "DomainKategoriAdi", satisFirsati.RefDomainKategoriId);
            ViewBag.RefFirmaId = new SelectList(Db.Firmas, "FirmaID", "FirmaAdi", satisFirsati.RefFirmaId);
            ViewBag.RefYetkiliId = new SelectList(Db.FirmaKisis, "Id", "Ad", satisFirsati.RefYetkiliId);
          
            ViewBag.RefAsamaId = new SelectList(Db.Etikets.Where(x => x.Kategori == "RefAsamaId"), "Value", "Text", satisFirsati.RefAsamaId);
            return View(satisFirsati);
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
