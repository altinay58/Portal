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
        public ActionResult List(int? p,string q,string s)
        {
            int baslangic = ((p ?? 1) - 1) * PagerCount;
            var datas = Db.CariHarekets.GetirCariHareketler(PagerCount, baslangic,q,s);

            int totalCount = Db.CariHarekets.Where(c=>(string.IsNullOrEmpty(q) ? true : c.Firma.FirmaAdi.ToUpper().Contains(q.ToUpper())))
                            .Count();
            PaginatedList pager = new PaginatedList((p ?? 1), PagerCount, totalCount);

            ViewBag.Sayfalama = pager;
            ViewData["queryData"] = q;
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

            CariHesapHareketEkle(kasaID, _chOdenen ?? 0, _chAlinan ?? 0, satisNot, dt1, "odemeKasa",null);

            CariHesapHareketEkle(personelID, _chAlinan ?? 0, _chOdenen ?? 0, satisNot, dt1, "odeme",null);
            Db.SaveChanges();
            return RedirectToAction("Detay", "CariHareket", new { id = personelID });
        }
        [Authorize(Roles ="Muhasebe")]
        public ActionResult CariHareketDuzenle(int id)
        {
            ViewBag.Kasa = Db.Firmas.GetirFirmalar("kasa");
            var model = Db.CariHarekets.SingleOrDefault(x => x.ChID == id);
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "Muhasebe")]
        public ActionResult CariHareketDuzenle(int id,DateTime chTarihi,int? firmaID,int? _chOdenen,int? _chAlinan,int? kasaID,string satisNot)
        {
            ViewBag.Kasa = Db.Firmas.GetirFirmalar("kasa");
            var entity = Db.CariHarekets.SingleOrDefault(x => x.ChID == id);
            //kasa
            if(entity.ChTuru=="odemeKasa" && _chOdenen.HasValue && entity.ChAlinanOdeme != null)
            {
                entity.ChAlinanOdeme = _chOdenen;
                entity.RefFirmaID = kasaID;
            }
            if (entity.ChTuru == "odemeKasa" && _chAlinan.HasValue && entity.ChSatisFiyati != null)
            {
                entity.ChSatisFiyati = _chAlinan;
                entity.RefFirmaID = kasaID;
            }
            //end kasa
            if (entity.ChTuru == "odeme" && _chOdenen.HasValue && entity.ChSatisFiyati != null)
            {
                entity.ChSatisFiyati = _chOdenen;
                entity.RefFirmaID = firmaID;
            }
            if (entity.ChTuru == "odeme" && _chAlinan.HasValue && entity.ChAlinanOdeme != null)
            {
                entity.ChAlinanOdeme = _chAlinan;
                entity.RefFirmaID = firmaID;
            }
            entity.ChTarihi = chTarihi;
            entity.ChNot = satisNot;
            Db.SaveChanges();
            TempData[SUCESS] = "Kaydedildi";
            return RedirectToAction("List");
        }

        // Cari hareketi sildiğimizde satış kısmındaki veriler değişmiyor. Kasadan girişi siliyoruz. Satıştan düşmüyor. Satış ödenmiş gibi duruyor.
        //[Authorize(Roles = "Muhasebe")]
        //public ActionResult CariHareketSil(int id)
        //{
        //    CariHareket entity = Db.CariHarekets.SingleOrDefault(x => x.ChID == id);
        //    Db.CariHarekets.Remove(entity);
        //    Db.SaveChanges();
        //    TempData[SUCESS] = "Kayıt silindi";
        //    return RedirectToAction("List");
        //}
        #endregion carihareket
        #region satislar
        [Authorize(Roles = "Muhasebe,Satis")]
        public ActionResult Satislar(int? p,string q)
        {
            if(User.IsInRole("MusteriTemsilcisi"))
            {
                TempData[ERROR] = "Bu bölüme giriş yetkiniz bulunmuyor.";
                return RedirectToAction("Index", "Home");
            }
            int baslangic = ((p ?? 1) - 1) * PagerCount;
            var datas = Db.Satis.GetirSatislar(PagerCount, baslangic,q);
            int totalCount = Db.Satis.Where(x => x.refSatisID == null && (string.IsNullOrEmpty(q) ? true : x.Firma.FirmaAdi.ToUpper().Contains(q.ToUpper())))
                            .Count();

            PaginatedList pager = new PaginatedList((p ?? 1), PagerCount, totalCount);

            ViewBag.Sayfalama = pager;
            ViewData["queryData"] = q;
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
          

            CariHesapHareketEkle(musteriKasaID, 0, _musteridenAlinanOdeme ?? 0, satisNot, dt1, "satisKasa",satisEkle);
            CariHesapHareketEkle(araciKasaID, _araciyaOdenen ?? 0, 0, satisNot, dt1, "araciKasa",satisEkle);
            CariHesapHareketEkle(musteriID, _musteridenAlinanOdeme ?? 0, 0, satisNot, dt1, "satis",satisEkle);
            CariHesapHareketEkle(araciID, 0, _araciyaOdenen ?? 0, satisNot, dt1, "araci", satisEkle);

            Db.SaveChanges();
            return RedirectToAction("SatisOdemeEkle", "CariHareket", new { satisID = satisID });
        }   
           
        public ActionResult SatisDuzenle(int satisID)
        {           
            ViewBag.Musteriler = Db.Firmas.GetirFirmalar("musteri");
            ViewBag.Araci = Db.Firmas.GetirFirmalar("araci");
            ViewBag.Kasa = Db.Firmas.GetirFirmalar("kasa");
            var satis = Db.Satis.GetirSatis(satisID);
            ViewBag.Satis = satis;
            ViewBag.Kasa = Db.Firmas.GetirFirmalar("kasa");
            
            return View(satis);
        }
        [Authorize(Roles = "Muhasebe")]
        [HttpPost]
        public ActionResult SatisDuzenle(Sati model,int? araciID, int satisKasaID,int araciKasaID)
        {
            Sati entity = Db.Satis.SingleOrDefault(x => x.SatisID == model.SatisID);
           
           // List<CariHareket> chList = Db.CariHarekets.Where(x => x.RefSatisId == model.SatisID).ToList();
          
            CariHareket chMusteriSatisFiyat = Db.CariHarekets.SingleOrDefault(x => x.RefSatisId == model.SatisID 
            && x.RefFirmaID==entity.musteriFirmaID && x.ChSatisFiyati!=null);
            if (chMusteriSatisFiyat != null)
            {
                chMusteriSatisFiyat.RefFirmaID = model.musteriFirmaID;
                chMusteriSatisFiyat.ChSatisFiyati = model.musteriSatis ?? 0;
                chMusteriSatisFiyat.ChTarihi = model.satisTarihi;
                chMusteriSatisFiyat.ChNot = model.satisNot;
            }
            else if(model.musteriSatis.HasValue && model.musteriSatis.Value>0)
            {
                var nchMusteriSatisFiyat = new CariHareket();
                nchMusteriSatisFiyat.RefFirmaID = model.musteriFirmaID;
                nchMusteriSatisFiyat.ChSatisFiyati = model.musteriSatis ?? 0;
                nchMusteriSatisFiyat.ChTarihi = model.satisTarihi;
                nchMusteriSatisFiyat.ChNot = model.satisNot;
                nchMusteriSatisFiyat.RefSatisId = model.SatisID;
                nchMusteriSatisFiyat.ChTuru = "satis";
                Db.CariHarekets.Add(nchMusteriSatisFiyat);
             
            }

            CariHareket chMusteriAlinanOdeme = Db.CariHarekets.SingleOrDefault(x => x.RefSatisId == model.SatisID
           && x.RefFirmaID == entity.musteriFirmaID && x.ChAlinanOdeme != null);
            if (chMusteriAlinanOdeme != null)
            {
                chMusteriAlinanOdeme.RefFirmaID = model.musteriFirmaID;
                chMusteriAlinanOdeme.ChAlinanOdeme = model.musteridenAlinanOdeme ?? 0;
                chMusteriAlinanOdeme.ChTarihi = model.satisTarihi;
                chMusteriAlinanOdeme.ChNot = model.satisNot;
            }else if(model.musteridenAlinanOdeme.HasValue && model.musteridenAlinanOdeme.Value > 0)
            {
                var nchMusteriAlinanOdeme = new CariHareket();
                nchMusteriAlinanOdeme.RefFirmaID = model.musteriFirmaID;
                nchMusteriAlinanOdeme.ChAlinanOdeme = model.musteridenAlinanOdeme ?? 0;
                nchMusteriAlinanOdeme.ChTarihi = model.satisTarihi;
                nchMusteriAlinanOdeme.ChNot = model.satisNot;
                nchMusteriAlinanOdeme.RefSatisId = model.SatisID;
                nchMusteriAlinanOdeme.ChTuru = "satis";
                Db.CariHarekets.Add(nchMusteriAlinanOdeme);
              
            }
            const int FIRMA_YOK = 6;
            if (araciID.HasValue && araciID!= FIRMA_YOK)
            {
                CariHareket chAraciAlinanOdeme = Db.CariHarekets.SingleOrDefault(x => x.RefSatisId == model.SatisID
       && x.RefFirmaID == entity.araciFirmaID && x.ChAlinanOdeme != null);
                if (chAraciAlinanOdeme != null)
                {
                    chAraciAlinanOdeme.RefFirmaID = araciID;
                    chAraciAlinanOdeme.ChAlinanOdeme = model.araciHakedis ?? 0;
                    chAraciAlinanOdeme.ChTarihi = model.satisTarihi;
                    chAraciAlinanOdeme.ChNot = model.satisNot;
                }
                else
                { 
                    var nchAraciAlinanOdeme = new CariHareket();
                    nchAraciAlinanOdeme.RefFirmaID = araciID;
                    nchAraciAlinanOdeme.ChAlinanOdeme = model.araciHakedis ?? 0;
                    nchAraciAlinanOdeme.ChTarihi = model.satisTarihi;
                    nchAraciAlinanOdeme.ChNot = model.satisNot;
                    nchAraciAlinanOdeme.RefSatisId = model.SatisID;
                    nchAraciAlinanOdeme.ChTuru = "araci";
                    Db.CariHarekets.Add(nchAraciAlinanOdeme);
                  
                }
                CariHareket chAraciSatisFiyat = Db.CariHarekets.SingleOrDefault(x => x.RefSatisId == model.SatisID
      && x.RefFirmaID == entity.araciFirmaID && x.ChSatisFiyati != null);
                if (chAraciSatisFiyat != null)
                {
                    chAraciSatisFiyat.RefFirmaID = araciID;
                    chAraciSatisFiyat.ChSatisFiyati = model.araciyaOdenen ?? 0;
                    chAraciSatisFiyat.ChTarihi = model.satisTarihi;
                    chAraciSatisFiyat.ChNot = model.satisNot;
                }else
                {
                    var nchAraciSatisFiyat = new CariHareket();
                    nchAraciSatisFiyat.RefFirmaID = araciID;
                    nchAraciSatisFiyat.ChSatisFiyati = model.araciyaOdenen ?? 0;
                    nchAraciSatisFiyat.ChTarihi = model.satisTarihi;
                    nchAraciSatisFiyat.ChNot = model.satisNot;
                    nchAraciSatisFiyat.RefSatisId = model.SatisID;
                    nchAraciSatisFiyat.ChTuru = "araci";
                    Db.CariHarekets.Add(nchAraciSatisFiyat);
                  
                }
                CariHareket araciKasa = Db.CariHarekets.SingleOrDefault(x => x.RefSatisId == model.SatisID
         && x.ChTuru == "araciKasa");
                if (araciKasa != null)
                {
                    araciKasa.RefFirmaID = satisKasaID;
                    araciKasa.ChAlinanOdeme = model.araciyaOdenen ?? 0;
                    araciKasa.ChTarihi = model.satisTarihi;
                    araciKasa.ChNot = model.satisNot;
                }else
                {
                    var naraciKasa = new CariHareket();
                    naraciKasa.RefFirmaID = satisKasaID;
                    naraciKasa.ChAlinanOdeme = model.araciyaOdenen ?? 0;
                    naraciKasa.ChTarihi = model.satisTarihi;
                    naraciKasa.ChNot = model.satisNot;
                    naraciKasa.RefSatisId = model.SatisID;
                    naraciKasa.ChTuru = "araciKasa";
                    Db.CariHarekets.Add(naraciKasa);
                    
                }

                CariHareket satisKasa = Db.CariHarekets.SingleOrDefault(x => x.RefSatisId == model.SatisID
              && x.ChTuru == "satisKasa");
                if (satisKasa != null)
                {
                    satisKasa.RefFirmaID = araciKasaID;
                    satisKasa.ChSatisFiyati = model.araciHakedis ?? 0;
                    satisKasa.ChTarihi = model.satisTarihi;
                    satisKasa.ChNot = model.satisNot;
                }
                else if (model.araciHakedis.HasValue && model.araciHakedis.Value > 0)
                {
                    var nsatisKasa = new CariHareket();
                    nsatisKasa.RefFirmaID = araciKasaID;
                    nsatisKasa.ChSatisFiyati = model.araciHakedis ?? 0;
                    nsatisKasa.ChTarihi = model.satisTarihi;
                    nsatisKasa.ChNot = model.satisNot;
                    nsatisKasa.RefSatisId = model.SatisID;
                    nsatisKasa.ChTuru = "satisKasa";
                    Db.CariHarekets.Add(nsatisKasa);

                }
            }
            else
            {
                //daha önce aracı var ise temizle
                List<CariHareket> list = Db.CariHarekets.Where(x => x.RefSatisId == model.SatisID && (x.ChTuru == "araci" || x.ChTuru == "araciKasa")).ToList();
                foreach(var item in list)
                {
                    Db.CariHarekets.Remove(item);
                }
            }
         

         

            entity.satisTarihi = model.satisTarihi;
            entity.musteriFirmaID = model.musteriFirmaID;
            entity.musteridenAlinanOdeme = model.musteridenAlinanOdeme;
            entity.musteriSatis = model.musteriSatis;
            entity.araciFirmaID = model.araciFirmaID;
            entity.araciyaOdenen = model.araciyaOdenen;
            entity.araciHakedis = model.araciHakedis;
            entity.satisNot = model.satisNot;
            entity.satisTarihi = model.satisTarihi;
            entity.araciFirmaID = araciID ?? FIRMA_YOK;
            Db.SaveChanges();
            TempData[SUCESS] = "Kaydedildi";
            return RedirectToAction("Satislar", "CariHareket");
        }
        [Authorize(Roles = "Muhasebe")]
        public ActionResult SatisSil(int id)
        {
            var carihareketler = Db.CariHarekets.Where(x => x.RefSatisId == id).ToList();
            foreach(var ch in carihareketler)
            {
                Db.CariHarekets.Remove(ch);
            }
            var satis = Db.Satis.SingleOrDefault(x => x.SatisID == id);
            Db.Satis.Remove(satis);
            Db.SaveChanges();
            TempData[SUCESS] = "Kayıt silindi";
            return RedirectToAction("Satislar");
        }
        public JsonResult SatisFiyatDegistir(int pk, int value)
        {
            JsonCevap jsn = new JsonCevap();
            Sati item = Db.Satis.SingleOrDefault(x => x.SatisID == pk);
            item.musteriSatis = value;
            Db.SaveChanges();
            return Json(jsn, JsonRequestBehavior.AllowGet);
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
          

            CariHesapHareketEkle(satisKasaID, 0, _musteridenAlinanOdeme ?? 0, satisNot, dt1, "satisKasa",satisEkle);
            CariHesapHareketEkle(araciKasaID, _araciyaOdenen ?? 0, 0, satisNot, dt1, "araciKasa",satisEkle);
            CariHesapHareketEkle(musteriID, _musteridenAlinanOdeme ?? 0, _musteriSatis ?? 0, satisNot, dt1, "satis",satisEkle);
            CariHesapHareketEkle(araciID, _araciHakedis ?? 0, _araciyaOdenen ?? 0, satisNot, dt1, "araci",satisEkle);

            Db.SaveChanges();
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

            CariHesapHareketEkle(kasaID, _maasOdeme ?? 0, 0, satisNot, dt1, "maas",null);

            CariHesapHareketEkle(personelID, 0, _maasOdeme ?? 0, satisNot, dt1, "maas",null);
            Db.SaveChanges();
            return RedirectToAction("Detay", "CariHareket", new { id = personelID });
        }
        private void CariHesapHareketEkle(int firmaID, int AlinanOdeme, int SatisFiyati, string not, DateTime tarih, string CariIslemTuru,Sati refSatis)
        {

            if (AlinanOdeme > 0)
            {
                CariHareket ch = new CariHareket()
                {
                    RefFirmaID = firmaID,
                    ChAlinanOdeme = AlinanOdeme,
                    ChNot = not,
                    ChTuru = CariIslemTuru,
                    ChTarihi = tarih,
                    Sati=refSatis
                };
                Db.CariHarekets.Add(ch);
                //Db.SaveChanges();
            }

            if (SatisFiyati > 0)
            {
                CariHareket ch = new CariHareket()
                {
                    RefFirmaID = firmaID,
                    ChSatisFiyati = SatisFiyati,
                    ChNot = not,
                    ChTuru = CariIslemTuru,
                    ChTarihi = tarih,
                    Sati = refSatis
                };
                Db.CariHarekets.Add(ch);
                //Db.SaveChanges();
            }

        }
    }
}