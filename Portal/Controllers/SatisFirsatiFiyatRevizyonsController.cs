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
    public class SatisFirsatiFiyatRevizyonsController : BaseController
    {
      

        // GET: SatisFirsatiFiyatRevizyons
        public ActionResult Index()
        {
            var satisFirsatiFiyatRevizyons = Db.SatisFirsatiFiyatRevizyons.Include(s => s.SatisFirsati);
            return View(satisFirsatiFiyatRevizyons.ToList());
        }

        // GET: SatisFirsatiFiyatRevizyons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SatisFirsatiFiyatRevizyon satisFirsatiFiyatRevizyon = Db.SatisFirsatiFiyatRevizyons.Find(id);
            if (satisFirsatiFiyatRevizyon == null)
            {
                return HttpNotFound();
            }
            return View(satisFirsatiFiyatRevizyon);
        }

        // GET: SatisFirsatiFiyatRevizyons/Create
        public ActionResult Kaydet(int? id,int satisFirsatId)
        {
            ViewBag.listRevizyonlar = Db.SatisFirsatiFiyatRevizyons.Include(s => s.SatisFirsati).Where(x=>x.RefSatisFirsatiId==satisFirsatId).ToList();
            SatisFirsatiFiyatRevizyon model = new SatisFirsatiFiyatRevizyon();
            if(id.HasValue)
            {
                model = Db.SatisFirsatiFiyatRevizyons.SingleOrDefault(x => x.Id == id);
            }
            ViewBag.satisId = satisFirsatId;
            //ViewBag.RefSatisFirsatiId = new SelectList(Db.SatisFirsatis, "Id", "Note");
            return View(model);
        }

        // POST: SatisFirsatiFiyatRevizyons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet([Bind(Include = "Id,Fiyat")] SatisFirsatiFiyatRevizyon satisFirsatiFiyatRevizyon,
            int?id,int satisFirsatId)
        {
            if (ModelState.IsValid)
            {
                if(id==null || id == 0)
                {
                    Db.SatisFirsatiFiyatRevizyons.Add(satisFirsatiFiyatRevizyon);
                }
                else
                {
                    Db.Entry(satisFirsatiFiyatRevizyon).State = EntityState.Modified;
                }
                satisFirsatiFiyatRevizyon.RefSatisFirsatiId = satisFirsatId;
                satisFirsatiFiyatRevizyon.Tarih = DateTime.Now;
                Db.SaveChanges();
                if (!string.IsNullOrEmpty(Request.UrlReferrer.AbsolutePath))
                {
                    return Redirect(Request.UrlReferrer.AbsolutePath);
                }else
                {
                    return RedirectToAction("Index");
                }
              
            }
            ViewBag.listRevizyonlar = Db.SatisFirsatiFiyatRevizyons.Include(s => s.SatisFirsati).Where(x => x.RefSatisFirsatiId == satisFirsatId);
            ViewBag.satisId = satisFirsatId;
            ViewBag.RefSatisFirsatiId = new SelectList(Db.SatisFirsatis, "Id", "Note", satisFirsatiFiyatRevizyon.RefSatisFirsatiId);
            return View(satisFirsatiFiyatRevizyon);
        }

        // GET: SatisFirsatiFiyatRevizyons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SatisFirsatiFiyatRevizyon satisFirsatiFiyatRevizyon = Db.SatisFirsatiFiyatRevizyons.Find(id);
            if (satisFirsatiFiyatRevizyon == null)
            {
                return HttpNotFound();
            }
            ViewBag.RefSatisFirsatiId = new SelectList(Db.SatisFirsatis, "Id", "Note", satisFirsatiFiyatRevizyon.RefSatisFirsatiId);
            return View(satisFirsatiFiyatRevizyon);
        }

        // POST: SatisFirsatiFiyatRevizyons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RefSatisFirsatiId,Fiyat,Tarih")] SatisFirsatiFiyatRevizyon satisFirsatiFiyatRevizyon)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(satisFirsatiFiyatRevizyon).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RefSatisFirsatiId = new SelectList(Db.SatisFirsatis, "Id", "Note", satisFirsatiFiyatRevizyon.RefSatisFirsatiId);
            return View(satisFirsatiFiyatRevizyon);
        }

        // GET: SatisFirsatiFiyatRevizyons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SatisFirsatiFiyatRevizyon satisFirsatiFiyatRevizyon = Db.SatisFirsatiFiyatRevizyons.Find(id);
            if (satisFirsatiFiyatRevizyon == null)
            {
                return HttpNotFound();
            }
            return View(satisFirsatiFiyatRevizyon);
        }

        // POST: SatisFirsatiFiyatRevizyons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SatisFirsatiFiyatRevizyon satisFirsatiFiyatRevizyon = Db.SatisFirsatiFiyatRevizyons.Find(id);
            Db.SatisFirsatiFiyatRevizyons.Remove(satisFirsatiFiyatRevizyon);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

       
    }
}
