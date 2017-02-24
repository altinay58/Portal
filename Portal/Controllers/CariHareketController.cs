using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;
namespace Portal.Controllers
{
    [Authorize(Roles = "Satis,Muhasebe")]
    public class CariHareketController : BaseController
    {
        public CariHareketController()
        {
            GuncelMenu = "Muhasebe";
        }
        #region carihareket
        [Authorize(Roles = "Muhasebe")]
        public ActionResult List(int?p)
        {
            int baslangic = ((p ?? 1) - 1) * PagerCount;
            var datas = Db.CariHarekets.GetirCariHareketler(PagerCount, baslangic);
            int totalCount = datas.Count();
            PaginatedList pager = new PaginatedList((p ?? 1), PagerCount, totalCount);

            ViewBag.Sayfalama = pager;
            return View(datas);
        }
        [Authorize(Roles = "Muhasebe,Satis")]
        public ActionResult Detay(int id)
        {
            if (User.IsInRole("Muhasebe"))
            {
                var data = Db.Firmas.SingleOrDefault(x=>x.FirmaID==id);
                return View(data);
            }
            else
            {
                var data = Db.Firmas.SingleOrDefault(x=>x.FirmaID==id); 
                if (data.Musteri == true)
                {
                    ViewBag.SayfaAdi = data.FirmaAdi + " Detayları";
                    return View(data);
                }
                else
                {
                    TempData[ERROR] = "Sadece müşterileri görüntüleyebilirsiniz.";
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        [Authorize(Roles = "Muhasebe")]
        public ActionResult CariHareketEkle()
        {
            ViewBag.SayfaAdi = "Carihareket Ekleme Bölümü";
            ViewBag.Personel = Db.Firmas.GetirFirmalar("");
            ViewBag.Kasa = Db.Firmas.GetirFirmalar("kasa");
            return View();
        }

        [Authorize(Roles = "Muhasebe")]
        [AcceptVerbs("POST"), ActionName("CariHareketEkle")]
        public ActionResult CariHareketEkle_Post(int day1, int month1, int year1, int? _chOdenen, int? _chAlinan, int personelID,
            int kasaID, string satisNot)
        {

            DateTime dt1 = new DateTime(year1, month1, day1);

            CariHesapHareketEkle(kasaID, _chOdenen ?? 0, _chAlinan ?? 0, satisNot, dt1, "odeme");

            CariHesapHareketEkle(personelID, _chAlinan ?? 0, _chOdenen ?? 0, satisNot, dt1, "odeme");

            return RedirectToAction("Detay", "CariHareket", new { id = personelID });
        }

        #endregion carihareket
        #region satislar
        [Authorize(Roles = "Muhasebe,Satis")]
        public ActionResult Satislar(int? p)
        {
            int baslangic = ((p ?? 1) - 1) * PagerCount;
            var datas = Db.Satis.GetirSatislar(PagerCount, baslangic);
            int totalCount = datas.Count();
            PaginatedList pager = new PaginatedList((p ?? 1), PagerCount, totalCount);

            ViewBag.Sayfalama = pager;
            return View(datas);
        }
        [Authorize(Roles = "Muhasebe")]
        public ActionResult SatisOdemeEkle(int? satisID)
        {
            IEnumerable<Sati> viewData = Db.Satis.GetirAltOdemeler(satisID ?? 0);
            ViewBag.SayfaAdi = "Satış Hareket Ekleme Bölümü";
            ViewBag.Satis = Db.Satis.GetirSatis(satisID ?? 0);
            ViewBag.Kasa = Db.Firmas.GetirFirmalar("kasa");
            return View(viewData);
        }

        [Authorize(Roles = "Muhasebe")]
        [AcceptVerbs("POST"), ActionName("SatisOdemeEkle")]
        public ActionResult SatisOdemeEkle_Post(int day1, int month1, int year1, int musteriID, int satisID,
                int? _musteridenAlinanOdeme, string satisNot, int? _araciyaOdenen, int araciID, int musteriKasaID, int araciKasaID)
        {
            DateTime dt1 = new DateTime(year1, month1, day1);

            Sati satisEkle = new Sati()
            {
                refSatisID = satisID,
                satisTarihi = dt1,
                musteridenAlinanOdeme = _musteridenAlinanOdeme,
                musteriFirmaID = musteriID,
                araciyaOdenen = _araciyaOdenen,
                araciFirmaID = araciID,
                satisNot = satisNot
            };
            Db.Satis.Add(satisEkle);
            Db.SaveChanges();

            CariHesapHareketEkle(musteriKasaID, 0, _musteridenAlinanOdeme ?? 0, satisNot, dt1, "satis");

            CariHesapHareketEkle(araciKasaID, _araciyaOdenen ?? 0, 0, satisNot, dt1, "araci");

            CariHesapHareketEkle(musteriID, _musteridenAlinanOdeme ?? 0, 0, satisNot, dt1, "satis");

            CariHesapHareketEkle(araciID, 0, _araciyaOdenen ?? 0, satisNot, dt1, "araci");

            return RedirectToAction("SatisOdemeEkle", "CariHareket", new { satisID = satisID });
        }   
           
               
      
        #endregion satislar
        [Authorize(Roles = "Muhasebe")]
        public ActionResult CariHareketSatisEkle()
        {
            ViewBag.SayfaAdi = "Cari Hareket Ekleme Bölümü";
            ViewBag.Musteriler = Db.Firmas.GetirFirmalar("musteri");
            ViewBag.Araci = Db.Firmas.GetirFirmalar("araci");
            ViewBag.Kasa = Db.Firmas.GetirFirmalar("kasa");
            return View();
        }

        [Authorize(Roles = "Muhasebe")]
        [AcceptVerbs("POST"), ActionName("CariHareketSatisEkle")]
        public ActionResult CariHareketSatisEkle_Post(int day1, int month1, int year1, int? _musteriSatis, int musteriID,
            int? _musteridenAlinanOdeme, string satisNot, int? _araciyaOdenen, int? _araciHakedis, int araciID,
            int satisKasaID, int araciKasaID)
        {

            DateTime dt1 = new DateTime(year1, month1, day1);

            Sati satisEkle = new Sati()
            {
                satisTarihi = dt1,

                musteriSatis = _musteriSatis,
                musteridenAlinanOdeme = _musteridenAlinanOdeme,
                musteriFirmaID = musteriID,

                araciHakedis = _araciHakedis,
                araciyaOdenen = _araciyaOdenen,
                araciFirmaID = araciID,

                satisNot = satisNot
            };
            Db.Satis.Add(satisEkle);
            Db.SaveChanges();

            CariHesapHareketEkle(satisKasaID, 0, _musteridenAlinanOdeme ?? 0, satisNot, dt1, "satis");
            CariHesapHareketEkle(araciKasaID, _araciyaOdenen ?? 0, 0, satisNot, dt1, "araci");
            CariHesapHareketEkle(musteriID, _musteridenAlinanOdeme ?? 0, _musteriSatis ?? 0, satisNot, dt1, "satis");
            CariHesapHareketEkle(araciID, _araciHakedis ?? 0, _araciyaOdenen ?? 0, satisNot, dt1, "araci");
            TempData[SUCESS] = "Kaydedildi";
            return RedirectToAction("Satislar", "CariHareket");
        }

        [Authorize(Roles = "Muhasebe")]
        public ActionResult PersonelMaasOdeme()
        {
            ViewBag.SayfaAdi = "Personel Maaş Ödeme Bölümü";
            ViewBag.Personel = Db.Firmas.GetirFirmalar("personel");
            ViewBag.Kasa = Db.Firmas.GetirFirmalar("kasa");
            return View();
        }

        [Authorize(Roles = "Muhasebe")]
        [AcceptVerbs("POST"), ActionName("PersonelMaasOdeme")]
        public ActionResult PersonelMaasOdeme_Post(int day1, int month1, int year1, int? _maasOdeme, int personelID,
            int kasaID, string satisNot)
        {

            DateTime dt1 = new DateTime(year1, month1, day1);

            CariHesapHareketEkle(kasaID, _maasOdeme ?? 0, 0, satisNot, dt1, "maas");

            CariHesapHareketEkle(personelID, 0, _maasOdeme ?? 0, satisNot, dt1, "maas");

            return RedirectToAction("Detay", "CariHareket", new { id = personelID });
        }
        private void CariHesapHareketEkle(int firmaID, int AlinanOdeme, int SatisFiyati, string not, DateTime tarih, string CariIslemTuru)
        {

            if (AlinanOdeme > 0)
            {
                CariHareket ch = new CariHareket()
                {
                    RefFirmaID = firmaID,
                    ChAlinanOdeme = AlinanOdeme,
                    ChNot = not,
                    ChTuru = CariIslemTuru,
                    ChTarihi = tarih
                };
                Db.CariHarekets.Add(ch);
                Db.SaveChanges();
            }

            if (SatisFiyati > 0)
            {
                CariHareket ch = new CariHareket()
                {
                    RefFirmaID = firmaID,
                    ChSatisFiyati = SatisFiyati,
                    ChNot = not,
                    ChTuru = CariIslemTuru,
                    ChTarihi = tarih
                };
                Db.CariHarekets.Add(ch);
                Db.SaveChanges();
            }

        }
    }
}