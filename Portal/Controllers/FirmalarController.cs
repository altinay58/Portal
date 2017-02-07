using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Portal.Models;
using AutoMapper;

namespace Portal.Controllers
{
    public class FirmalarController : BaseController
    {
        public FirmalarController()
        {
            ViewBag.guncelMenu = "Firmalar";
        }
        public ActionResult Tumu(int? sayfaNo,string q)
        {
            //if (!User.IsInRole("Muhasebe") && durum != "musteri")
            //{

            //    TempData["KirmiziMesaj"] = "Bu bölüme giriş yetkiniz bulunmuyor.";
            //    return RedirectToAction("Index", "Home");
            //}

            ViewBag.SayfaAdi = "Firmalar";
            ViewBag.Durum = "tumu";


            int SayfaNo = sayfaNo ?? 0;


            int domainBaslangic = 0;
            if (SayfaNo > 1)
            {
                domainBaslangic = (SayfaNo - 1) * PagerCount;
            }

            var viewData = Db.Firmas.TumFirmalar()
                .Where(x => !string.IsNullOrEmpty(q) ? x.FirmaAdi.Contains(q) : true)
                .Skip(domainBaslangic).Take(PagerCount).ToList();
                
                //.GetirFirmalar(PagerCount, domainBaslangic, "tumu");


            ViewBag.BulunduguSayfa = SayfaNo;
            int totalCount = Db.Firmas.TumFirmalar()
                .Where(x => !string.IsNullOrEmpty(q) ? x.FirmaAdi.Contains(q) : true).Count();
            PaginatedList pager = new PaginatedList((sayfaNo ?? 1), PagerCount, totalCount);
            //sayfalama
            ViewData["queryData"] = q;
            ViewBag.Sayfalama = pager;
            //ViewBag.Domainler = Db.Domains.GetirDomainler(id);

            return View(viewData);
        }
        #region firmakisilist
        public ActionResult FirmaKisiList()
        {
            return View();
        }
        public JsonResult FirmaListAra(string ad, string soyad, string firmaAdi, string departman)
        {
            var list = (from fl in Db.FirmaKisis.Include(x=>x.Firma)
                        orderby fl.Firma.FirmaAdi descending
                        where
                        (!string.IsNullOrEmpty(ad) ? fl.Ad.Contains(ad) : true)
            && (!string.IsNullOrEmpty(soyad) ? fl.Soyad.Contains(soyad) : true)
                            && (!string.IsNullOrEmpty(firmaAdi) ? fl.Firma.FirmaAdi.Contains(firmaAdi) : true)
                            && (!string.IsNullOrEmpty(departman) ? fl.Departman.Contains(departman) : true)
                        select new
                        {
                            Id=fl.Id,Ad=fl.Ad,Soyad=fl.Soyad,Departman=fl.Departman,
                            Tel=fl.Tel,Email=fl.Email,FirmaAdi=fl.Firma.FirmaAdi
                        }
                       );
            //list = list.Where(x => (!string.IsNullOrEmpty(x.Ad) ? x.Ad.Contains(ad):true)
            //&& (!string.IsNullOrEmpty(x.Soyad) ? x.Soyad.Contains(soyad) : true)
            //                && (!string.IsNullOrEmpty(x.FirmaAdi) ? x.FirmaAdi.Contains(firmaAdi) : true) 
            //                && (!string.IsNullOrEmpty(x.Departman) ? x.Departman.Contains(departman) : true));
            return Json(list.ToList(),JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region firmaksi ekle
        public ActionResult FirmaKisiEkle(int? id)
        {
            FirmaKisi model = new FirmaKisi();
            if (id.HasValue)
            {
                model = Db.FirmaKisis.SingleOrDefault(x=>x.Id==id);
            }
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult FirmaKisiEkle(FirmaKisi model)
        {
            FirmaKisi entity = new FirmaKisi();
            if (model.Id>0)
            {
                entity = Db.FirmaKisis.SingleOrDefault(x => x.Id == model.Id);
            }
            entity.Ad = model.Ad;
            entity.Soyad = model.Soyad;
            entity.Departman = model.Departman;
            entity.Email = model.Email;
            entity.FirmaId = model.FirmaId;
            entity.Tel = model.Tel;
            if (model.Id > 0)
            {
                Db.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                Db.FirmaKisis.Add(entity);
            }
            Db.SaveChanges();
            TempData[SUCESS] = "Firma Kisi Kaydedildi";
            return View("FirmaKisiList");
        }
        #endregion
    }
}