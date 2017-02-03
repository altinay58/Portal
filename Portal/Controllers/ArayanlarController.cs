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
            ,string note,string adSoyad)
        {
            DateTime tBas = DateTime.Parse(basTarih);
            DateTime tBit = DateTime.Parse(bitisTarih).AddHours(23).AddMinutes(59);
            var query = Db.ArayanListesis.Where(x =>x.Tarih >= tBas && x.Tarih <= tBit
            && (!string.IsNullOrEmpty(firma)?x.Firma.Contains(firma):true) && (!string.IsNullOrEmpty(telNo) ? x.Tel.Contains(telNo) : true)
             && (!string.IsNullOrEmpty(note) ? x.Note.Contains(note) : true) &&  (!string.IsNullOrEmpty(adSoyad) ? x.AdSoyad.Contains(adSoyad) : true)
            ).OrderByDescending(x => x.Tarih);         
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ArayanEkle()
        {
            ArayanModel arayanlar = new ArayanModel();
          
            ViewBag.arayanGrupID = new SelectList(Db.ArayanGrups, "arayanGrupID", "arayanGrupAdi");
            ViewBag.arayanRefKonumID = new SelectList(Db.Konums.OrderBy(x=>x.Konum1), "KonumID", "Konum1");
            ViewBag.arayanDomainKategoriID = new SelectList(Db.DomainKategoris, "DomainKategoriID", "DomainKategoriAdi");
            ViewBag.arayanSektorID = new SelectList(Db.Sektorlers, "sektorID", "sektorAdi");
            ViewBag.mailSablonlari = new SelectList(Db.MailSablonus, "MailSablonuID", "MailSablonuAdi");
            ViewBag.islerisiYapacakKisi = new SelectList(Db.AspNetUsers, "Id", "UserName");
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
                arayan.arayanTelefonNo = vmodel.arayanTelefonNo;
                arayan.arayanCepTelNo = vmodel.arayanCepTelNo;
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
                arayan.arayanKonu = vmodel.arayanKonu;
                arayan.arayanNot = vmodel.arayanNot;
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
    }
}