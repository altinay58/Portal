using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Portal.Models;
using Microsoft.AspNet.Identity;

namespace Portal.Controllers
{
    public class SatisGorusmesController : BaseController
    {
      
        public SatisGorusmesController()
        {
            GuncelMenu = "Satis Bolumu";
        }
        // GET: SatisGorusmes
        public ActionResult List()
        {
            var satisGorusmes = Db.SatisGorusmes.Include(s => s.DomainKategori).Include(s => s.Firma).Include(s => s.FirmaKisi);
            return View(satisGorusmes.ToList());
        }

        // GET: SatisGorusmes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SatisGorusme satisGorusme = Db.SatisGorusmes.Find(id);
            if (satisGorusme == null)
            {
                return HttpNotFound();
            }
            return View(satisGorusme);
        }

        // GET: SatisGorusmes/Create
        public ActionResult Kaydet(int? id)
        {
            SatisGorusme model = new SatisGorusme();
            if (id.HasValue)
            {
                model = Db.SatisGorusmes.SingleOrDefault(x => x.Id == id);
            }
            var butunEtiketler = CacheManagement.Get<Etiket>(CacheKeys.ETIKETS);
            ViewBag.RefDomainKategoriId = new SelectList(Db.DomainKategoris, "DomainKategoriID", "DomainKategoriAdi",model.RefDomainKategoriId);
        
            //ViewBag.RefYetkiliId = new SelectList(Db.FirmaKisis, "Id", "Ad");

            ViewBag.EtiketSatisGorusmeTypeId = new SelectList(butunEtiketler.Where(x => x.Kategori == "EtiketSatisGorusmeTypeId").OrderBy(x => x.Sira), "Value", "Text", model.EtiketSatisGorusmeTypeId);
            return View(model);
        }

        // POST: SatisGorusmes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet([Bind(Include = "Id,EtiketSatisGorusmeTypeId,RefDomainKategoriId,RefFirmaId,RefYetkiliId,Tarih,Note")]
        SatisGorusme satisGorusme,int? id)
        {
            if (ModelState.IsValid)
            {
                if (id == null || id==0)
                {
                    satisGorusme.RefEkleyenKisiId = User.Identity.GetUserId();
                    Db.SatisGorusmes.Add(satisGorusme);
                }
                else
                {
                    Db.Entry(satisGorusme).State = EntityState.Modified;
                }
              
                Db.SaveChanges();
                TempData[SUCESS] = $"Kaydedildi {satisGorusme.Id}";
                return RedirectToAction("List");
            }

            ViewBag.RefDomainKategoriId = new SelectList(Db.DomainKategoris, "DomainKategoriID", "DomainKategoriAdi", satisGorusme.RefDomainKategoriId);
            ViewBag.RefFirmaId = new SelectList(Db.Firmas, "FirmaID", "FirmaAdi", satisGorusme.RefFirmaId);
            ViewBag.RefYetkiliId = new SelectList(Db.FirmaKisis, "Id", "Ad", satisGorusme.RefYetkiliId);
            return View(satisGorusme);
        }

        // GET: SatisGorusmes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SatisGorusme satisGorusme = Db.SatisGorusmes.Find(id);
            if (satisGorusme == null)
            {
                return HttpNotFound();
            }
            ViewBag.RefDomainKategoriId = new SelectList(Db.DomainKategoris, "DomainKategoriID", "DomainKategoriAdi", satisGorusme.RefDomainKategoriId);
            ViewBag.RefFirmaId = new SelectList(Db.Firmas, "FirmaID", "FirmaAdi", satisGorusme.RefFirmaId);
            ViewBag.RefYetkiliId = new SelectList(Db.FirmaKisis, "Id", "Ad", satisGorusme.RefYetkiliId);
            return View(satisGorusme);
        }

        // POST: SatisGorusmes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EtiketSatisGorusmeTypeId,RefDomainKategoriId,RefFirmaId,RefYetkiliId,RefEkleyenKisiId,Tarih,Note")] SatisGorusme satisGorusme)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(satisGorusme).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("List");
            }
            ViewBag.RefDomainKategoriId = new SelectList(Db.DomainKategoris, "DomainKategoriID", "DomainKategoriAdi", satisGorusme.RefDomainKategoriId);
            ViewBag.RefFirmaId = new SelectList(Db.Firmas, "FirmaID", "FirmaAdi", satisGorusme.RefFirmaId);
            ViewBag.RefYetkiliId = new SelectList(Db.FirmaKisis, "Id", "Ad", satisGorusme.RefYetkiliId);
            return View(satisGorusme);
        }

        // GET: SatisGorusmes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SatisGorusme satisGorusme = Db.SatisGorusmes.Find(id);
            if (satisGorusme == null)
            {
                return HttpNotFound();
            }
            return View(satisGorusme);
        }

        // POST: SatisGorusmes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SatisGorusme satisGorusme = Db.SatisGorusmes.Find(id);
            Db.SatisGorusmes.Remove(satisGorusme);
            Db.SaveChanges();
            return RedirectToAction("List");
        }

      
    }
}
