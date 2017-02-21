using Portal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    [Authorize(Roles = "Herkes")]
    public class SifresController : BaseController
    {
        public SifresController()
        {
            ViewBag.guncelMenu = "ArgeBolumu";
        }
        // GET: Sifres
        public ActionResult List()
        {
            return View(Db.Sifres.OrderByDescending(s => s.SifreID).ToList());
        }
        public JsonResult SifreAra(string q)
        {
            var list = Db.Sifres.Where(x => x.SifreWebSitesi.Contains(q) ||
              x.SifreUserID.Contains(q) || x.SifreSifre.Contains(q) || x.SifreKullaniciAdi.Contains(q)
              || x.SifreGmailAdresi.Contains(q) || x.SifreDiger1.Contains(q)
              ||x.SifreDiger2.Contains(q)).OrderByDescending(x => x.SifreID).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sifre sifre = Db.Sifres.Find(id);
            if (sifre == null)
            {
                return HttpNotFound();
            }
            return View(sifre);
        }
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sifres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SifreID,SifreUserID,SifreWebSitesi,SifreKullaniciAdi,SifreSifre,SifreGmail,SifreGmailAdresi,SifreDiger1,SifreDiger2,SifreDiger3,SifreDiger4,SifreDiger5,SifreDiger6")] Sifre sifre)
        {
            if (ModelState.IsValid)
            {
                Db.Sifres.Add(sifre);
                Db.SaveChanges();
                return RedirectToAction("List");
            }

            return View(sifre);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sifre sifre = Db.Sifres.Find(id);
            if (sifre == null)
            {
                return HttpNotFound();
            }
            return View(sifre);
        }

        // POST: Sifres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SifreID,SifreUserID,SifreWebSitesi,SifreKullaniciAdi,SifreSifre,SifreGmail,SifreGmailAdresi,SifreDiger1,SifreDiger2,SifreDiger3,SifreDiger4,SifreDiger5,SifreDiger6")] Sifre sifre)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(sifre).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(sifre);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sifre sifre = Db.Sifres.Find(id);
            if (sifre == null)
            {
                return HttpNotFound();
            }
            return View(sifre);
        }

        // POST: Sifres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sifre sifre = Db.Sifres.Find(id);
            Db.Sifres.Remove(sifre);
            Db.SaveChanges();
            return RedirectToAction("List");
        }

    }
}