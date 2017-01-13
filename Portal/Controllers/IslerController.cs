using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace Portal.Controllers
{
    public class IslerController : BaseController
    {
       
        // GET: Isler
        public ActionResult Index(bool kontrolBekleyenIsler, bool onaylananIsler, int? RefBolgeID, int? SayfaNo)
        {
            ViewBag.SayfaSiraNo = SayfaNo ?? 1;
            ViewBag.kontrolBekleyenIsler = kontrolBekleyenIsler;
            ViewBag.onaylananIsler = onaylananIsler;
            ViewBag.RefBolgeID = RefBolgeID;

            int GosterilecekEleman = (SayfaNo ?? 1) * 10;
            int baslanacakSira = GosterilecekEleman - 10;
            using (var db = new PortalEntities())
            {
                string kullaniciID = User.Identity.GetUserId();
                IEnumerable<isler> islerim = db.islers.GetirIsler(kontrolBekleyenIsler, onaylananIsler, kullaniciID, RefBolgeID).Skip(baslanacakSira).Take(10).ToList();
                ViewBag.ToplamEleman = islerim.ToList().Count;

                return View(islerim);
            }

        }
        public ActionResult IcerikFormu()
        {
           
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult IcerikFormu(IcerikFormu icerik, FormCollection frm)
        {
            // is atanacak ve kontrol edecek kullanici ayar tablosundaki kayitlara gore belirleniyor
            // Ayar table da IcerikFormuIsAtanacakKullanici ve IcerikFormuIsKontrolEdenKullanici kayitlari yok ise ekranda uyari gosteriyor
            var isAtanacakKullanici = Database.Db.Ayars.Where(x=>x.AyarAdi== "IcerikFormuIsAtanacakKullanici").SingleOrDefault();
            var isKontrolEdenKullanici = Database.Db.Ayars.Where(x=>x.AyarAdi== "IcerikFormuIsKontrolEdenKullanici").SingleOrDefault();
            if (ModelState.IsValid && (isAtanacakKullanici!=null && isKontrolEdenKullanici!=null) &&
                (!string.IsNullOrEmpty(isAtanacakKullanici.AyarDeger) && !(string.IsNullOrEmpty(isKontrolEdenKullanici.AyarDeger)) ))
            {
                var listStandardIsler = Database.Db.StandartProjeIsleris.ToList().OrderBy(x => x.StandartProjeIsleriSirasi);
                var dinamiStandartIsler = listStandardIsler.Where(x => x.StandartProjeIsleriIdAnahtarIsmi != null);
                var isHtml = string.Format("<p>Firma Adı:{0}</p>",icerik.FirmaAdi);
                isHtml += string.Format("<p>Domain Adı:{0}</p>", icerik.DomainAdi);
                isHtml += string.Format("<p>Telefon 1:{0}</p>", icerik.Telefon1);
                isHtml += string.Format("<p>Telefon 2:{0}</p>", icerik.Telefon2);
                isHtml += string.Format("<p>Email:{0}</p>", icerik.Email);
                isHtml += string.Format("<p>Adres:{0}</p>", icerik.Adres);
                isHtml += string.Format("<p>Konum Adı:{0}</p>", icerik.Konum);
                isHtml += string.Format("<p>Instagram Adı:{0}</p>", icerik.Instagram);
                isHtml += string.Format("<p>Google Plus Adı:{0}</p>", icerik.GooglePlus);
                isHtml += string.Format("<p>Twitter:{0}</p>", icerik.Twitter);
                foreach (var dinamikIs in dinamiStandartIsler)
                {
                    string anahtar = dinamikIs.StandartProjeIsleriIdAnahtarIsmi + "Alindi";
                    if (frm[anahtar].Contains("true"))
                    {
                        isHtml += string.Format("<p>{0} Alındı:{1}</p>", dinamikIs.StandartProjeIsleriIdAnahtarIsmi,frm[anahtar]);
                    }else
                    {
                        isHtml += string.Format("<p>{0} Alınmadı</p>", dinamikIs.StandartProjeIsleriIdAnahtarIsmi);
                    }
                }

                isler ilkIs = new isler();
                ilkIs.islerAciklama = string.Format("Domain:{0}-Firma:{1}", icerik.DomainAdi, icerik.FirmaAdi);
                ilkIs.islerAdi=icerik.DomainAdi+" bilgileri";
                ilkIs.islerRefDomainID = icerik.DomainId;
                ilkIs.islerRefFirmaID = icerik.FirmaId;
                ilkIs.islerisiYapacakKisi = isAtanacakKullanici.AyarDeger;
                //degişebilir
                ilkIs.islerisiVerenKisi = isKontrolEdenKullanici.AyarDeger;
                ilkIs.islerTarih = DateTime.Now;
                ilkIs.islerOncelikSiraID =(int) IslerOncelikSira.Ikinci;
                List<isler> isler = new List<Models.isler>();
                isler.Add(ilkIs);
                foreach(var standardIs in listStandardIsler)
                {
                    isler job = new isler();
                    job.islerAciklama = string.Format("{0}", standardIs.StandartProjeIsleriAciklama);
                    job.islerAdi =standardIs.StandartProjeIsleriIsAdi;
                    job.islerRefDomainID = icerik.DomainId;
                    job.islerRefFirmaID = icerik.FirmaId;
                    job.islerisiYapacakKisi = standardIs.RefStandartProjeIsleriYapacakKullaniciId;
                    job.islerOncelikSiraID = (int)IslerOncelikSira.Ikinci;
                    job.islerisiVerenKisi = standardIs.RefStandartProjeIsleriKontrolEdecekKullaniciId;
                    job.islerTarih = DateTime.Now;
                    isler.Add(job);
                }
                Database.Db.islers.AddRange(isler);
                Database.Db.SaveChanges();
                TempData["Success"] = "Kaydedildi";

                return RedirectToAction("Index",new { kontrolBekleyenIsler=false,  onaylananIsler=false });

            }
            else
            {
                TempData["Error"] = "Ayar tablosuna IcerikFormuIsAtanacakKullanici ve IcerikFormuIsKontrolEdenKullanici kayıtlarnı giriniz .";
                return View();
            }
          


         
        }
        [ValidateInput(false)]
        public ActionResult IcerikKaydet(string json)
        {
            IcerikFormu m = JsonConvert.DeserializeObject<IcerikFormu>(json);
            JsonCevap cevap = new JsonCevap();
            cevap.Basarilimi = true;

            return Json(cevap, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FirmalariGetir(string firmaAdi)
        {


            var firmalar = new List<object>();

            foreach (Firma firma in Database.Db.Firmas.Where(a => a.FirmaAdi.Contains(firmaAdi) ))
            {
                firmalar.Add(new
                {
                    value = firma.FirmaID, label = firma.FirmaAdi,Telefon1=firma.YetkiliTelefon,Telefon2=firma.YetkiliCepTelefon,
                    Email=firma.Email,Adres=firma.FirmaAdres
                });
            }

            /*
            var diziArayanlar = db.Arayanlars.Where(a => a.arayanKayitliMusterimi == false && a.arayanFirmaAdi.Contains(firmaAdi) || a.arayanCepTelNo.Contains(firmaAdi) || a.arayanTelefonNo.Contains(firmaAdi)).GroupBy(g => g.arayanFirmaAdi).Select(o => new { ArayanAdi = o.Key, Arayan = o.OrderBy(c => c.arayanFirmaAdi).ToList() }).ToList();

            foreach (var aramaYapan in diziArayanlar)
            {
                Arayanlar arayanFirma = db.Arayanlars.FirstOrDefault(a => a.arayanFirmaAdi == aramaYapan.ArayanAdi);
                if (arayanFirma != null)
                {
                    firmalar.Add(new { value = "kayitliDegil" + arayanFirma.arayanID, label = arayanFirma.arayanFirmaAdi + " - Kayıtlı Değil" });
                }
            }
            */
            return Json(firmalar, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DomainGetir(string domainAdi,int? firmaId)
        {
        

            var listDomain = (from d in Database.Db.Domains
                              where (firmaId.HasValue ? d.RefDomainFirmaID == firmaId.Value : true) && d.DomainAdi.Contains(domainAdi)
                              select new
                              {
                                  value = d.DomainID,label=d.DomainAdi,firmaId=d.Firma.FirmaID,firmaAdi=d.Firma.FirmaAdi,
                                  Telefon1 = d.Firma.YetkiliTelefon,
                                  Telefon2 = d.Firma.YetkiliCepTelefon,
                                  Email = d.Firma.Email,
                                  Adres = d.Firma.FirmaAdres
                              }).ToList();

          

            return Json(listDomain, JsonRequestBehavior.AllowGet);
        }
    }
   
}