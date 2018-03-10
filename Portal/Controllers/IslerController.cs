using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Portal.Models.IslerModels;
using System.Data.Entity;
using Portal.Filters;

namespace Portal.Controllers
{
    public class IslerController : BaseController
    {




       public IslerController()
        {
            ViewBag.guncelMenu = "Isler";
        }
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

        // Summary:
        //     Domain e ait is bilgileri goserilen sayfa
        // Parameters:
        //   id:domain id
        //  
        [HttpPost]
        public ActionResult TeknikRapor(string islerisiYapacakKisi, DateTime? basTarih, DateTime? bitisTarih)
        {

            if (basTarih == null)
            {
                basTarih = DateTime.Now;
            }

            if (bitisTarih == null)
            {
                bitisTarih = DateTime.Now;
            }

            ViewBag.kullanicilar = Db.AspNetUsers.Where(x => x.LockoutEnabled == false).ToList();

            if (!String.IsNullOrEmpty(islerisiYapacakKisi))
            {
                List<string> yeniisiYapacakKisiler = islerisiYapacakKisi.Split(',').ToList();
                IEnumerable<TeknikRapor> rapor = Db.TeknikRapors.Where(a => a.TeknikRaporTarih >= basTarih
            && a.TeknikRaporTarih <= bitisTarih
            && yeniisiYapacakKisiler.Contains(a.RefTeknikRaporUserID)).OrderBy(x => x.RefTeknikRaporUserID).ToList();
                return View(rapor);
            }
            else
            {
                IEnumerable<TeknikRapor> rapor = Db.TeknikRapors.Where(a => a.TeknikRaporTarih >= basTarih
            && a.TeknikRaporTarih <= bitisTarih).OrderBy(x => x.RefTeknikRaporUserID).ToList();
                return View(rapor);
            }
        }

        public ActionResult TeknikRapor()
        {
            ViewBag.kullanicilar = Db.AspNetUsers.Where(x => x.LockoutEnabled == false).ToList();
            DateTime tarih = DateTime.Now.Date;
           IEnumerable<TeknikRapor> rapor = Db.TeknikRapors.Where(a => a.TeknikRaporTarih == tarih).OrderBy(x=>x.RefTeknikRaporUserID).ThenByDescending(x=>x.TeknikRaporTarih).ToList();

            return View(rapor);
        }


        #region domain isler   
        public ActionResult DomainIsler(int? id)
        {
            id = id ?? 13448;
            ViewBag.domainId = id;
            var domain = Db.Domains.SingleOrDefault(x=>x.DomainID==id);
            ViewBag.kullanicilar = Db.AspNetUsers.Where(x => x.LockoutEnabled == false).ToList() ;
            //todo:login ekrani olunca değiştirilecek
            //User.Identity.IsAuthenticated ? 
            string userId= User.Identity.GetUserId() ?? "f5f53da2-c311-44c9-af6a-b15ca29aee57";
            ViewBag.guncelKullanici = Db.AspNetUsers.Where(x => x.Id == userId).
                                      Select(x => new Kullanici { Id = x.Id, AdSoyad = x.Isim + " " + x.SoyIsim }).FirstOrDefault();

            return View(domain);
        }
        //[OutputCache(Duration = 3600, VaryByParam = "domainId")]
        public JsonResult DomainAitIsler(int domainId)
        {
            bool adminMi = User.IsInRole("Muhasebe");
            string userId = User.Identity.GetUserId();
            var list = (from p in Db.islers.Include(x => x.IsiYapacakKisis).Include(x=>x.isYorums)
                        join a in Db.AspNetUsers on p.islerisiVerenKisi equals a.Id
                        join z in Db.ZamanIs on p.islerID equals z.RefIsId into temp
                        from zz in temp.DefaultIfEmpty()
                        let users = p.IsiYapacakKisis.Select(x=>x.RefIsiYapacakKisiUserID)
                        let isAdmin = adminMi
                        where p.islerIsinDurumu != 5 && p.islerRefDomainID == domainId && (isAdmin || (p.islerisiVerenKisi == userId || users.Contains(userId)))
                       // where p.islerRefDomainID == domainId && (isAdmin || (p.islerisiVerenKisi == userId || users.Contains(userId)))
                        orderby p.islerIsinDurumu ascending,p.islerID descending
                        select new DomainIs
                        {
                            IsId = p.islerID,
                            IsAd = p.islerAdi,
                            IsAciklama = p.islerAciklama,
                            FirmaId = p.islerRefFirmaID.Value,
                            IsiVerenKullanici=new Kullanici { Id=p.islerisiVerenKisi,AdSoyad=a.Isim+" "+a.SoyIsim},
                            IsDurum=p.islerIsinDurumu,
                            SiraNo=p.islerSiraNo ?? 1,
                            BitisTarihiVarmi=p.islerBitisTarihiVarmi,
                            BitisTarihi=p.islerBitisTarihi,
                            Tarih=p.islerTarih,
                            Yorumlar=(from y in p.isYorums
                                      join q in Db.AspNetUsers on y.isYorumRefYorumuYapanID equals q.Id
                                      orderby y.isYorumKayitTarih ascending
                                      select new YorumIs
                                      { KullaniciId=q.Id,Aciklama=y.isYorumAciklama,Tarih=y.isYorumKayitTarih.Value,AdSoyad=q.Isim+" "+q.SoyIsim}
                                      ).ToList(),
                            IsGecenZaman=new GecenZaman { GecenZamanSaniye=zz.GecenZamanSaniye,ZamanBasTarih=zz.ZamanIsBasTarih},
                            
                            IsiYapacakKullanicilar = (from pf in p.IsiYapacakKisis
                                                      join q in Db.AspNetUsers on pf.RefIsiYapacakKisiUserID equals q.Id
                                                      where pf.RefIsID == p.islerID
                                                      select new Kullanici {
                                                          Id = pf.RefIsiYapacakKisiUserID,AdSoyad=q.Isim+" "+q.SoyIsim
                                                      }
                                                    ).ToList()
                        }
                        
                        );
            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

           
        }
        [JsonNetFilter]
        public JsonResult GetDate()
        {
            return Json(new { Date=DateTime.Now,DateStr=DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")},JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        [JsonNetFilter]
        public JsonResult IsDurumuDegistir(string domainIs,byte yeniDurum)
        {
            IsinDurumu yeniisDurum = (IsinDurumu) yeniDurum;
            var microsoftDateFormatSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            };
            DomainIs obj = JsonConvert.DeserializeObject<DomainIs>(domainIs, microsoftDateFormatSettings);
            JsonCevap jsn = new JsonCevap();
            jsn.Basarilimi = true;
            try
            {
                ZamanI zamanIs = Db.ZamanIs.SingleOrDefault(x => x.RefIsId == obj.IsId);
                if(zamanIs!=null)
                {
                    if (yeniisDurum == IsinDurumu.Yapiliyor) // İşi başlattığı zaman burası çalışır İş Yeşil olur
                    {                       
                        zamanIs.ZamanIsBasTarih = DateTime.Now;                        
                        obj.IsGecenZaman.ZamanBasTarih = DateTime.Now;                      
                    }
                    else
                    {
                        //islerIsinDurumu Yapilacak veya YapilacakDeadline
                        if (obj.IsDurum == (int)IsinDurumu.Yapiliyor) // Durum Yapılıyorken Tamamlaya yada durdura bastığımızda bu kısım çalışıyor. Tamamlaya basarsak isdurum 4  KontrolBekleyen olur. durdura basarsak isdurum 1 Yapilacak olur.
                        {
                            var diffInSeconds = (DateTime.Now - obj.IsGecenZaman.ZamanBasTarih.Value).TotalSeconds;
                            zamanIs.GecenZamanSaniye = zamanIs.GecenZamanSaniye + (long)diffInSeconds;
                            zamanIs.ZamanIsBasTarih = DateTime.Now;
                            obj.IsGecenZaman.ZamanBasTarih = DateTime.Now;
                            obj.IsGecenZaman.GecenZamanSaniye = zamanIs.GecenZamanSaniye;

                            TeknikRaporEkle(obj.IsId, (int)yeniisDurum, (long)diffInSeconds);

                        }
                      
                    }
                }
                else
                {
                    //zamanis kayit yok ise islerIsinDurumu Yapilacak veya YapilacakDeadline dir
                    zamanIs = new ZamanI() { GecenZamanSaniye = 0, RefIsId = obj.IsId, ZamanIsBasTarih = DateTime.Now };
                    Db.ZamanIs.Add(zamanIs);
                    obj.IsGecenZaman.ZamanBasTarih = DateTime.Now;
                    obj.IsGecenZaman.GecenZamanSaniye = 0;

                }



                isler job = Db.islers.SingleOrDefault(x => x.islerID == obj.IsId);
                job.islerIsinDurumu = (int)yeniisDurum;
                obj.IsDurum= (int)yeniisDurum;
                jsn.Data = obj;
                Db.SaveChanges();
            }
            catch(Exception exc)
            {
                jsn.Data = exc.Message;
                jsn.Basarilimi = false;
            }
          
            return Json(jsn,JsonRequestBehavior.AllowGet);
        }

        public void TeknikRaporEkle(int isID, int isDurum, long isZaman)
        {
            DateTime tarih = DateTime.Now.Date;
            string userID = User.Identity.GetUserId();

            TeknikRapor Rapor = Db.TeknikRapors.FirstOrDefault(a => a.TeknikRaporTarih == tarih && a.RefTeknikRaporUserID == userID && a.RefTeknikRaporIsID == isID);

            if (Rapor == null)
            {
                TeknikRapor tekRap = new TeknikRapor()
                {
                    RefTeknikRaporIsID = isID,
                    TeknikRaporDurum = isDurum,
                    TeknikRaporTarih = DateTime.Now.Date,
                    TeknikRaporZaman = isZaman,
                    RefTeknikRaporUserID = userID
                };
                Db.TeknikRapors.Add(tekRap);
            }
            else
            {
                Rapor.TeknikRaporDurum = isDurum;
                Rapor.TeknikRaporZaman = Rapor.TeknikRaporZaman + isZaman;
            }
            Db.SaveChanges();
        }


        public JsonResult GetirDomainNotlari(int domainId)
        {
            var list = (from dn in Db.DomainNots
                        join d in Db.Domains on dn.RefDomainId equals d.DomainID
                        join f in Db.Firmas on d.RefDomainFirmaID equals f.FirmaID
                        join u in Db.AspNetUsers on dn.RefNotKullaniciId equals u.Id
                        orderby dn.DomainNotTarih descending
                        where dn.RefDomainId==domainId
                        select new
                        {
                            Id = dn.DomainNotId,
                            Note = dn.DomainNotNot,
                            DomainAdi = d.DomainAdi,
                            DomainId = d.DomainID,
                            FirmaAdi = f.FirmaAdi,
                            AdSoyad = u.Isim + " " + u.SoyIsim,
                            Tarih = dn.DomainNotTarih
                        }
                     ).ToList();
            return Json(list,JsonRequestBehavior.AllowGet);

        }
        public JsonResult FirmaKisiler(int firmaId)
        {
            var list = (from fl in Db.FirmaKisis.Include(x => x.Firma)
                        orderby fl.Firma.FirmaAdi descending                      
                        select new
                        {
                            Id = fl.Id,
                            Ad = fl.Ad,
                            Soyad = fl.Soyad,
                            Departman = fl.Departman,
                            Tel = fl.Tel,
                            Email = fl.Email,
                            FirmaAdi = fl.Firma.FirmaAdi
                        }
                       ); 
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        public JsonResult YorumKaydet(string kullaniciId,string aciklama,int isId)
        {
            JsonCevap jsn = new JsonCevap();

            jsn.Basarilimi = true;
            isYorum yorum = new isYorum();
            yorum.isYorumAciklama = aciklama;
            yorum.isYorumKayitTarih = DateTime.Now;
            yorum.isYorumRefYorumuYapanID = kullaniciId;
            yorum.isYorumRefislerID = isId;
            Db.isYorums.Add(yorum);

            isler hangiIS = Db.islers.FirstOrDefault(a => a.islerID == isId);

            if (hangiIS.islerisiVerenKisi == kullaniciId)
            {
                List<string> idler;


                idler = Db.IsiYapacakKisis.Where(a => a.RefIsID == hangiIS.islerID).Select(x => x.RefIsiYapacakKisiUserID).ToList();

                int idNo = idler.IndexOf(kullaniciId);

                if (Db.YorumDurums.FirstOrDefault(a => a.RefIsID == hangiIS.islerID && idNo != -1) == null)
                {
                    YorumDurum durum = new YorumDurum();
                    durum.RefIsID = hangiIS.islerID;
                    durum.RefUserID = kullaniciId;
                    Db.YorumDurums.Add(durum);
                 
                }

            }
            else
            {
                if (Db.YorumDurums.FirstOrDefault(a => a.RefIsID == hangiIS.islerID && a.RefUserID == hangiIS.islerisiVerenKisi) == null)
                {
                    YorumDurum durum = new YorumDurum();
                    durum.RefIsID = hangiIS.islerID;
                    durum.RefUserID = hangiIS.islerisiVerenKisi;
                    Db.YorumDurums.Add(durum);
                 
                }
            }
            Db.SaveChanges();
            return Json(jsn,JsonRequestBehavior.AllowGet);
        }
        public JsonResult YorumDurumDegistir(int isId,string kullaniciId)
        {
            JsonCevap jsn = new JsonCevap();
            if (Db.YorumDurums.FirstOrDefault(a => a.RefIsID == isId && a.RefUserID == kullaniciId) != null)
            {
                YorumDurum yorumDur = Db.YorumDurums.FirstOrDefault(a => a.RefIsID == isId && a.RefUserID == kullaniciId);
                Db.YorumDurums.Remove(yorumDur);
                Db.SaveChanges();
            }
            return Json(jsn, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DegistirSatisOncelik(int domainId, bool value)
        {
            JsonCevap jsn = new JsonCevap();
            var domain = Db.Domains.SingleOrDefault(x=>x.DomainID==domainId);
            domain.SatisOncelikli = value;
            Db.Database.ExecuteSqlCommand(@"update isler set islerOncelikSiraID=@p0 
                                         where islerRefDomainID=@p1",(int)IslerOncelikSira.Birinci,domainId);
           
            Db.SaveChanges();
            return Json(jsn, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DegistirGuncellemeSozlesmesi(int domainId, bool value)
        {
            JsonCevap jsn = new JsonCevap();
            var domain = Db.Domains.SingleOrDefault(x => x.DomainID == domainId);
            domain.GuncellemeSozlesmesiVarmi = value;
            Db.SaveChanges();
            return Json(jsn, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DegistirOdemesiAlindi(int domainId, bool value)
        {
            JsonCevap jsn = new JsonCevap();
            var domain = Db.Domains.SingleOrDefault(x => x.DomainID == domainId);
            domain.OdemesiAlindi = value;
            Db.SaveChanges();
            return Json(jsn, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DomainAksiyonDegistir(int domainId, int domainAksiyon)
        {
            JsonCevap jsn = new JsonCevap();
            DomainAksiyon yeniAksiyon = (DomainAksiyon)domainAksiyon;
            var domain = Db.Domains.SingleOrDefault(x => x.DomainID == domainId);
            if(yeniAksiyon==DomainAksiyon.BeklemeyeAl || yeniAksiyon == DomainAksiyon.YayiniDurdur)
            {
                Db.Database.ExecuteSqlCommand( @" UPDATE dbo.isler  
SET     islerIsinDurumu =  CASE when  
								islerIsinDurumu=3 
								then 
								case when
									islerBitisTarihiVarmi=1
									then 2--deadline
									else 1--yapilacak
								end
						   end
WHERE   islerRefDomainID=@p0 and islerIsinDurumu=3",domainId);
            }
            domain.DomainAksiyon = (int)yeniAksiyon;
            Db.SaveChanges();
            jsn.Basarilimi = true;
            return Json(jsn, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetirDomainSonNot(int domainId)
        {
            var list = Db.DomainNots.Where(x => x.RefDomainId == domainId).OrderByDescending(x => x.DomainNotTarih)
                .Take(1).SingleOrDefault();
            return Json(list!=null ? list.DomainNotNot: "", JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult DomainArama(string term)
        {
            var domainler = new List<object>();

            IEnumerable<Domain> veriler = from sonuclar in Db.Domains
                                                                    where  sonuclar.DomainAdi.Contains(term.ToLower())
                                                                    select sonuclar;

            foreach (Domain domainn in veriler)
            {
                domainler.Add(new { id = domainn.DomainID.ToString(), text = domainn.DomainAdi, firmaadi = domainn.Firma.FirmaAdi , firmaid = domainn.Firma.FirmaID  });
            }

            return this.Json(domainler, JsonRequestBehavior.AllowGet);
        }


        #region içerik formu
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
            var isAtanacakKullanici = Db.Ayars.Where(x=>x.AyarAdi== "IcerikFormuIsAtanacakKullanici").SingleOrDefault();
            var isKontrolEdenKullanici = Db.Ayars.Where(x=>x.AyarAdi== "IcerikFormuIsKontrolEdenKullanici").SingleOrDefault();
            if (ModelState.IsValid && (isAtanacakKullanici!=null && isKontrolEdenKullanici!=null) &&
                (!string.IsNullOrEmpty(isAtanacakKullanici.AyarDeger) && !(string.IsNullOrEmpty(isKontrolEdenKullanici.AyarDeger)) ))
            {
                var listStandardIsler = Db.StandartProjeIsleris.Where(x=>string.IsNullOrEmpty(x.StandartProjeIsleriIdAnahtarIsmi)).ToList().OrderBy(x => x.StandartProjeIsleriSirasi);
                var dinamiStandartIsler = Db.StandartProjeIsleris.Where(x => x.StandartProjeIsleriIdAnahtarIsmi != null).OrderBy(x=>x.StandartProjeIsleriSirasi).ToList();
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
                        isHtml += string.Format("<p>{0} Alındı:{1}</p>", dinamikIs.StandartProjeIsleriIdAnahtarIsmi,frm[dinamikIs.StandartProjeIsleriIdAnahtarIsmi]);
                    }else
                    {
                        isHtml += string.Format("<p>{0} Alınmadı</p>", dinamikIs.StandartProjeIsleriIdAnahtarIsmi);
                    }
                }

                isler ilkIs = new isler();
                ilkIs.islerAciklama = isHtml;
                ilkIs.islerAdi=icerik.DomainAdi+" bilgileri";
                ilkIs.islerRefDomainID = icerik.DomainId;
                ilkIs.islerRefFirmaID = icerik.FirmaId;
                ilkIs.islerisiYapacakKisi = isAtanacakKullanici.AyarDeger;
                //degişebilir
                ilkIs.islerisiVerenKisi = isKontrolEdenKullanici.AyarDeger;
                ilkIs.islerTarih = DateTime.Now;
                ilkIs.islerOncelikSiraID =(int) IslerOncelikSira.Ikinci;
                //List<isler> isler = new List<Models.isler>();
                //isler.Add(ilkIs);
                ilkIs.islerSiraNo = 1;
                ilkIs.islerIsinDurumu = (int)IsinDurumu.Yapilacak;
                IsiYapacakKisi kisi2 = new IsiYapacakKisi();
                kisi2.RefIsiYapacakKisiUserID = isAtanacakKullanici.AyarDeger;
                kisi2.isler = ilkIs;
                Db.IsiYapacakKisis.Add(kisi2);
                Db.islers.Add(ilkIs);
                int siraNo = 2;
                foreach (var standardIs in listStandardIsler)
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
                    job.islerSiraNo = siraNo;
                    job.islerIsinDurumu = (int)IsinDurumu.Yapilacak;
                    siraNo = siraNo + 1;
                    IsiYapacakKisi kisi = new IsiYapacakKisi();
                    kisi.RefIsiYapacakKisiUserID = standardIs.RefStandartProjeIsleriYapacakKullaniciId;
                    kisi.isler = job;
                    Db.IsiYapacakKisis.Add(kisi);
                    Db.islers.Add(job);
                }
                //Db.islers.AddRange(isler);
                Db.SaveChanges();
                TempData["Success"] = "Kaydedildi";

                return RedirectToAction("ListIsler");

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
        public JsonResult FirmalariGetir(string firmaAdi,bool sadeceFirma=true,bool sadeceArayanlar=true)
        {


            var firmalar = new List<object>();
            if (sadeceFirma)
            {

                IEnumerable<int> integerDizi = Db.FirmaKisis.Where(a => a.Tel.Contains(firmaAdi) || a.Tel2.Contains(firmaAdi) || a.Email.Contains(firmaAdi) || a.Ad.Contains(firmaAdi)).Select(a => a.FirmaId);

                var list = from sonuclar in Db.Firmas
                       where integerDizi.Contains(sonuclar.FirmaID) || sonuclar.FirmaAdi.Contains(firmaAdi)
                       select new { Firma = sonuclar };

                foreach (var firma in list)
                {
                    firmalar.Add(new
                    {
                        value = firma.Firma.FirmaID,
                        label =  firma.Firma.FirmaAdi, // TODO : firma.Kisi.Ad + " " + firma.Kisi.Soyad + " " + İSİM SOYİSİM EKLENİYOR
                        Adres = firma.Firma.FirmaAdres,
                        Kayitlimi = true,
                        Sehir = firma.Firma.firmaSehir,
                        ilce = firma.Firma.firmailce,
                        KonumId = firma.Firma.RefKonumID,
                        SektorId = firma.Firma.firmaSektorID,
                        DomainKategoriId = firma.Firma.firmaDomainKategoriID,
                        //Telefon1 = firma.Kisi.Tel,
                        //Telefon2 = firma.Kisi.Tel2,
                        //Email = firma.Kisi.Email,
                        WebAdresi = firma.Firma.FirmaAdres,
                        //Adi = firma.Kisi.Ad,
                        //Soyadi = firma.Kisi.Soyad,
                        //FirmaSahibiOzellik = firma.Kisi.Departman
                    });
                }
            }
        

            if (sadeceArayanlar)
            {
                var diziArayanlar = Db.Arayanlars.Where(a => a.RefFirmaID == null && (a.arayanFirmaAdi.Contains(firmaAdi) || a.arayanCepTelNo.Contains(firmaAdi)
            || a.arayanTelefonNo.Contains(firmaAdi) || a.arayanMailAdresi.Contains(firmaAdi)));

                List<int> listTemp = new List<int>();

                //.Select(o => new { ArayanAdi = o.Key, Arayan = o.OrderBy(c => c.arayanFirmaAdi).ToList() }).ToList();

                foreach (var arayan in diziArayanlar)
                {
                    //Arayanlar arayanFirma = db.Arayanlars.FirstOrDefault(a => a.arayanFirmaAdi == aramaYapan.ArayanAdi);
                    //if (arayanFirma != null)
                    //{
                    //    firmalar.Add(new { value = "kayitliDegil" + arayanFirma.arayanID, label = arayanFirma.arayanFirmaAdi + " - Kayıtlı Değil" });
                    //}
                    if (!listTemp.Contains(arayan.arayanID))
                    {
                        firmalar.Add(new
                        {
                            value = arayan.arayanID,
                            label = arayan.arayanFirmaAdi,// +" Kayıtlı Değil",
                            Telefon1 = arayan.arayanTelefonNo,
                            Telefon2 = arayan.arayanCepTelNo,
                            Email = arayan.arayanMailAdresi,
                            Adres = arayan.arayanAdres,
                            Kayitlimi = false,
                            Adi = arayan.arayanAdi,
                            Soyadi = arayan.arayanSoyadi,
                            FirmaSahibiOzellik = arayan.arayanFirmaSahibiOzelligi,
                            Sehir = arayan.arayanSehir,
                            ilce = arayan.arayanilce,
                            WebAdresi = arayan.arayanWebAdresi,
                            KonumId = arayan.arayanRefKonumID,
                            SektorId = arayan.arayanSektorID,
                            DomainKategoriId = arayan.arayanDomainKategoriID
                        });
                        listTemp.Add(arayan.arayanID);
                    }

                }
            }
            

            return Json(firmalar, JsonRequestBehavior.AllowGet);
        }
        public JsonResult KisileriGetir(int? firmaId)
        {
            var listKisi = (from d in Db.FirmaKisis
                              where (firmaId.HasValue ? d.FirmaId == firmaId.Value : true)
                              select new
                              {
                                  value = d.Id,
                                  label = d.Ad + " " + d.Soyad,
                                  firmaId = d.FirmaId,
                                  Telefon1 = d.Tel,
                                  Telefon2 = d.Tel2,
                                  Email = d.Email,
                              }).ToList();



            return Json(listKisi, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DomainGetir(string domainAdi,int? firmaId)
        {
        

            var listDomain = (from d in Db.Domains
                              let kisi = d.Firma.FirmaKisis.FirstOrDefault()
                              where (firmaId.HasValue ? d.RefDomainFirmaID == firmaId.Value : true) 
                              && (string.IsNullOrEmpty(domainAdi) ? true: d.DomainAdi.Contains(domainAdi))
                              select new
                              {
                                  value = d.DomainID,label=d.DomainAdi,firmaId=d.Firma.FirmaID,firmaAdi=d.Firma.FirmaAdi,
                                  Telefon1 = kisi != null ? kisi.Tel : "",
                                  Telefon2 = kisi != null ? kisi.Tel2 : "",
                                  Email = kisi != null ? kisi.Email : "",
                                  Adres = d.Firma.FirmaAdres
                              }).ToList();

          

            return Json(listDomain, JsonRequestBehavior.AllowGet);
        }
        #endregion içerik formu
        #region iş ekle
        public ActionResult IsEkleDuzenle(int id)
        {
            ViewBag.kullanicilar = Db.AspNetUsers.Where(x => x.LockoutEnabled == false).ToList();
            ViewBag.isOncelikler = Db.isOncelikSiras;

            isler entity = new isler();
            ViewBag.domainler = null;
            if (Request.UrlReferrer != null)
            {
                ViewBag.oncekiSayfa = Request.UrlReferrer.ToString();
            }
            
                entity = Db.islers.SingleOrDefault(x => x.islerID == id);
                ViewBag.domainler = Db.Domains.Where(x => x.RefDomainFirmaID == entity.islerRefFirmaID);
            
            return View(entity);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult IsEkleDuzenle(isler model)
        {
            try
            {
                isler entity = new isler();
                List<IsiYapacakKisi> isiDbYapanKullanicilar = new List<IsiYapacakKisi>();
                if (model.islerID > 0)
                {
                    entity = Db.islers.SingleOrDefault(x => x.islerID == model.islerID);
                    isiDbYapanKullanicilar = Db.IsiYapacakKisis.Where(x => x.RefIsID == model.islerID).ToList();
                }
                else
                {
                    entity.islerTarih = DateTime.Now;
                    entity.islerinisinOnayDurumu = false;
                    entity.islerIsinDurumu = (int)IsinDurumu.Yapilacak;
                    int siraNo = Db.islers.Where(x => x.islerRefDomainID == model.islerRefDomainID).Max(x => x.islerSiraNo) ?? 0;
                    entity.islerSiraNo = siraNo + 1;
                    Db.islers.Add(entity);

                }
                entity.islerAdi = Fonksiyonlar.KarakterDuzenle(model.islerAdi);
                entity.islerAciklama = Fonksiyonlar.KarakterDuzenle(model.islerAciklama);
                var varmi = Db.islers.Where(x => 
               // x.islerAdi == entity.islerAdi && 
                x.islerRefDomainID == entity.islerRefDomainID &&
                x.islerAciklama == entity.islerAciklama && 
                (entity.islerID>0 ? x.islerID == x.islerID : true)
                ).Count() > 0;
                if (varmi)
                {
                    TempData[ERROR] = "Bu iş daha önceden kayıt edilmiş";
                    ViewBag.kullanicilar = Db.AspNetUsers.Where(x => x.LockoutEnabled == false).ToList();
                    ViewBag.isOncelikler = Db.isOncelikSiras;
                    ViewBag.domainler = null;
                    if (entity.islerID > 0)
                    {
                        ViewBag.domainler = Db.Domains.Where(x => x.RefDomainFirmaID == entity.islerRefFirmaID);
                    }
                    return View(entity);
                }

                if (model.islerRefFirmaID == null)
                {
                    entity.islerRefFirmaID = Db.Domains.FirstOrDefault(a => a.DomainID == model.islerRefDomainID).RefDomainFirmaID;
                }
                else
                {
                    entity.islerRefFirmaID = model.islerRefFirmaID;
                }
                entity.islerRefDomainID = model.islerRefDomainID;
              
                entity.islerOncelikSiraID = model.islerOncelikSiraID;
                entity.islerBitisTarihiVarmi = model.islerBitisTarihiVarmi;
                entity.islerOncelikSiraID = model.islerOncelikSiraID;
             
                if (model.islerBitisTarihiVarmi)
                {
                    entity.islerBitisTarihi = model.islerBitisTarihi;
                }
                List<string> yeniisiYapacakKisiler = Request["islerisiYapacakKisi"].Split(',').ToList();
                foreach (string userId in yeniisiYapacakKisiler)
                {
                    if (isiDbYapanKullanicilar.FindIndex(x => x.AspNetUser.Id == userId) == -1)
                    {
                        IsiYapacakKisi kisi = new IsiYapacakKisi();
                        kisi.RefIsiYapacakKisiUserID = userId;
                        kisi.isler = entity;
                        Db.IsiYapacakKisis.Add(kisi);
                    }
                }
                foreach (var kullanici in isiDbYapanKullanicilar)
                {
                    if (yeniisiYapacakKisiler.FindIndex(x => x == kullanici.AspNetUser.Id) == -1)
                    {
                        Db.IsiYapacakKisis.Remove(kullanici);
                    }
                }
                entity.islerisiVerenKisi = model.islerisiVerenKisi ?? User.Identity.GetUserId();

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
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            
           
        }
        #endregion iş ekle
        #region list işler
        public ActionResult ListIsler()
        {
            string userId = User.Identity.GetUserId() ?? "f5f53da2-c311-44c9-af6a-b15ca29aee57";
            ViewBag.guncelKullanici = Db.AspNetUsers.Where(x => x.Id == userId).
                                    Select(x => new Kullanici { Id = x.Id, AdSoyad = x.Isim + " " + x.SoyIsim }).FirstOrDefault();
            return View();
        }
        public JsonResult ListIsAra(int page,string basTarih, string bitisTarih, string isAdi)
        {
            int baslangic = (page - 1) * PagerCount;
            JsonCevap jsn = new JsonCevap();
            var query = Db.IslerListesis.Where(x => (!string.IsNullOrEmpty(isAdi) ? x.IsAdi.Contains(isAdi) : true));
            if (!string.IsNullOrEmpty(basTarih) && !string.IsNullOrEmpty(bitisTarih))
            {
                DateTime tBas = DateTime.Parse(basTarih);
                DateTime tBit = DateTime.Parse(bitisTarih).AddHours(23).AddMinutes(59);
                jsn.ToplamSayi = query.Where(x => x.Tarih >= tBas && x.Tarih <= tBit).Count();
                query = query.Where(x => x.Tarih >= tBas && x.Tarih <= tBit).OrderByDescending(x => x.Tarih).Skip(baslangic).Take(PagerCount);
                jsn.Data = query.ToList();
            }
            else
            {
                jsn.ToplamSayi = query.Count();
                query = query.OrderByDescending(x => x.Tarih).Skip(baslangic).Take(20);
                jsn.Data = query.ToList();
            }
            return Json(jsn, JsonRequestBehavior.AllowGet);
        }
        #endregion list işler
    }

}