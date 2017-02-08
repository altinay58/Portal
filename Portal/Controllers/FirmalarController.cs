﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Portal.Models;
using AutoMapper;
using System.Net;

namespace Portal.Controllers
{
    public class FirmalarController : BaseController
    {
        public FirmalarController()
        {
            ViewBag.guncelMenu = "Firmalar";
        }
        #region firma tumu
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
                .Where(x => !string.IsNullOrEmpty(q) ? x.FirmaAdi.Contains(q): true)
                .Skip(domainBaslangic).Take(PagerCount).ToList();
                
                //.GetirFirmalar(PagerCount, domainBaslangic, "tumu");


            ViewBag.BulunduguSayfa = SayfaNo;
            int totalCount = Db.Firmas.TumFirmalar()
                .Where(x => !string.IsNullOrEmpty(q) ? x.FirmaAdi.Contains(q) : true).Count();
            PaginatedList pager = new PaginatedList((sayfaNo ?? 1), PagerCount, totalCount);
            //sayfalama
            ViewData["queryData"] =  q;
            ViewBag.Sayfalama = pager;
            //ViewBag.Domainler = Db.Domains.GetirDomainler(id);

            return View(viewData);
        }
        public ActionResult FirmaSil(int id)
        {
            var firma = Db.Firmas.SingleOrDefault(x => x.FirmaID == id);
            Db.Firmas.Remove(firma);
            TempData[SUCESS] = "Kayıt Silindi";
            Db.SaveChanges();
            return RedirectToAction("Tumu");
        }
        #endregion firma tuu
        #region firmakaydet
        public ActionResult FirmaKaydet(int? id)
        {
            Firma firma = new Firma();
            if (id.HasValue)
            {
                firma = Db.Firmas.SingleOrDefault(x => x.FirmaID == id);
            }
            ViewBag.Firmalar = Db.Firmas.GetirFirmalar("tumu");
            ViewBag.Konumlar = Db.Konums.GetirKonumlar();
            return View(firma);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult FirmaKaydet(Firma model,FormCollection frm)
        {
            Firma firma = new Firma();
            if (model.FirmaID>0)
            {
                firma = Db.Firmas.SingleOrDefault(x => x.FirmaID == model.FirmaID);
            }
            firma.FirmaAdi = Temizle(model.FirmaAdi);
            firma.FirmaAdres = model.FirmaAdres;
            firma.FirmaVergiDairesi = model.FirmaVergiDairesi;
            firma.FirmaVergiNumarasi = model.FirmaVergiNumarasi;
            firma.FirmaAdres = model.FirmaAdres;
            firma.YetkiliAdi = model.YetkiliAdi;
            firma.YetkiliSoyAdi = model.YetkiliSoyAdi;
            firma.YetkiliTelefon = model.YetkiliTelefon;
            firma.YetkiliCepTelefon = model.YetkiliCepTelefon;
            firma.Email = model.Email;
            firma.RefFirmaID = model.RefFirmaID;
            firma.RefKonumID = model.RefKonumID;
            firma.Araci = Request["araci"] != null ? true : false;
            firma.Musteri = Request["musteri"] != null ? true : false;
            firma.Personel = Request["personel"] != null ? true : false;
            firma.Araci = Request["kasa"] != null ? true : false;
            if (model.FirmaID == 0)
            {
                firma.firmaKayitTarih = DateTime.Now;
                firma.firmaRefArayanID = model.firmaRefArayanID;
                if (model.firmaRefArayanID!=null)
                {      
                    //burasi arayanlar tablosundaki güncelemeri yapıyor
                    string firmaAdi = model.FirmaAdi.Replace(" ", "");
                    firmaAdi = firmaAdi.ToUpper();
                    //int SonFirmaID = Db.Firmas.Max(item => item.FirmaID);
                    IEnumerable<Arayanlar> arayanGecmisAramalari = Db.Arayanlars.GetirArayanGecmisAramalar(firmaAdi).ToList();
                    foreach (Arayanlar arayanim in arayanGecmisAramalari)
                    {

                        arayanim.arayanFirmaKayitDurum = true;
                        arayanim.arayanKayitliMusterimi = true;
                        arayanim.Firma = firma;
                    }

                 

                }
                Db.Firmas.Add(firma);
            }
            Db.SaveChanges();
            TempData[SUCESS] = "Kaydedildi";
            return RedirectToAction("Tumu");
        }
        private  string Temizle(string Kelime)
        {
            if (!String.IsNullOrEmpty(Kelime))
            {
                while (Kelime.Contains("  "))
                    Kelime = Kelime.Replace("  ", " ");
                Kelime = Kelime.Trim();
            }
            return Kelime;
        }
        #endregion firmakaydet
        #region firma ekle
        public ActionResult ArayaniFirmayaKaydet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Firma firma = new Firma();
            Arayanlar arayan = Db.Arayanlars.SingleOrDefault(x => x.arayanID == id);

            firma.FirmaAdi = arayan.arayanFirmaAdi;
            firma.FirmaAdres = arayan.arayanAdres;
            firma.YetkiliAdi = arayan.arayanAdi;
            firma.YetkiliSoyAdi = arayan.arayanSoyadi;
            firma.YetkiliCepTelefon = arayan.arayanCepTelNo;
            firma.YetkiliTelefon = arayan.arayanTelefonNo;
            firma.Email = arayan.arayanMailAdresi;
            firma.RefKonumID = arayan.arayanRefKonumID;
            firma.firmaSektorID = arayan.arayanSektorID;
            firma.firmaDomainKategoriID = arayan.arayanDomainKategoriID;
            firma.firmaKayitTarih = DateTime.Now;
            firma.firmaAraciGrupID = arayan.arayanGrupID;
            firma.firmaRefArayanID = id;
            ViewBag.Firmalar = Db.Firmas.GetirFirmalar("tumu");
            ViewBag.Konumlar = Db.Konums.GetirKonumlar();
            return View("FirmaKaydet",firma);
        }
        #endregion firma ekle
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