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
    [Authorize(Roles = "Yonetim")]
    public class ToplantisController : Controller
    {
        private PortalEntities db = new PortalEntities();

        [Authorize]
        public ActionResult Index()
        {
            string userid = User.Identity.GetUserId();

            if(User.IsInRole("Yonetim"))
            {
                return View(db.Toplantis.ToList());
            }
            else
            {
                List<int> toplantiIdleri = db.ToplantiyaKatilanlars.Where(a => a.RefAspNetUsers == userid).Select(x => x.RefToplantiId).ToList();
                return View(db.Toplantis.Where(a => toplantiIdleri.Contains(a.ToplantiId)).ToList());
            }
        }


        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Toplanti toplanti = db.Toplantis.Find(id);

            if (toplanti == null)
            {
                return HttpNotFound();
            }

            string userid = User.Identity.GetUserId();


            if (User.IsInRole("Yonetim"))
            {
                ToplantiyaKatilanlar katilim = db.ToplantiyaKatilanlars.FirstOrDefault(a => a.RefToplantiId == toplanti.ToplantiId && a.RefAspNetUsers == userid);

                if (katilim != null)
                {
                    ViewBag.Gorusler = "YonetimKatilmadi";
                }
                else
                {
                    ViewBag.Gorusler = katilim.ToplantiYorumu;
                }

                ViewBag.Tutanaklar = db.ToplantiyaKatilanlars.Where(a => a.RefToplantiId == toplanti.ToplantiId).ToList(); 

                return View(toplanti);
            }
            else
            {
                ToplantiyaKatilanlar katilim = db.ToplantiyaKatilanlars.FirstOrDefault(a => a.RefToplantiId == toplanti.ToplantiId && a.RefAspNetUsers == userid);

                if (katilim != null)
                {
                    TempData["Success"] = "Bu toplantıya katılmadınız. Lütfen katıldığınız toplantılara bakın...";

                    return RedirectToAction("Index");
                }

                ViewBag.Gorusler = katilim.ToplantiYorumu;

                return View(toplanti);
            }
        }


        [Authorize]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Details(int? id, string ToplantiYorumu)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Toplanti toplanti = db.Toplantis.Find(id);

            string userid = User.Identity.GetUserId();

            ToplantiyaKatilanlar katilim = db.ToplantiyaKatilanlars.FirstOrDefault(a => a.RefToplantiId == toplanti.ToplantiId && a.RefAspNetUsers == userid);

            katilim.ToplantiYorumu = ToplantiYorumu;

            db.SaveChanges();

            return RedirectToAction("Details", new { id = id });
        }


        public ActionResult Create()
        {
            ViewBag.Kullanicilar = db.AspNetUsers.Where(x => x.LockoutEnabled == false).ToList();
            return View();
        }


        [HttpPost]
        public ActionResult Create(Toplanti toplanti)
        {
            List<string> toplantiyaKatilanlar = Request["ToplantiyaKatilanlar"].Split(',').ToList();

            foreach (string userId in toplantiyaKatilanlar)
            {
                ToplantiyaKatilanlar kisi = new ToplantiyaKatilanlar();
                kisi.RefAspNetUsers = userId;
                toplanti.ToplantiyaKatilanlars.Add(kisi);
            }

            db.Toplantis.Add(toplanti);
            db.SaveChanges();

            return View(toplanti);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Toplanti toplanti = db.Toplantis.Find(id);
            if (toplanti == null)
            {
                return HttpNotFound();
            }
            return View(toplanti);
        }

        // POST: Toplantis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ToplantiId,ToplandiAdi,ToplantiDetayi,ToplantiTarihi")] Toplanti toplanti)
        {
            if (ModelState.IsValid)
            {
                db.Entry(toplanti).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(toplanti);
        }

        // GET: Toplantis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Toplanti toplanti = db.Toplantis.Find(id);
            if (toplanti == null)
            {
                return HttpNotFound();
            }
            return View(toplanti);
        }

        // POST: Toplantis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Toplanti toplanti = db.Toplantis.Find(id);
            db.Toplantis.Remove(toplanti);
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
