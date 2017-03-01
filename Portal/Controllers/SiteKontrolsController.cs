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
    public class SiteKontrolsController : BaseController
    {
        // GET: SiteKontrols
        public SiteKontrolsController()
        {
            GuncelMenu = "Yonetimsel";
        }
        public ActionResult List()
        {
            return View(Db.SiteKontrols);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteKontrol siteKontrol = Db.SiteKontrols.Find(id);
            if (siteKontrol == null)
            {
                return HttpNotFound();
            }
            return View(siteKontrol);
        }
        public ActionResult Create()
        {
            return View();
        }

        // POST: SiteKontrols/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SiteKontrolID,SiteKontrolAdi,SiteKontrolAciklama,SiteKontrolSira")] SiteKontrol siteKontrol)
        {
            if (ModelState.IsValid)
            {
                Db.SiteKontrols.Add(siteKontrol);
                Db.SaveChanges();
                return RedirectToAction("List");
            }

            return View(siteKontrol);
        }
              // GET: SiteKontrols/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteKontrol siteKontrol = Db.SiteKontrols.Find(id);
            if (siteKontrol == null)
            {
                return HttpNotFound();
            }
            return View(siteKontrol);
        }

        // POST: SiteKontrols/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SiteKontrolID,SiteKontrolAdi,SiteKontrolAciklama,SiteKontrolSira")] SiteKontrol siteKontrol)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(siteKontrol).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(siteKontrol);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteKontrol siteKontrol = Db.SiteKontrols.Find(id);
            if (siteKontrol == null)
            {
                return HttpNotFound();
            }
            return View(siteKontrol);
        }

        // POST: SiteKontrols/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SiteKontrol siteKontrol = Db.SiteKontrols.Find(id);
            Db.SiteKontrols.Remove(siteKontrol);
            Db.SaveChanges();
            return RedirectToAction("List");
        }
        public JsonResult SilinenDomainKontrol(int domainID, string kontrol, bool deger)
        {
            Domain domainKontrol = Db.Domains.Find(domainID);

            if (deger)
            {
                if (kontrol == "amazon")
                {
                    domainKontrol.SilindimiAmazon = true;
                }
                else if (kontrol == "hosting")
                {
                    domainKontrol.SilindimiHosting = true;

                }
                else if (kontrol == "googleBusiness")
                {
                    domainKontrol.SilindimiGoogleBusiness = true;

                }
                else if (kontrol == "webYoneticiAraclari")
                {
                    domainKontrol.SilindimiWebYoneticiAraclari = true;

                }
                else if (kontrol == "yedekAlindimi")
                {
                    domainKontrol.SilindimiYedekAlindimi = true;

                }
                else if (kontrol == "veriTabani")
                {
                    domainKontrol.SilindimiVeriTabani = true;

                }

            }
            else
            {
                if (kontrol == "amazon")
                {
                    domainKontrol.SilindimiAmazon = false;
                }
                else if (kontrol == "hosting")
                {
                    domainKontrol.SilindimiHosting = false;

                }
                else if (kontrol == "googleBusiness")
                {
                    domainKontrol.SilindimiGoogleBusiness = false;

                }
                else if (kontrol == "webYoneticiAraclari")
                {
                    domainKontrol.SilindimiWebYoneticiAraclari = false;

                }
                else if (kontrol == "yedekAlindimi")
                {
                    domainKontrol.SilindimiYedekAlindimi = false;

                }
                else if (kontrol == "veriTabani")
                {
                    domainKontrol.SilindimiVeriTabani = false;

                }
            }
            Db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }


    }
}