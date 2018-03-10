using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Portal.Models;
using AutoMapper;
using System.Net;
using System.Text.RegularExpressions;

namespace Portal.Controllers
{
    public class FirmalarController : BaseController
    {
        public FirmalarController()
        {
            ViewBag.guncelMenu = "Firmalar";
        }


        
        [HttpPost]
        public ActionResult BorcNotEkle()
        {

            int firmaidsi = Convert.ToInt32(Request.Form["pk"]);
            string not = Request.Form["value"];

            JsonCevap jsn = new JsonCevap();

            Firma firmanotu = Db.Firmas.FirstOrDefault(a => a.FirmaID == firmaidsi);

            firmanotu.FirmaBorcNotu = not;

            Db.SaveChanges();

            jsn.Basarilimi = true;

            return Json(jsn, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Yonetim")]
        public JsonResult BorcluFirmalardaGoster(int id)
        {
            JsonCevap jsn = new JsonCevap();
            Firma firmam = Db.Firmas.FirstOrDefault(x => x.FirmaID == id);

            if(firmam.FirmaBorcluListesindeGizle)
            {
                firmam.FirmaBorcluListesindeGizle = false;
                jsn.Data = false;
            }
            else
            {
                firmam.FirmaBorcluListesindeGizle = true;
                jsn.Data = true;
            }
            Db.SaveChanges();
            jsn.Basarilimi = true;
            return Json(jsn, JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "Satis,Muhasebe")]
        public ActionResult BorcluFirmalar()
        {

            var firmaIDleri = Db.Satis.GroupBy(a => a.musteriFirmaID).Where(x => x.Sum(a => a.musteridenAlinanOdeme) != x.Sum(a => a.musteriSatis) ).Select(o => new { id = o.Key });

            List<int?> idler = firmaIDleri.Select(a => a.id).ToList();

            IEnumerable<Firma> result = Db.Firmas.TumFirmalar().Where( a => idler.Contains( a.FirmaID )).OrderBy(a => a.RefKonumID).ThenByDescending(x => x.Satis.Sum(c => c.musteriSatis) - x.Satis.Sum(c => c.musteridenAlinanOdeme));

            return View(result);
        }


        #region firma tumu
        [Authorize(Roles = "Satis,Muhasebe,MusteriTemsilcisi")]
        public ActionResult List(string durum,int? p,string q)
        {
            //if (!User.IsInRole("Muhasebe") && durum != "musteri")
            //{

            //    TempData[ERROR] = "Bu bölüme giriş yetkiniz bulunmuyor.";
            //    return RedirectToAction("Index", "Home");
            //}

            ViewBag.SayfaAdi = "Firmalar";
            ViewBag.Durum = durum;


            int SayfaNo = p ?? 1;
            int domainBaslangic = (SayfaNo - 1) * PagerCount;
            List<Firma> result = new List<Firma>();      
            if(durum== "BorcluFirmalar")
            {
                IEnumerable<Firma> list2 = Db.Firmas.TumFirmalar().Where(x => x.Musteri == true)
                     .Where(a => (a.CariHarekets.Sum(c => c.ChSatisFiyati) - a.CariHarekets.Sum(c => c.ChAlinanOdeme)) > 0)
                     .Where(x => !string.IsNullOrEmpty(q) ? x.FirmaAdi.Contains(q) : true);
                
                if ((!User.IsInRole("Muhasebe") && User.IsInRole("Satis"))|| User.IsInRole("MusteriTemsilcisi")) 
                {
                    list2 = list2.Where(x => (x.Personel != true && x.Kasa != true));
                }
                var qTotal2 = list2;
                result=list2.OrderByDescending(x => x.CariHarekets.Sum(c => c.ChSatisFiyati) - x.CariHarekets.Sum(c => c.ChAlinanOdeme))
                    .Skip(domainBaslangic).Take(PagerCount).ToList();

                int totalCount = qTotal2.Count();
                PaginatedList pager = new PaginatedList((p ?? 1), PagerCount, totalCount);

                ViewData["queryData"] = q;
                ViewBag.Sayfalama = pager;
            }
            else if(durum == "BakimAntlasmasi")
            {
                var list3 = (from f in Db.Firmas.TumFirmalar()
                             join d in Db.Domains on f.FirmaID equals d.RefDomainFirmaID
                             orderby f.FirmaAdi descending
                             where d.GuncellemeSozlesmesiVarmi==true
                             select f
                           );
                if (!User.IsInRole("Muhasebe") && User.IsInRole("Satis"))
                {
                    list3 = list3.Where(x => (x.Personel != true && x.Kasa != true));
                }

                var qT = list3;
                int totalCount = qT.Count();
                PaginatedList pager = new PaginatedList((p ?? 1), PagerCount, totalCount);

                ViewData["queryData"] = q;
                ViewBag.Sayfalama = pager;
                result = list3.Where(x => !string.IsNullOrEmpty(q) ? x.FirmaAdi.Contains(q) : true)
                    .Skip(domainBaslangic).Take(PagerCount).ToList();

            }
            else
            {
             IEnumerable<Firma> list =   Db.Firmas.GetirFirmalar(durum.ToLower()).Where(x => !string.IsNullOrEmpty(q) ? x.FirmaAdi.Contains(q) : true ).OrderByDescending(w => w.FirmaID);
                if (User.IsInRole("MusteriTemsilcisi"))
                {
                    list = list.Where(x => (x.Personel != true && x.Kasa != true));
                }
                int totalCount = list.Count();

                list = list.Skip(domainBaslangic).Take(PagerCount);
                
                PaginatedList pager = new PaginatedList((p ?? 1), PagerCount, totalCount);

                ViewData["queryData"] = q;
                ViewBag.Sayfalama = pager;
                result = list.ToList();
            }
           
            //ViewBag.Domainler = Db.Domains.GetirDomainler(id);

            return View(result);
        }
        [Authorize(Roles = "Muhasebe")]
        public ActionResult FirmaSil(int id)
        {
            var firma = Db.Firmas.SingleOrDefault(x => x.FirmaID == id);
            Db.Firmas.Remove(firma);
            TempData[SUCESS] = "Kayıt Silindi";
            Db.SaveChanges();
            return RedirectToAction("List",new { durum="Tumu"});
        }
        #endregion firma tuu
        #region firmakaydet
        [Authorize(Roles = "Satis,Muhasebe")]
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
        [Authorize(Roles = "Satis,Muhasebe")]
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
                        arayanim.Firma = firma;
                    }
                }
                Db.Firmas.Add(firma);
            }
            Db.SaveChanges();
            TempData[SUCESS] = "Kaydedildi";
            return RedirectToAction("List", new { durum = "Tumu" });
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

            FirmaKisi yeniKisi = new FirmaKisi()
            {
                Ad = arayan.arayanAdi,
                Soyad = arayan.arayanSoyadi,
                Tel = arayan.arayanCepTelNo,
                Tel2 = arayan.arayanTelefonNo,
                Email = arayan.arayanMailAdresi,
                Departman = "Yönetici"
            };

            firma.FirmaKisis.Add(yeniKisi);

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

        public string TelefonDuzelt(string TelefonNo)
        {
            if(String.IsNullOrEmpty(TelefonNo))
            {
                return TelefonNo;
            }

            Regex rgx = new Regex("[^0-9]");
            TelefonNo = rgx.Replace(TelefonNo, "");

            if(TelefonNo.Length == 7)
            {
                TelefonNo = "0242"+TelefonNo;
            }

            if(TelefonNo.Length>11)
            {
                TelefonNo = TelefonNo.Substring(TelefonNo.Length - 11, 11);
            }

            if (TelefonNo.Substring(0, 1) != "0")
            {
                TelefonNo = "0" + TelefonNo;
            }

            return TelefonNo;
        }

        [HttpPost]
        public ActionResult FirmaKisiEkle(FirmaKisi model)
        {

            if(model.Id == 0)
            {
                FirmaKisi entity = new FirmaKisi();
                entity.Ad = model.Ad;
                entity.Soyad = model.Soyad;
                entity.Departman = model.Departman;
                entity.Email = model.Email;
                entity.FirmaId = model.FirmaId;
                entity.Tel = TelefonDuzelt(model.Tel);
                entity.Tel2 = TelefonDuzelt(model.Tel2);
                Db.FirmaKisis.Add(entity);
                Db.SaveChanges();
                TempData[SUCESS] = "Firma Kisi Kaydedildi";
            }
            else if(model.Id > 0)
            {
                FirmaKisi entity = Db.FirmaKisis.FirstOrDefault(a=>a.Id == model.Id);
                entity.Ad = model.Ad;
                entity.Soyad = model.Soyad;
                entity.Departman = model.Departman;
                entity.Email = model.Email;
                entity.FirmaId = model.FirmaId;
                entity.Tel = TelefonDuzelt(model.Tel);
                entity.Tel2 = TelefonDuzelt(model.Tel2);
                Db.SaveChanges();
                TempData[SUCESS] = "Firma Kisi Güncellendi";
            }

            return RedirectToAction("Detay", "CariHareket", new { id = model.FirmaId });
        }

        public ActionResult FirmaKisiSil(int id)
        {
            if(Db.Arayanlars.FirstOrDefault(a=>a.RefFirmaKisiId == id) != null)
            {
                return RedirectToAction("AramalariAktaripSil", "Firmalar", new { id = id });
            }

            FirmaKisi entity = Db.FirmaKisis.FirstOrDefault(a => a.Id == id);
            Db.FirmaKisis.Remove(entity);
            Db.SaveChanges();
            TempData[SUCESS] = "Firma Kisi Silindi";
            return RedirectToAction("Detay", "CariHareket", new { id = entity.FirmaId });
        }

        public ActionResult AramalariAktaripSil(int id)
        {
            FirmaKisi kisi = Db.FirmaKisis.FirstOrDefault(a => a.Id == id);

            ViewBag.Kisi = kisi;

            IEnumerable<FirmaKisi> firmaKisileri = Db.FirmaKisis.Where(a => a.FirmaId == kisi.FirmaId);

            return View(firmaKisileri);
        }


        
        //public ActionResult FirmaKisiDuzenle(int id)
        //{
        //    FirmaKisi kisi = Db.FirmaKisis.FirstOrDefault(a => a.Id == id);

        //    return View(kisi);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult FirmaKisiDuzenle(FirmaKisi kisi)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Db.Entry(kisi).State = EntityState.Modified;
        //        Db.SaveChanges();
        //        return RedirectToAction("Detay", "CariHareket", new { id = kisi.FirmaId });
        //    }
        //    return View(kisi);
        //}


        [HttpPost]
        public ActionResult AramalariAktaripSil(int id, int aktarilacakkisi)
        {
            FirmaKisi eskiKisi = Db.FirmaKisis.FirstOrDefault(a => a.Id == id);
            FirmaKisi yeniKisi = Db.FirmaKisis.FirstOrDefault(a => a.Id == aktarilacakkisi);

            IEnumerable<Arayanlar> kayitlar = Db.Arayanlars.Where(a => a.RefFirmaKisiId == id);

            foreach(Arayanlar kayit in kayitlar)
            {
                kayit.RefFirmaKisiId = aktarilacakkisi;
                kayit.arayanKonu = eskiKisi.Ad + " " + eskiKisi.Soyad + " --> " + yeniKisi.Ad + " " + yeniKisi.Soyad + " : " + kayit.arayanKonu; 
            }

            Db.SaveChanges();

            TempData["Success"] = "Tüm arama kayıtları " + eskiKisi.Ad + " " + eskiKisi.Soyad + "'dan " + yeniKisi.Ad + " " + yeniKisi.Soyad + " a aktarılmıştır." + eskiKisi.Ad + " " + eskiKisi.Soyad + "'i silebilirsiniz.";

            return RedirectToAction("Detay", "CariHareket", new { id = eskiKisi.FirmaId });
        }
        #endregion
    }
}