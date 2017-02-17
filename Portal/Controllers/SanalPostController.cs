using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Portal.Models;
using System.Net;

namespace Portal.Controllers
{
    public class SanalPostController : BaseController
    {
        public SanalPostController()
        {
            ViewBag.guncelMenu = "ArgeBolumu";
        }
        // GET: SanalPost
        public ActionResult List()
        {
            var sanalpos = Db.SanalPos.Include(s => s.Firma);
            return View(sanalpos.ToList());
        }
        public ActionResult Create()
        {
            ViewBag.firmaID = new SelectList(Db.Firmas, "FirmaID", "FirmaAdi");
            return View();
        }

        // POST: /SanalPos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,firmaID,banka,magaza,kullaniciAdi,sifre,parola")] SanalPos sanalpos)
        {
            if (ModelState.IsValid)
            {
                Db.SanalPos.Add(sanalpos);
                Db.SaveChanges();
                return RedirectToAction("List");
            }

            ViewBag.firmaID = new SelectList(Db.Firmas, "FirmaID", "FirmaAdi", sanalpos.firmaID);
            return View(sanalpos);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanalPos sanalpos = Db.SanalPos.Find(id);
            if (sanalpos == null)
            {
                return HttpNotFound();
            }
            ViewBag.firmaID = new SelectList(Db.Firmas, "FirmaID", "FirmaAdi", sanalpos.firmaID);
            return View(sanalpos);
        }

        // POST: /SanalPos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,firmaID,banka,magaza,kullaniciAdi,sifre,parola")] SanalPos sanalpos)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(sanalpos).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("List");
            }
            ViewBag.firmaID = new SelectList(Db.Firmas, "FirmaID", "FirmaAdi", sanalpos.firmaID);
            return View(sanalpos);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanalPos sanalpos = Db.SanalPos.Find(id);
            if (sanalpos == null)
            {
                return HttpNotFound();
            }
            return View(sanalpos);
        }

        // POST: /SanalPos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanalPos sanalpos = Db.SanalPos.Find(id);
            Db.SanalPos.Remove(sanalpos);
            Db.SaveChanges();
            return RedirectToAction("List");
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanalPos sanalpos = Db.SanalPos.Find(id);
            if (sanalpos == null)
            {
                return HttpNotFound();
            }
            return View(sanalpos);
        }

    }
}