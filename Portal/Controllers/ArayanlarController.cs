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
    public class ArayanlarController : BaseController
    {
        // GET: Arayanlar
        public ActionResult ArayanEkle()
        {
            ArayanModel arayanlar = new ArayanModel();
          
            ViewBag.arayanGrupID = new SelectList(Db.ArayanGrups, "arayanGrupID", "arayanGrupAdi");
            ViewBag.arayanRefKonumID = new SelectList(Db.Konums, "KonumID", "Konum1");
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
                    vmodel.arayanKayitTarih = new DateTime(vmodel.arayanKayitTarih.Value.Year, vmodel.arayanKayitTarih.Value.Month, 
                        vmodel.arayanKayitTarih.Value.Day, 0, 0, 0);
                    if (!string.IsNullOrEmpty(vmodel.SaatDakika))
                    {
                        string[] ary = vmodel.SaatDakika.Split(':');
                        vmodel.arayanKayitTarih = new DateTime(vmodel.arayanKayitTarih.Value.Year, vmodel.arayanKayitTarih.Value.Month, vmodel.arayanKayitTarih.Value.Day,
                            Convert.ToInt32(ary[0]), Convert.ToInt32(ary[1]), 0);
                    }
                }
                else
                {
                    vmodel.arayanKayitTarih = DateTime.Now;
                }

                arayan.arayanDomainKategoriID = vmodel.arayanDomainKategoriID;

                arayan.arayanSektorID = vmodel.arayanSektorID;
                arayan.arayanGrupID = vmodel.araciVarmi?ARACI_VAR:ARACI_YOK;

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
                    YeniIsEkle(vmodel);
                }
                Db.Arayanlars.Add(arayan);
                Db.SaveChanges();
                TempData[SUCESS] = "Kayıt eklendi";
                return RedirectToAction("ArayanEkle");

            }
            return RedirectToAction("ArayanEkle");
        }

        private void YeniIsEkle(ArayanModel vmodel)
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
            isE.islerisiVerenKisi = User.Identity.GetUserId();

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



            Db.islers.Add(isE);



            int sonDetayID = Db.islers.Max(item => item.islerID);
            sonDetayID++;
            //string mailAdres = Fonksiyonlar.KullaniciMailAdresGetir(yeni.isler.islerisiYapacakKisi);
            //Fonksiyonlar.MailGonder(mailAdres, "is", sonDetayID);
        }
    }
}