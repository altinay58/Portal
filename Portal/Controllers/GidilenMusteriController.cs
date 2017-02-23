using Microsoft.AspNet.Identity;
using Portal.Models;
using Portal.Models.ArayanlarModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class GidilenMusteriController : BaseController
    {
        public GidilenMusteriController()
        {
            ViewBag.guncelMenu = "Arayanlar";
        }
        // GET: GidilenMusteri
        public ActionResult List(int?p)
        {                 
            int baslangic = ((p??1) - 1) * PagerCount;

            int totalCount = Db.GidilenMusteris.Count();
            PaginatedList pager = new PaginatedList((p ?? 1), PagerCount, totalCount);
           
            ViewBag.Sayfalama = pager;
            var gidilenmusteris = Db.GidilenMusteris.OrderByDescending(x=>x.GidilenMusteriTarih).Skip(baslangic).Take(PagerCount);

            return View(gidilenmusteris.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GidilenMusteri gidilenmusteri = Db.GidilenMusteris.Find(id);
            if (gidilenmusteri == null)
            {
                return HttpNotFound();
            }
            return View(gidilenmusteri);
        }
        public ActionResult Create()
        {
            ViewBag.GidilenMuteriGorusenKullaniciID = new SelectList(Db.AspNetUsers, "Id", "UserName");
            ViewBag.GidilenMusteriDomainKategoriID = new SelectList(Db.DomainKategoris, "DomainKategoriID", "DomainKategoriAdi");
            ViewBag.GidilenMusteriKonumID = new SelectList(Db.Konums, "KonumID", "Konum1");
            ViewBag.GidilenMusteriSektorID = new SelectList(Db.Sektorlers, "sektorID", "sektorAdi");
            return View();
        }

        // POST: /GidilenMusteri/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GidilenMusteriView yeniGidilenMusteri)
        {
            if (ModelState.IsValid)
            {
                GidilenMusteri gidilenMusterim = new GidilenMusteri();
                gidilenMusterim.GidilenMusteriGorusenKullaniciID = User.Identity.GetUserId();
                gidilenMusterim.GidilenMusteriFirmaAdi = yeniGidilenMusteri.GidilenMusteriFirmaAdi;
                gidilenMusterim.GidilenMusteriAdi = yeniGidilenMusteri.GidilenMusteriAdi;
                gidilenMusterim.GidilenMusteriSoyadi = yeniGidilenMusteri.GidilenMusteriSoyadi;
                gidilenMusterim.GidilenMusteriTelefon = yeniGidilenMusteri.GidilenMusteriTelefon;
                gidilenMusterim.GidilenMusteriCepTelNo = yeniGidilenMusteri.GidilenMusteriCepTelNo;
                gidilenMusterim.GidilenMusteriAdres = yeniGidilenMusteri.GidilenMusteriAdres;
                gidilenMusterim.GidilenMusteriMailAdres = yeniGidilenMusteri.GidilenMusteriMailAdres;
                gidilenMusterim.GidilenMusteriSehir = yeniGidilenMusteri.GidilenMusteriSehir;
                gidilenMusterim.GidilenMusteriilce = yeniGidilenMusteri.GidilenMusteriilce;
                gidilenMusterim.GidilenMusteriWebAdresi = yeniGidilenMusteri.GidilenMusteriWebAdresi;
                gidilenMusterim.GidilenMusteriKonumID = yeniGidilenMusteri.GidilenMusteriKonumID;
                gidilenMusterim.GidilenMusteriDomainKategoriID = yeniGidilenMusteri.GidilenMusteriDomainKategoriID;
                gidilenMusterim.GidilenMusteriSektorID = yeniGidilenMusteri.GidilenMusteriSektorID;


                Db.GidilenMusteris.Add(gidilenMusterim);
                Db.SaveChanges();
                return RedirectToAction("List");
            }

            ViewBag.GidilenMuteriGorusenKullaniciID = new SelectList(Db.AspNetUsers, "Id", "UserName", yeniGidilenMusteri.GidilenMusteriGorusenKullaniciID);
            ViewBag.GidilenMusteriDomainKategoriID = new SelectList(Db.DomainKategoris, "DomainKategoriID", "DomainKategoriAdi", yeniGidilenMusteri.GidilenMusteriDomainKategoriID);
            ViewBag.GidilenMusteriKonumID = new SelectList(Db.Konums, "KonumID", "Konum1", yeniGidilenMusteri.GidilenMusteriKonumID);
            ViewBag.GidilenMusteriSektorID = new SelectList(Db.Sektorlers, "sektorID", "sektorAdi", yeniGidilenMusteri.GidilenMusteriSektorID);
            return View(yeniGidilenMusteri);
        }
        // GET: /GidilenMusteri/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GidilenMusteri yeniGidilenMusteri = Db.GidilenMusteris.Find(id);
            GidilenMusteriView gidilenMusterim = new GidilenMusteriView();
            gidilenMusterim.GidilenMusteriID = yeniGidilenMusteri.GidilenMusteriID;
            gidilenMusterim.GidilenMusteriFirmaAdi = yeniGidilenMusteri.GidilenMusteriFirmaAdi;
            gidilenMusterim.GidilenMusteriAdi = yeniGidilenMusteri.GidilenMusteriAdi;
            gidilenMusterim.GidilenMusteriSoyadi = yeniGidilenMusteri.GidilenMusteriSoyadi;
            gidilenMusterim.GidilenMusteriTelefon = yeniGidilenMusteri.GidilenMusteriTelefon;
            gidilenMusterim.GidilenMusteriCepTelNo = yeniGidilenMusteri.GidilenMusteriCepTelNo;
            gidilenMusterim.GidilenMusteriAdres = yeniGidilenMusteri.GidilenMusteriAdres;
            gidilenMusterim.GidilenMusteriMailAdres = yeniGidilenMusteri.GidilenMusteriMailAdres;
            gidilenMusterim.GidilenMusteriSehir = yeniGidilenMusteri.GidilenMusteriSehir;
            gidilenMusterim.GidilenMusteriilce = yeniGidilenMusteri.GidilenMusteriilce;
            gidilenMusterim.GidilenMusteriWebAdresi = yeniGidilenMusteri.GidilenMusteriWebAdresi;
            gidilenMusterim.GidilenMusteriKonumID = yeniGidilenMusteri.GidilenMusteriKonumID;
            gidilenMusterim.GidilenMusteriDomainKategoriID = yeniGidilenMusteri.GidilenMusteriDomainKategoriID;
            gidilenMusterim.GidilenMusteriSektorID = yeniGidilenMusteri.GidilenMusteriSektorID;
            if (yeniGidilenMusteri == null)
            {
                return HttpNotFound();
            }
            ViewBag.GidilenMuteriGorusenKullaniciID = new SelectList(Db.AspNetUsers, "Id", "UserName", yeniGidilenMusteri.GidilenMusteriGorusenKullaniciID);
            ViewBag.GidilenMusteriDomainKategoriID = new SelectList(Db.DomainKategoris, "DomainKategoriID", "DomainKategoriAdi", yeniGidilenMusteri.GidilenMusteriDomainKategoriID);
            ViewBag.GidilenMusteriKonumID = new SelectList(Db.Konums, "KonumID", "Konum1", yeniGidilenMusteri.GidilenMusteriKonumID);
            ViewBag.GidilenMusteriSektorID = new SelectList(Db.Sektorlers, "sektorID", "sektorAdi", yeniGidilenMusteri.GidilenMusteriSektorID);
            return View(gidilenMusterim);
        }

        // POST: /GidilenMusteri/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GidilenMusteriView yeniGidilenMusteri)
        {
            if (ModelState.IsValid)
            {
                GidilenMusteri gidilenMusterim = Db.GidilenMusteris.Find(yeniGidilenMusteri.GidilenMusteriID);
                gidilenMusterim.GidilenMusteriID = yeniGidilenMusteri.GidilenMusteriID;
                gidilenMusterim.GidilenMusteriFirmaAdi = yeniGidilenMusteri.GidilenMusteriFirmaAdi;
                gidilenMusterim.GidilenMusteriAdi = yeniGidilenMusteri.GidilenMusteriAdi;
                gidilenMusterim.GidilenMusteriSoyadi = yeniGidilenMusteri.GidilenMusteriSoyadi;
                gidilenMusterim.GidilenMusteriTelefon = yeniGidilenMusteri.GidilenMusteriTelefon;
                gidilenMusterim.GidilenMusteriCepTelNo = yeniGidilenMusteri.GidilenMusteriCepTelNo;
                gidilenMusterim.GidilenMusteriAdres = yeniGidilenMusteri.GidilenMusteriAdres;
                gidilenMusterim.GidilenMusteriMailAdres = yeniGidilenMusteri.GidilenMusteriMailAdres;
                gidilenMusterim.GidilenMusteriSehir = yeniGidilenMusteri.GidilenMusteriSehir;
                gidilenMusterim.GidilenMusteriilce = yeniGidilenMusteri.GidilenMusteriilce;
                gidilenMusterim.GidilenMusteriWebAdresi = yeniGidilenMusteri.GidilenMusteriWebAdresi;
                gidilenMusterim.GidilenMusteriKonumID = yeniGidilenMusteri.GidilenMusteriKonumID;
                gidilenMusterim.GidilenMusteriDomainKategoriID = yeniGidilenMusteri.GidilenMusteriDomainKategoriID;
                gidilenMusterim.GidilenMusteriSektorID = yeniGidilenMusteri.GidilenMusteriSektorID;
                Db.Entry(gidilenMusterim).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("List");
            }
            ViewBag.GidilenMuteriGorusenKullaniciID = new SelectList(Db.AspNetUsers, "Id", "UserName", yeniGidilenMusteri.GidilenMusteriGorusenKullaniciID);
            ViewBag.GidilenMusteriDomainKategoriID = new SelectList(Db.DomainKategoris, "DomainKategoriID", "DomainKategoriAdi", yeniGidilenMusteri.GidilenMusteriDomainKategoriID);
            ViewBag.GidilenMusteriKonumID = new SelectList(Db.Konums, "KonumID", "Konum1", yeniGidilenMusteri.GidilenMusteriKonumID);
            ViewBag.GidilenMusteriSektorID = new SelectList(Db.Sektorlers, "sektorID", "sektorAdi", yeniGidilenMusteri.GidilenMusteriSektorID);
            return View(yeniGidilenMusteri);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GidilenMusteri gidilenmusteri = Db.GidilenMusteris.Find(id);
            if (gidilenmusteri == null)
            {
                return HttpNotFound();
            }
            return View(gidilenmusteri);
        }

        // POST: /GidilenMusteri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GidilenMusteri gidilenmusteri = Db.GidilenMusteris.Find(id);
            Db.GidilenMusteris.Remove(gidilenmusteri);
            Db.SaveChanges();
            return RedirectToAction("List");
        }
    }
}