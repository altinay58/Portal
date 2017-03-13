using Portal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class HtmlTasarimlarController : BaseController
    {
        public HtmlTasarimlarController()
        {
            ViewBag.guncelMenu = "ArgeBolumu";
        }
        // GET: HtmlTasarimlar
        public ActionResult List()
        {
            var temas=Db.Temas.ToList();
            return View(temas);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tema tema = Db.Temas.Find(id);
            if (tema == null)
            {
                return HttpNotFound();
            }
            return View(tema);
        }
        public ActionResult Create()
        {

            ViewBag.temaDomainKategoriID = new SelectList(Db.DomainKategoris, "DomainKategoriID", "DomainKategoriAdi");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tema tema)
        {
            if (ModelState.IsValid)
            {

                var file = Request.Files["ResimYolu"];
                if (file != null && file.ContentLength > 0)
                {
                    string filePath = Path.Combine(Server.MapPath("~/upload"), file.FileName);
                    file.SaveAs(filePath);
                    tema.ResimYolu = "/upload/" + file.FileName;
                }

                Db.Temas.Add(tema);
                Db.SaveChanges();
                return RedirectToAction("List");
            }


            ViewBag.temaDomainKategoriID = new SelectList(Db.DomainKategoris, "DomainKategoriID", "DomainKategoriAdi", tema.temaDomainKategoriID);
            return View(tema);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tema tema = Db.Temas.Find(id);
            if (tema == null)
            {
                return HttpNotFound();
            }

            ViewBag.temaDomainKategoriID = new SelectList(Db.DomainKategoris, "DomainKategoriID", "DomainKategoriAdi");
            return View(tema);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "temaID,temaAdresi,temaDemoAdi,temaProjeTipi,temaFirmaAdi")] Tema tema)
        {
            var entity = Db.Temas.AsNoTracking().SingleOrDefault(x => x.temaID == tema.temaID);
            if (ModelState.IsValid)
            {
                var file = Request.Files["ResimYolu"];
                if (file != null && file.ContentLength > 0)
                {
                    string filePath = Path.Combine(Server.MapPath("~/upload"), file.FileName);
                    file.SaveAs(filePath);
                    tema.ResimYolu = "/upload/"+file.FileName;
                }
                else
                {
                    tema.ResimYolu = entity.ResimYolu;
                }
                Db.Entry(tema).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("List");
            }

            ViewBag.temaDomainKategoriID = new SelectList(Db.DomainKategoris, "DomainKategoriID", "DomainKategoriAdi", tema.temaDomainKategoriID);
            return View(tema);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tema tema = Db.Temas.Find(id);
            if (tema == null)
            {
                return HttpNotFound();
            }
            return View(tema);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tema tema = Db.Temas.Find(id);
            Db.Temas.Remove(tema);
            Db.SaveChanges();
            return RedirectToAction("List");
        }
    }
}