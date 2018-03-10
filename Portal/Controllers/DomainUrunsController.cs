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
    [Authorize(Roles = "Yonetim")]
    public class DomainUrunsController : Controller
    {
        private PortalEntities db = new PortalEntities();

        // GET: DomainUruns
        public ActionResult Index()
        {
            return View(db.DomainUruns.ToList());
        }

        // GET: DomainUruns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DomainUrun domainUrun = db.DomainUruns.Find(id);
            if (domainUrun == null)
            {
                return HttpNotFound();
            }
            return View(domainUrun);
        }

        // GET: DomainUruns/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DomainUruns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DomainUrunID,DomainUrunAdi,DomainUrunFiyati")] DomainUrun domainUrun)
        {
            if (ModelState.IsValid)
            {
                db.DomainUruns.Add(domainUrun);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(domainUrun);
        }

        // GET: DomainUruns/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DomainUrun domainUrun = db.DomainUruns.Find(id);
            if (domainUrun == null)
            {
                return HttpNotFound();
            }
            return View(domainUrun);
        }

        // POST: DomainUruns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DomainUrunID,DomainUrunAdi,DomainUrunFiyati")] DomainUrun domainUrun)
        {
            if (ModelState.IsValid)
            {
                db.Entry(domainUrun).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(domainUrun);
        }

        // GET: DomainUruns/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DomainUrun domainUrun = db.DomainUruns.Find(id);
            if (domainUrun == null)
            {
                return HttpNotFound();
            }
            return View(domainUrun);
        }

        // POST: DomainUruns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DomainUrun domainUrun = db.DomainUruns.Find(id);
            db.DomainUruns.Remove(domainUrun);
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
