using Microsoft.AspNet.Identity;
using Portal.Models;
using Portal.Models.ArayanlarModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    //public class ArayanListeModel
    //{
    //    public int Id { get; set; }
    //    public string AdSoyad { get; set; }
    //    public string Firma { get; set; }
    //    public string Tel { get; set; }
    //    public string CepTel { get; set; }
    //    public string Not { get; set; }
    //    public string MailDurumu { get; set; }
    //    public int? Ticket { get; set; }
    //}
    public class ArayanlarController : BaseController
    {
        public ArayanlarController()
        {
            ViewBag.guncelMenu = "Arayanlar";
        }
        // GET: Arayanlar
        public ActionResult ArayanListesi()
        {
            //var liste = Db.ArayanListesis.ToList();
            //var liste = (from ary in Db.Arayanlars
            //             join q in Db.islers on ary  equals q.Arayanlar into jobs
            //             from j in jobs.DefaultIfEmpty()
            //             join ms in Db.MailSablonus.DefaultIfEmpty() on ary.arayanMailSablonuId equals ms.MailSablonuID
            //             where j.islerRefArayanID!=null
            //             select new ArayanListeModel {Id=ary.arayanID,AdSoyad=ary.arayanAdi+" "+ary.arayanSoyadi,Firma=ary.Firma.FirmaAdi,
            //                 Tel =ary.arayanTelefonNo,CepTel=ary.arayanCepTelNo,Not=ary.arayanNot,MailDurumu=ms.MailSablonuAdi,Ticket=j.islerID}
            //           );
            return View();
        }
        public JsonResult ArayanListesiParametre(string basTarih,string bitisTarih,string firma,string telNo
            ,string note,string adSoyad,int sayfaNo)
        {
            int baslangic = (sayfaNo - 1) * PagerCount;
            JsonCevap jsn = new JsonCevap();
            var query = Db.ArayanListesis.Where(x => (!string.IsNullOrEmpty(firma) ? x.Firma.Contains(firma) : true) && (!string.IsNullOrEmpty(telNo) ? x.Tel.Contains(telNo) : true)
             && (!string.IsNullOrEmpty(note) ? x.Note.Contains(note) : true) && (!string.IsNullOrEmpty(adSoyad) ? x.AdSoyad.Contains(adSoyad) : true)
            );
            if(!string.IsNullOrEmpty(basTarih) && !string.IsNullOrEmpty(bitisTarih))
            {
                DateTime tBas = DateTime.Parse(basTarih);
                DateTime tBit = DateTime.Parse(bitisTarih).AddHours(23).AddMinutes(59);
                jsn.ToplamSayi = query.Where(x => x.Tarih >= tBas && x.Tarih <= tBit).Count();
                query = query.Where(x=> x.Tarih >= tBas && x.Tarih <= tBit).OrderByDescending(x=>x.Tarih).Skip(baslangic).Take(PagerCount);
                jsn.Data = query.ToList();
            }
            else
            {
                jsn.ToplamSayi = query.Count();
                query =query.OrderByDescending(x => x.Tarih).Skip(baslangic).Take(20);               
                jsn.Data = query.ToList();
            }     
            return Json(jsn, JsonRequestBehavior.AllowGet);
        }
        #region arayanekle
        public ActionResult ArayanEkle()
        {
            ArayanModel arayanlar = new ArayanModel();
          
            ViewBag.arayanGrupID = new SelectList(Db.ArayanGrups, "arayanGrupID", "arayanGrupAdi");
            ViewBag.arayanRefKonumID = new SelectList(Db.Konums.OrderBy(x=>x.Konum1), "KonumID", "Konum1");
            ViewBag.arayanDomainKategoriID = new SelectList(Db.DomainKategoris, "DomainKategoriID", "DomainKategoriAdi");
            ViewBag.arayanSektorID = new SelectList(Db.Sektorlers, "sektorID", "sektorAdi");
            ViewBag.mailSablonlari = new SelectList(Db.MailSablonus, "MailSablonuID", "MailSablonuAdi");
            //ViewBag.islerisiYapacakKisi = new SelectList(Db.AspNetUsers, "Id", "UserName");
            ViewBag.kullanicilar = Db.AspNetUsers.Where(x => x.LockoutEnabled == false).ToList();
            return View(arayanlar);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ArayanEkle(ArayanModel vmodel)
        {
            const int ARACI_VAR = 1;
            const int ARACI_YOK = 2;
            if (ModelState.IsValid)
            {
                Arayanlar arayan = new Arayanlar();
                arayan.arayanilkArama = vmodel.arayanKayitliRefFirmaID.HasValue ? false : true;
                //TODO: login giris tamamlaninca aktif edilecek
                arayan.arayanTelefonaBakanKisiID = User.Identity.GetUserId();
                arayan.arayanAdi = vmodel.arayanAdi;
                arayan.arayanSoyadi = vmodel.arayanSoyadi;
                arayan.arayanFirmaAdi = vmodel.arayanFirmaAdi;
                arayan.arayanFirmaSahibiOzelligi = vmodel.arayanFirmaSahibiOzelligi;
                arayan.arayanTelefonNo =Fonksiyonlar.TelefonDuzelt(vmodel.arayanTelefonNo);
                arayan.arayanCepTelNo = Fonksiyonlar.TelefonDuzelt( vmodel.arayanCepTelNo);
                arayan.arayanMailAdresi = vmodel.arayanMailAdresi;
                arayan.arayanWebAdresi = vmodel.arayanWebAdresi;
                arayan.arayanSehir = vmodel.arayanSehir;
                arayan.arayanilce = vmodel.arayanilce;
                arayan.arayanAdres = vmodel.arayanAdres;
                if (vmodel.arayanRefKonumID.HasValue)
                {
                    arayan.arayanRefKonumID = vmodel.arayanRefKonumID;
                    arayan.arayanFirmaKayitDurum = true;
                }
                else
                {
                    arayan.arayanRefKonumID = Db.Konums.SingleOrDefault(m => m.Konum1 == "Antalya").KonumID;
                }
                arayan.arayanKonu =Fonksiyonlar.KarakterDuzenle(vmodel.arayanKonu);
                arayan.arayanNot =Fonksiyonlar.KarakterDuzenle( vmodel.arayanNot);
                arayan.arayanBegendigiWebSiteleri = vmodel.arayanBegendigiWebSiteleri;
                if (vmodel.arayanKayitTarih.HasValue)
                {
                    arayan.arayanKayitTarih = new DateTime(vmodel.arayanKayitTarih.Value.Year, vmodel.arayanKayitTarih.Value.Month, 
                        vmodel.arayanKayitTarih.Value.Day, 0, 0, 0);
                    if (!string.IsNullOrEmpty(vmodel.SaatDakika))
                    {
                        string[] ary = vmodel.SaatDakika.Split(':');
                        arayan.arayanKayitTarih = new DateTime(vmodel.arayanKayitTarih.Value.Year, vmodel.arayanKayitTarih.Value.Month, vmodel.arayanKayitTarih.Value.Day,
                            Convert.ToInt32(ary[0]), Convert.ToInt32(ary[1]), 0);
                    }
                }
                else
                {
                    arayan.arayanKayitTarih = DateTime.Now;
                }

                arayan.arayanDomainKategoriID = vmodel.arayanDomainKategoriID;

                arayan.arayanSektorID = vmodel.arayanSektorID;
                arayan.arayanGrupID = vmodel.araciVarmi?ARACI_VAR:ARACI_YOK;
                arayan.arayanMailSablonuId = vmodel.arayanMailSablonuId;
                if (vmodel.arayanKayitliRefFirmaID.HasValue)
                {
                    arayan.arayanKayitliRefFirmaID = vmodel.arayanKayitliRefFirmaID;
                    arayan.arayanKayitliMusterimi = true;
                    arayan.arayanFirmaKayitDurum = true;

                }
                else
                {
                    arayan.arayanKayitliMusterimi = false;
                    arayan.arayanFirmaKayitDurum = false;

                }
                if (vmodel.isEkle)
                {
                    isler yeni = YeniIsEkle(vmodel);
                    yeni.Arayanlar = arayan;
                    Db.islers.Add(yeni);                
                }
                if (vmodel.TanitimMailiGonder)
                {
                    TanitimMailiGonder(vmodel.arayanMailAdresi, vmodel.MailBaslik);
                }
                Db.Arayanlars.Add(arayan);
                Db.SaveChanges();
                TempData[SUCESS] = "Kayıt eklendi";
                return RedirectToAction("ArayanEkle");

            }
            return RedirectToAction("ArayanEkle");
        }       
        private isler YeniIsEkle(ArayanModel vmodel)
        {
            isler isE = new isler();
            if (vmodel.arayanKayitliRefFirmaID.HasValue)
            {
                isE.islerRefFirmaID = vmodel.arayanKayitliRefFirmaID.Value;
            }
            else
            {
                int sonArayanID = Db.Arayanlars.Max(item => item.arayanID);
                sonArayanID++;

                isE.islerRefArayanID = sonArayanID;
            }

            //islerim.islerDosyaAdi = filename;
            isE.islerRefDomainID = vmodel.domainId;
            //isE.islerRefKategoriID = vmodel.islerRefKategoriID;
            isE.islerAdi = vmodel.isAdi;
            isE.islerAciklama = Fonksiyonlar.KarakterDuzenle(vmodel.isAciklama);
            //TODO: login ekrani yapilinca aktif edilecek
            isE.islerisiVerenKisi = vmodel.kontrolEdecekKisi ?? User.Identity.GetUserId();

            //if (!string.IsNullOrEmpty(islerim.islerTarih.ToString()))
            //{
            //    islerim.islerTarih = yeni.isler.islerTarih;
            //}
            //else
            //{
            //    islerim.islerTarih = DateTime.Now;
            //}
            isE.islerTarih = DateTime.Now;

            isE.islerisinTamamlanmaDurumu = false;
            isE.islerinisinOnayDurumu = false;
            isE.islerOncelikSiraID = (int)IslerOncelikSira.Ikinci;//yeni.isler.islerOncelikSiraID;
            isE.islerBitisTarihiVarmi = vmodel.BitirmeTarihiVarmi;
            if (vmodel.BitirmeTarihiVarmi)
            {
                isE.islerBitisTarihi = vmodel.bitirmeTarihi;
            }
            isE.islerGelisYontemi = vmodel.gelisYonetemi;
            //Db.islers.Add(isE);

            //int sonDetayID = Db.islers.Max(item => item.islerID);
            //sonDetayID++;
            isE.islerIsinDurumu = (int)IsinDurumu.Yapilacak;
            isE.islerSiraNo = Db.islers.Where(x => x.islerRefDomainID == vmodel.domainId).Max(x=>x.islerSiraNo)+1;
            IsiYapacakKisi kisi = new IsiYapacakKisi();
            kisi.RefIsiYapacakKisiUserID = vmodel.islerisiYapacakKisi;
            kisi.isler = isE;
            Db.IsiYapacakKisis.Add(kisi);

           
            return isE;
            //string mailAdres = Fonksiyonlar.KullaniciMailAdresGetir(yeni.isler.islerisiYapacakKisi);
            //Fonksiyonlar.MailGonder(mailAdres, "is", sonDetayID);
        }
        private void TanitimMailiGonder(string mailAdres,string baslik)
        {
            MailKontrol yeniKontrol = new MailKontrol();
            yeniKontrol.MailBaslik = baslik;
            yeniKontrol.MailAdresi = mailAdres;
            yeniKontrol.MailOkundumu = false;
            yeniKontrol.MailGondermeTarihi = DateTime.Now;
            Db.MailKontrols.Add(yeniKontrol);
            Db.SaveChanges();
            string mesaj= Db.MailSablonus.FirstOrDefault(a => a.MailSablonuAdi == "Tanitim").MailSablonu1;
            mesaj = mesaj + "<img src=\"http://is.karayeltasarim.com/mail/oku/" + yeniKontrol.MailKontrolID + "\" width=\"1\" height=\"1\" />";
            Fonksiyonlar.MailGonder(mailAdres, baslik, mesaj);
           
        }
        #endregion arayanekle
        #region arayan duzenle
        public ActionResult ArayanDuzenle(int id)
        {
            ViewBag.arayanGrupID = Db.ArayanGrups;
            ViewBag.arayanRefKonumID = Db.Konums;
            ViewBag.arayanDomainKategoriID = Db.DomainKategoris;
            ViewBag.arayanSektorID = Db.Sektorlers;
            ViewBag.mailSablonlari = Db.MailSablonus;
            if (Request.UrlReferrer != null)
            {
                var ary = Request.UrlReferrer.ToString().Split('/');
                if (ary.Length >= 4)
                {
                    ViewBag.oncekiSayfa = Request.UrlReferrer.ToString().Split('/')[4];
                }
            }
            var arayan = Db.Arayanlars.SingleOrDefault(x => x.arayanID == id);
            return View(arayan);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ArayanDuzenle(Arayanlar vmodel,int id)
        {           
            var entity = Db.Arayanlars.SingleOrDefault(x => x.arayanID == id);
            entity.arayanilkArama = vmodel.arayanKayitliRefFirmaID.HasValue ? false : true;
            entity.arayanAdi = vmodel.arayanAdi;
            entity.arayanSoyadi = vmodel.arayanSoyadi;
            entity.arayanFirmaAdi = vmodel.arayanFirmaAdi;
            entity.arayanFirmaSahibiOzelligi = vmodel.arayanFirmaSahibiOzelligi;
            entity.arayanTelefonNo = Fonksiyonlar.TelefonDuzelt(vmodel.arayanTelefonNo);
            entity.arayanCepTelNo = Fonksiyonlar.TelefonDuzelt(vmodel.arayanCepTelNo);
            entity.arayanMailAdresi = vmodel.arayanMailAdresi;
            entity.arayanWebAdresi = vmodel.arayanWebAdresi;
            entity.arayanSehir = vmodel.arayanSehir;
            entity.arayanilce = vmodel.arayanilce;
            entity.arayanAdres = vmodel.arayanAdres;
            if (vmodel.arayanRefKonumID.HasValue)
            {
                entity.arayanRefKonumID = vmodel.arayanRefKonumID;
                entity.arayanFirmaKayitDurum = true;
            }
            else
            {
                entity.arayanRefKonumID = Db.Konums.SingleOrDefault(m => m.Konum1 == "Antalya").KonumID;
            }
            entity.arayanKonu = Fonksiyonlar.KarakterDuzenle(vmodel.arayanKonu);
            entity.arayanNot = Fonksiyonlar.KarakterDuzenle(vmodel.arayanNot);
            entity.arayanBegendigiWebSiteleri = vmodel.arayanBegendigiWebSiteleri;
            if (vmodel.arayanKayitTarih.HasValue)
            {
                entity.arayanKayitTarih = new DateTime(vmodel.arayanKayitTarih.Value.Year, vmodel.arayanKayitTarih.Value.Month,
                    vmodel.arayanKayitTarih.Value.Day, 0, 0, 0);
                if (!string.IsNullOrEmpty(Request["SaatDakika"]))
                {
                    string[] ary = Request["SaatDakika"].Split(':');
                    entity.arayanKayitTarih = new DateTime(vmodel.arayanKayitTarih.Value.Year, vmodel.arayanKayitTarih.Value.Month, vmodel.arayanKayitTarih.Value.Day,
                        Convert.ToInt32(ary[0]), Convert.ToInt32(ary[1]), 0);
                }
            }
            else
            {
                entity.arayanKayitTarih = DateTime.Now;
            }
            entity.arayanDomainKategoriID = vmodel.arayanDomainKategoriID;
            entity.arayanSektorID = vmodel.arayanSektorID;
            //entity.arayanGrupID = vmodel.araciVarmi ? ARACI_VAR : ARACI_YOK;
            entity.arayanMailSablonuId = vmodel.arayanMailSablonuId;
            if (vmodel.arayanKayitliRefFirmaID.HasValue)
            {
                entity.arayanKayitliRefFirmaID = vmodel.arayanKayitliRefFirmaID;
                entity.arayanKayitliMusterimi = true;
                entity.arayanFirmaKayitDurum = true;
            }
            else
            {
                entity.arayanKayitliMusterimi = false;
                entity.arayanFirmaKayitDurum = false;
            }
            Db.SaveChanges();
            TempData[SUCESS] = "Kaydedildi";
            if (Request["oncekiSayfa"] != "")
            {
                string rd = Request["oncekiSayfa"].Trim();
                return RedirectToAction(rd);
            }
            else
            {
                return View("ArayanListesi");
            }
            
        }
        #endregion arayan duzenle
        public ActionResult GonderilenMailler()
        {
            return View(Db.MailKontrols.OrderByDescending(x=>x.MailKontrolID));
        }
        //public ActionResult MailOkundu(int id)
        //{
        //    MailKontrol yeniKontrol = Db.MailKontrols.FirstOrDefault(a => a.MailKontrolID == id);
        //    yeniKontrol.MailOkundumu = true;
        //    yeniKontrol.MailOkunmaTarihi = DateTime.Now;
        //    Db.SaveChanges();

        //    return View();
        //}
    }
}