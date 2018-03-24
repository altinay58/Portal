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
    //test rt ert ert ert ert ert
    //falan filan
    //asdasdasd
    //pğoğowekrjsf hf sdjfhhdsjfh dhsfjksdhf dh fdshfdsj fhdshfsd dsdfsdf
    public class ArayanlarController : BaseController
    {
        public ArayanlarController()
        {
            string a = "";
            ViewBag.guncelMenu = "Arayanlar";
            a = "b";
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
             && (!string.IsNullOrEmpty(note) ? x.Konu.Contains(note) : true) && (!string.IsNullOrEmpty(adSoyad) ? x.AdSoyad.Contains(adSoyad) : true)
            );
            if(!string.IsNullOrEmpty(basTarih) && !string.IsNullOrEmpty(bitisTarih))
            {
                DateTime tBas = DateTime.Parse(basTarih);
                DateTime tBit = DateTime.Parse(bitisTarih).AddHours(23).AddMinutes(59);
                jsn.ToplamSayi = query.Where(x => x.Tarih >= tBas && x.Tarih <= tBit).Count();
                query = query.Where(x=> x.Tarih >= tBas && x.Tarih <= tBit).OrderByDescending(x=>x.Id).Skip(baslangic).Take(PagerCount);
                jsn.Data = query.ToList();
            }
            else
            {
                jsn.ToplamSayi = query.Count();
                query =query.OrderByDescending(x => x.Id).Skip(baslangic).Take(20);               
                jsn.Data = query.ToList();
            }     
            return Json(jsn, JsonRequestBehavior.AllowGet);
        }
        #region arayanekle
        public ActionResult ArayanEkle()
        {
            ArayanModel arayanlar = new ArayanModel();


            ViewBag.Departmanlar = new SelectList(Db.Etikets.Where(a=>a.Kategori == "EtiketArayanDepartmanId").OrderBy(x=>x.Sira), "Value", "Text");
            ViewBag.AramaYontemiId = new SelectList(Db.Etikets.Where(a=>a.Kategori == "AramaYontemiId").OrderBy(x=>x.Sira), "Value", "Text");
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
                string userid = User.Identity.GetUserId();

                if (vmodel.arayanKayitliRefFirmaID == 0) // firma kayıtlı mı diye kontrol ediyoruz.
                {
                    if(String.IsNullOrEmpty(vmodel.arayanFirmaAdi))
                    {
                        TempData[SUCESS] = "Firma Adını Girin";
                        return RedirectToAction("ArayanEkle");
                    }

                    Firma yeniFirma = new Firma()
                    {
                        FirmaAdi = vmodel.arayanFirmaAdi,
                        FirmaWebSitesi = vmodel.arayanWebAdresi,
                        firmaSehir =  vmodel.arayanSehir,
                        FirmaAdres = vmodel.arayanAdres,
                        firmailce = vmodel.arayanilce,
                        RefKonumID = vmodel.arayanRefKonumID,
                        Araci = null,
                        Musteri = true,
                        Personel = false,
                        Kasa = false,
                        firmaKayitYapanKisiID = userid,
                        firmaKayitTarih = DateTime.Now,
                        FirmaSilindi = false
                    };

                    Db.Firmas.Add(yeniFirma);
                    Db.SaveChanges();

                    vmodel.arayanKayitliRefFirmaID = yeniFirma.FirmaID;
                }
                else
                {
                    Firma firmam = Db.Firmas.FirstOrDefault(a => a.FirmaID == vmodel.arayanKayitliRefFirmaID);
                    vmodel.arayanFirmaAdi = firmam.FirmaAdi;
                    vmodel.arayanWebAdresi = firmam.FirmaWebSitesi;
                    vmodel.arayanSehir = firmam.firmaSehir;
                    vmodel.arayanilce = firmam.firmailce;
                    vmodel.arayanAdres = firmam.FirmaAdres;
                    vmodel.arayanRefKonumID = firmam.RefKonumID;
                    vmodel.arayanKayitliMusterimi = true;
                }

                if(vmodel.kisiid == null)
                {
                    FirmaKisi yeniKisi = new FirmaKisi()
                    {
                        Ad = vmodel.arayanAdi,
                        Soyad = vmodel.arayanSoyadi,
                        Tel = vmodel.arayanTelefonNo,
                        Tel2 = vmodel.arayanCepTelNo,
                        Departman = vmodel.arayanFirmaSahibiOzelligi,
                        Email = vmodel.arayanMailAdresi,
                        FirmaId = vmodel.arayanKayitliRefFirmaID
                    };

                    Db.FirmaKisis.Add(yeniKisi);
                    Db.SaveChanges();

                    vmodel.kisiid = yeniKisi.Id;
                }
                else
                {
                    FirmaKisi kisim = Db.FirmaKisis.FirstOrDefault(a => a.Id == vmodel.kisiid);

                    vmodel.arayanAdi = kisim.Ad;
                    vmodel.arayanSoyadi = kisim.Soyad;
                    vmodel.arayanCepTelNo = kisim.Tel;
                    vmodel.arayanTelefonNo = kisim.Tel2;
                    vmodel.arayanFirmaSahibiOzelligi = kisim.Departman;
                    vmodel.arayanMailAdresi = kisim.Email;
                    vmodel.arayanKayitliRefFirmaID = kisim.FirmaId;


                }

                Arayanlar arayan = new Arayanlar();

                vmodel.arayanFirmaAdi = vmodel.arayanFirmaAdi.Replace("  Kayıtlı Değil", "").Trim();
                // TO DO : login giris tamamlaninca aktif edilecek
                arayan.arayanTelefonaBakanKisiID = userid;
                arayan.arayanAdi = vmodel.arayanAdi;
                arayan.arayanSoyadi = vmodel.arayanSoyadi;
                arayan.arayanFirmaAdi = vmodel.arayanFirmaAdi;
                arayan.arayanFirmaSahibiOzelligi = vmodel.arayanFirmaSahibiOzelligi;
                arayan.arayanTelefonNo = Fonksiyonlar.TelefonDuzelt(vmodel.arayanTelefonNo);
                arayan.arayanCepTelNo = Fonksiyonlar.TelefonDuzelt(vmodel.arayanCepTelNo);
                arayan.arayanMailAdresi = vmodel.arayanMailAdresi;
                arayan.arayanWebAdresi = vmodel.arayanWebAdresi;
                arayan.arayanSehir = vmodel.arayanSehir;
                arayan.arayanilce = vmodel.arayanilce;
                arayan.arayanAdres = vmodel.arayanAdres;
                arayan.AramaYontemiId = vmodel.AramaYontemiId;

                if (vmodel.arayanRefKonumID.HasValue)
                {
                    arayan.arayanRefKonumID = vmodel.arayanRefKonumID;
                }
                else
                {
                    arayan.arayanRefKonumID = Db.Konums.SingleOrDefault(m => m.Konum1 == "Antalya").KonumID;
                }

                arayan.arayanKonu = Fonksiyonlar.KarakterDuzenle(vmodel.arayanKonu);
                arayan.arayanNot = Fonksiyonlar.KarakterDuzenle(vmodel.arayanNot);
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
                arayan.arayanGrupID = vmodel.araciVarmi ? ARACI_VAR : ARACI_YOK;
                arayan.arayanMailSablonuId = vmodel.arayanMailSablonuId;
                arayan.RefFirmaID = vmodel.arayanKayitliRefFirmaID;
                arayan.RefFirmaKisiId = vmodel.kisiid ?? 0;

                if (vmodel.isEkle)
                {
                    isler yeni = YeniIsEkle(vmodel);
                    yeni.Arayanlar = arayan;
                    Db.islers.Add(yeni);
                }
                if (vmodel.TanitimMailiGonder)
                {
                    TanitimMailiGonder(vmodel.arayanMailAdresi, vmodel.MailBaslik);
                    if (arayan.arayanMailSablonuId == 4)
                    {
                        IsBasvurusuMailiGonder(arayan.arayanMailAdresi, arayan.arayanAdi + " " + arayan.arayanSoyadi);
                    }
                }
                Db.Arayanlars.Add(arayan);
                Db.SaveChanges();
                TempData[SUCESS] = "Kayıt eklendi";
                return RedirectToAction("ArayanEkle");

            }
            return RedirectToAction("ArayanEkle");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[ValidateInput(false)]
        //public ActionResult ArayanEkle(ArayanModel vmodel)
        //{
        //    const int ARACI_VAR = 1;
        //    const int ARACI_YOK = 2;
        //    if (ModelState.IsValid)
        //    {
        //        Arayanlar arayan = new Arayanlar();
        //        vmodel.arayanFirmaAdi = vmodel.arayanFirmaAdi.Replace("  Kayıtlı Değil", "").Trim();
        //        arayan.arayanilkArama = vmodel.arayanKayitliRefFirmaID.HasValue ? false : true;
        //        //TODO: login giris tamamlaninca aktif edilecek
        //        arayan.arayanTelefonaBakanKisiID = User.Identity.GetUserId();
        //        arayan.arayanAdi = vmodel.arayanAdi;
        //        arayan.arayanSoyadi = vmodel.arayanSoyadi;
        //        arayan.arayanFirmaAdi = vmodel.arayanFirmaAdi;
        //        arayan.arayanFirmaSahibiOzelligi = vmodel.arayanFirmaSahibiOzelligi;
        //        arayan.arayanTelefonNo =Fonksiyonlar.TelefonDuzelt(vmodel.arayanTelefonNo);
        //        arayan.arayanCepTelNo = Fonksiyonlar.TelefonDuzelt( vmodel.arayanCepTelNo);
        //        arayan.arayanMailAdresi = vmodel.arayanMailAdresi;
        //        arayan.arayanWebAdresi = vmodel.arayanWebAdresi;
        //        arayan.arayanSehir = vmodel.arayanSehir;
        //        arayan.arayanilce = vmodel.arayanilce;
        //        arayan.arayanAdres = vmodel.arayanAdres;
        //        arayan.AramaYontemiId = vmodel.AramaYontemiId;
        //        if (vmodel.arayanRefKonumID.HasValue)
        //        {
        //            arayan.arayanRefKonumID = vmodel.arayanRefKonumID;
        //            arayan.arayanFirmaKayitDurum = true;
        //        }
        //        else
        //        {
        //            arayan.arayanRefKonumID = Db.Konums.SingleOrDefault(m => m.Konum1 == "Antalya").KonumID;
        //        }
        //        arayan.arayanKonu =Fonksiyonlar.KarakterDuzenle(vmodel.arayanKonu);
        //        arayan.arayanNot =Fonksiyonlar.KarakterDuzenle( vmodel.arayanNot);
        //        arayan.arayanBegendigiWebSiteleri = vmodel.arayanBegendigiWebSiteleri;
        //        if (vmodel.arayanKayitTarih.HasValue)
        //        {
        //            arayan.arayanKayitTarih = new DateTime(vmodel.arayanKayitTarih.Value.Year, vmodel.arayanKayitTarih.Value.Month, 
        //                vmodel.arayanKayitTarih.Value.Day, 0, 0, 0);
        //            if (!string.IsNullOrEmpty(vmodel.SaatDakika))
        //            {
        //                string[] ary = vmodel.SaatDakika.Split(':');
        //                arayan.arayanKayitTarih = new DateTime(vmodel.arayanKayitTarih.Value.Year, vmodel.arayanKayitTarih.Value.Month, vmodel.arayanKayitTarih.Value.Day,
        //                    Convert.ToInt32(ary[0]), Convert.ToInt32(ary[1]), 0);
        //            }
        //        }
        //        else
        //        {
        //            arayan.arayanKayitTarih = DateTime.Now;
        //        }

        //        arayan.arayanDomainKategoriID = vmodel.arayanDomainKategoriID;

        //        arayan.arayanSektorID = vmodel.arayanSektorID;
        //        arayan.arayanGrupID = vmodel.araciVarmi?ARACI_VAR:ARACI_YOK;
        //        arayan.arayanMailSablonuId = vmodel.arayanMailSablonuId;
        //        if (vmodel.arayanKayitliRefFirmaID.HasValue)
        //        {
        //            arayan.arayanKayitliRefFirmaID = vmodel.arayanKayitliRefFirmaID;
        //            arayan.arayanKayitliMusterimi = true;
        //            arayan.arayanFirmaKayitDurum = true;

        //        }
        //        else
        //        {
        //            arayan.arayanKayitliMusterimi = false;
        //            arayan.arayanFirmaKayitDurum = false;

        //        }
        //        if (vmodel.isEkle)
        //        {
        //            isler yeni = YeniIsEkle(vmodel);
        //            yeni.Arayanlar = arayan;
        //            Db.islers.Add(yeni);                
        //        }
        //        if (vmodel.TanitimMailiGonder)
        //        {
        //            TanitimMailiGonder(vmodel.arayanMailAdresi, vmodel.MailBaslik);
        //        }
        //        Db.Arayanlars.Add(arayan);
        //        Db.SaveChanges();
        //        TempData[SUCESS] = "Kayıt eklendi";
        //        return RedirectToAction("ArayanEkle");

        //    }
        //    return RedirectToAction("ArayanEkle");
        //}       
        private isler YeniIsEkle(ArayanModel vmodel)
        {
            isler isE = new isler();
                isE.islerRefFirmaID = vmodel.arayanKayitliRefFirmaID;

            //islerim.islerDosyaAdi = filename;
            isE.islerRefDomainID = vmodel.domainId;
            //isE.islerRefKategoriID = vmodel.islerRefKategoriID;
            isE.islerAdi = vmodel.arayanKonu;
            isE.islerAciklama = Fonksiyonlar.KarakterDuzenle(vmodel.arayanNot);
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

        private void IsBasvurusuMailiGonder(string mailAdres, string isimsoyisim)
        {
            MailKontrol yeniKontrol = new MailKontrol();
            yeniKontrol.MailBaslik = "İş Başvurusu";
            yeniKontrol.MailAdresi = mailAdres;
            yeniKontrol.MailOkundumu = false;
            yeniKontrol.MailGondermeTarihi = DateTime.Now;
            Db.MailKontrols.Add(yeniKontrol);
            Db.SaveChanges();
            string mesaj = Db.MailSablonus.FirstOrDefault(a => a.MailSablonuAdi == "İş Başvurusu").MailSablonu1;
            mesaj = "Sayın " + isimsoyisim + ", <br /><br /> " +  mesaj + "<img src=\"http://portal.karayeltasarim.com/mail/oku/" + yeniKontrol.MailKontrolID + "\" width=\"1\" height=\"1\" />";
            Fonksiyonlar.MailGonder(mailAdres, "İş Başvurunuz Hakkında", mesaj);

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
            mesaj = mesaj + "<img src=\"http://portal.karayeltasarim.com/mail/oku/" + yeniKontrol.MailKontrolID + "\" width=\"1\" height=\"1\" />";
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
              
                ViewBag.oncekiSayfa = Request.UrlReferrer.ToString();
            }
            var arayan = Db.Arayanlars.SingleOrDefault(x => x.arayanID == id);

            ViewBag.KisiListesi = Db.FirmaKisis.Where(a => a.FirmaId == arayan.RefFirmaID);


            return View(arayan);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ArayanDuzenle(Arayanlar vmodel,int id, int kisiid , int arayanKayitliRefFirmaID)
        {           
            var entity = Db.Arayanlars.SingleOrDefault(x => x.arayanID == id);
            //entity.arayanAdi = vmodel.arayanAdi;
            //entity.arayanSoyadi = vmodel.arayanSoyadi;
            //entity.arayanFirmaAdi = vmodel.arayanFirmaAdi;
            //entity.arayanFirmaSahibiOzelligi = vmodel.arayanFirmaSahibiOzelligi;
            //entity.arayanTelefonNo = Fonksiyonlar.TelefonDuzelt(vmodel.arayanTelefonNo);
            //entity.arayanCepTelNo = Fonksiyonlar.TelefonDuzelt(vmodel.arayanCepTelNo);
            //entity.arayanMailAdresi = vmodel.arayanMailAdresi;
            //entity.arayanWebAdresi = vmodel.arayanWebAdresi;
            //entity.arayanSehir = vmodel.arayanSehir;
            //entity.arayanilce = vmodel.arayanilce;
            //entity.arayanAdres = vmodel.arayanAdres;
            //if (vmodel.arayanRefKonumID.HasValue)
            //{
            //    entity.arayanRefKonumID = vmodel.arayanRefKonumID;
            //}
            //else
            //{
            //    entity.arayanRefKonumID = Db.Konums.SingleOrDefault(m => m.Konum1 == "Antalya").KonumID;
            //}
            entity.arayanKonu = Fonksiyonlar.KarakterDuzenle(vmodel.arayanKonu);
            entity.arayanNot = Fonksiyonlar.KarakterDuzenle(vmodel.arayanNot);
            //entity.arayanBegendigiWebSiteleri = vmodel.arayanBegendigiWebSiteleri;
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
            //entity.arayanDomainKategoriID = vmodel.arayanDomainKategoriID;
            //entity.arayanSektorID = vmodel.arayanSektorID;
            //entity.arayanGrupID = vmodel.araciVarmi ? ARACI_VAR : ARACI_YOK;
            //entity.arayanMailSablonuId = vmodel.arayanMailSablonuId;
            entity.RefFirmaID = arayanKayitliRefFirmaID;
            entity.RefFirmaKisiId = kisiid;
            Db.SaveChanges();
            TempData[SUCESS] = "Kaydedildi";
            if (Request["oncekiSayfa"] != "")
            {
                string rd = Request["oncekiSayfa"].Trim();
                rd = Server.HtmlDecode(rd);
                return Redirect(rd);
            }
            else
            {
                return View("ArayanListesi");
            }
            
        }
        #endregion arayan duzenle
        public ActionResult GonderilenMailler(int?page)
        {
            int baslangic = ((page ?? 1) - 1) * PagerCount;
            var data = Db.MailKontrols.OrderByDescending(x => x.MailKontrolID);
            var qTotal = data;

            int totalCount = qTotal.Count();
            PaginatedList pager = new PaginatedList((page ?? 1), PagerCount, totalCount);
            ViewBag.Sayfalama = pager;
            return View(data.Skip(baslangic).Take(PagerCount).ToList());
        }
        //public ActionResult MailOkundu(int id)
        //{
        //    MailKontrol yeniKontrol = Db.MailKontrols.FirstOrDefault(a => a.MailKontrolID == id);
        //    yeniKontrol.MailOkundumu = true;
        //    yeniKontrol.MailOkunmaTarihi = DateTime.Now;
        //    Db.SaveChanges();

        //    return View();
        //}
        public ActionResult ArayanGecmisAramaGetir(string firmaAdi, int? SayfaNo)
        {


            string firmaAdim = firmaAdi.Replace(" ", "");
            firmaAdim = firmaAdim.ToUpper();
            IEnumerable<Arayanlar> arayanlarim = null;
            arayanlarim = Db.Arayanlars.GetirArayanGecmisAramalar(firmaAdim);
            var qTotal2 = arayanlarim;
            int domainBaslangic = ((SayfaNo?? 1) - 1) * PagerCount;


            int totalCount = qTotal2.Count();
            PaginatedList pager = new PaginatedList((SayfaNo ?? 1), PagerCount, totalCount);
            ViewData["queryData"] = firmaAdi;
            ViewBag.Sayfalama = pager;
            return View(arayanlarim.Skip(domainBaslangic).Take(PagerCount).ToList());
        }
    }
}