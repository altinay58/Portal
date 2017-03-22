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
    public class EtiketsController : BaseController
    {
        private PortalEntities db = new PortalEntities();

        // GET: Etikets
        public ActionResult Index()
        {
            var data = db.Etikets.ToList();
            CacheManagement.SetCache(CacheKeys.ETIKETS, data);
            return View(data);
        }

        // GET: Etikets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Etiket etiket = db.Etikets.Find(id);
            if (etiket == null)
            {
                return HttpNotFound();
            }
            return View(etiket);
        }

        // GET: Etikets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Etikets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Kategori,Text,Sira,Renk,FontIcon,Value")] Etiket etiket)
        {
            if (ModelState.IsValid)
            {
                db.Etikets.Add(etiket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(etiket);
        }

        // GET: Etikets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Etiket etiket = db.Etikets.Find(id);
            if (etiket == null)
            {
                return HttpNotFound();
            }
            return View(etiket);
        }

        // POST: Etikets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Kategori,Text,Sira,Renk,FontIcon,Value")] Etiket etiket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(etiket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(etiket);
        }

        // GET: Etikets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Etiket etiket = db.Etikets.Find(id);
            if (etiket == null)
            {
                return HttpNotFound();
            }
            return View(etiket);
        }

        // POST: Etikets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Etiket etiket = db.Etikets.Find(id);
            db.Etikets.Remove(etiket);
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
