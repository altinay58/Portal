using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Portal.Models;

namespace Portal.Controllers
{
    public class StandartProjeIslerisController : Controller
    {
        private PortalEntities db = new PortalEntities();

        // GET: StandartProjeIsleris
        public ActionResult Index()
        {
            var standartProjeIsleris = db.StandartProjeIsleris.Include(s => s.AspNetUser).Include(s => s.AspNetUser1);
            return View(standartProjeIsleris.ToList());
        }

        // GET: StandartProjeIsleris/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StandartProjeIsleri standartProjeIsleri = db.StandartProjeIsleris.Find(id);
            if (standartProjeIsleri == null)
            {
                return HttpNotFound();
            }
            return View(standartProjeIsleri);
        }

        // GET: StandartProjeIsleris/Create
        public ActionResult Create()
        {
            ViewBag.RefStandartProjeIsleriYapacakKullaniciId = new SelectList(db.AspNetUsers.TumKullanicilar().ToList(), "Id", "Isim");
            ViewBag.RefStandartProjeIsleriKontrolEdecekKullaniciId = new SelectList(db.AspNetUsers.TumKullanicilar().ToList(), "Id", "Isim");
            return View();
        }

        // POST: StandartProjeIsleris/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( StandartProjeIsleri standartProjeIsleri)
        {
            if (ModelState.IsValid)
            {
                db.StandartProjeIsleris.Add(standartProjeIsleri);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RefStandartProjeIsleriYapacakKullaniciId = new SelectList(db.AspNetUsers, "Id", "UserName", standartProjeIsleri.RefStandartProjeIsleriYapacakKullaniciId);
            ViewBag.RefStandartProjeIsleriKontrolEdecekKullaniciId = new SelectList(db.AspNetUsers, "Id", "UserName", standartProjeIsleri.RefStandartProjeIsleriKontrolEdecekKullaniciId);
            return View(standartProjeIsleri);
        }

        // GET: StandartProjeIsleris/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StandartProjeIsleri standartProjeIsleri = db.StandartProjeIsleris.Find(id);
            if (standartProjeIsleri == null)
            {
                return HttpNotFound();
            }
            ViewBag.RefStandartProjeIsleriYapacakKullaniciId = new SelectList(db.AspNetUsers, "Id", "UserName", standartProjeIsleri.RefStandartProjeIsleriYapacakKullaniciId);
            ViewBag.RefStandartProjeIsleriKontrolEdecekKullaniciId = new SelectList(db.AspNetUsers, "Id", "UserName", standartProjeIsleri.RefStandartProjeIsleriKontrolEdecekKullaniciId);
            return View(standartProjeIsleri);
        }

        // POST: StandartProjeIsleris/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StandartProjeIsleri standartProjeIsleri)
        {
            if (ModelState.IsValid)
            {
                standartProjeIsleri.StandartProjeIsleriTarih = DateTime.Now;
                db.Entry(standartProjeIsleri).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RefStandartProjeIsleriYapacakKullaniciId = new SelectList(db.AspNetUsers, "Id", "UserName", standartProjeIsleri.RefStandartProjeIsleriYapacakKullaniciId);
            ViewBag.RefStandartProjeIsleriKontrolEdecekKullaniciId = new SelectList(db.AspNetUsers, "Id", "UserName", standartProjeIsleri.RefStandartProjeIsleriKontrolEdecekKullaniciId);
            return View(standartProjeIsleri);
        }

        // GET: StandartProjeIsleris/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StandartProjeIsleri standartProjeIsleri = db.StandartProjeIsleris.Find(id);
            if (standartProjeIsleri == null)
            {
                return HttpNotFound();
            }
            return View(standartProjeIsleri);
        }

        // POST: StandartProjeIsleris/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StandartProjeIsleri standartProjeIsleri = db.StandartProjeIsleris.Find(id);
            db.StandartProjeIsleris.Remove(standartProjeIsleri);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
