using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;
using System.Threading.Tasks;
using Microsoft.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace Portal.Controllers
{
    public class DomainController : BaseController
    {
        const int  sayfada_gosterilecek_domain_sayisi = 10;
        public DomainController()
        {
            ViewBag.guncelMenu = "Domainler";
        }
        // GET: Domain
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Ekle()
        {
            //KayitliFirma yerine DomainKayitliFirma tablosu acilacak
            SetViewBagEkle();
            return View();
        }      

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Ekle(Portal.Models.Domain domain,FormCollection frm)
        {
            if (Db.Domains.DomainEklimi(domain.DomainAdi))
            {
                SetViewBagEkle();
                TempData[ERROR] = "Domain daha önce eklenmiş!";
                return View(domain);
            }

            if (ModelState.IsValid)
            {
                domain.DomainDurum = true;
                domain.UzatmaTarihi = DateTime.Now.AddYears(-1);
                Db.Domains.Add(domain);
                Db.SaveChanges();
                SetViewBagEkle();
                TempData[SUCESS] = "Domain Kaydedildi";
                return View(domain);
            }
            
            return View();
        }
        public ActionResult Domainler(int? page,string ara,string aramaType)
        {
            if(!String.IsNullOrEmpty(ara))
            {
                ara = ara.Trim();
            }

            Dictionary<string, string> aramatypes = new Dictionary<string, string>
    { {"UzatmaTarihi","Uzatma Tarihine Göre" } , {"Alfabetik","Alfabetik" } , {"Hosting","Hosting" } , {  "Ip", "Ip"} };
            ViewBag.aramatypes = aramatypes;
            int domainBaslangic = ((page ?? 1) - 1) * PagerCount;
            var viewData = Db.Domains.Where(x => ((!string.IsNullOrEmpty(ara)) ? x.DomainAdi.ToUpper().Contains(ara.ToUpper()) : true)
            || ((!string.IsNullOrEmpty(ara)) ? x.Firma.FirmaAdi.Contains(ara) : true)
            || (!string.IsNullOrEmpty(ara) ? x.Hosting.HostingAdi.ToUpper().Contains(ara.ToUpper()) : true)
            || (!string.IsNullOrEmpty(ara) ? x.IpAdres.Contains(ara.ToUpper()) : true)
            );
            var qt = viewData;
            if (aramaType == "UzatmaTarihi")
            {
                viewData = viewData.OrderByDescending(x => x.UzatmaTarihi);
            }
            else if (aramaType == "Alfabetik")
            {
                viewData = viewData.OrderBy(x => x.DomainAdi);
            }else if (aramaType == "Hosting")
            {
                viewData = viewData.OrderBy(x => x.Hosting.HostingAdi);
            }else if(aramaType=="Ip")
            {
                viewData = viewData.OrderByDescending(x => x.IpAdres);
            }else
            {
                viewData = viewData.OrderByDescending(x => x.DomainID);
            }
          
           viewData = viewData.Skip(domainBaslangic).Take(PagerCount);

            int totalCount = qt.Count() ;
            ViewBag.Firmalar = Db.Firmas.GetirFirmalar("");
            ViewBag.DomainKategorileri = Db.DomainKategoris.GetirDomainKategorileri();
            
            PaginatedList pager = new PaginatedList((page ?? 1), PagerCount, totalCount);

            ViewData["queryData"] = ara;
            ViewData["aramaType"] = aramaType;
            ViewBag.Sayfalama = pager;
            return View(viewData);

        }
        public ActionResult Duzenle(int?id)
        {
            Domain d = new Domain();
       
            var viewData =  Db.Domains.FirstOrDefault(a => a.DomainID == id);
            ViewBag.DomainKayitliFirma = Db.DomainKayitliFirmas.OrderBy(x=>x.DomainKayitliFirmaAdi);
            ViewBag.HostingDetay = Db.Hostings.GetirHosting();
            ViewBag.DomainKategorileri = Db.DomainKategoris.GetirDomainKategorileri();
            if (Request.UrlReferrer != null)
            {
                //var ary = Request.UrlReferrer.ToString().Split('/');
                ViewBag.oncekiSayfa = Request.UrlReferrer.ToString();
                //if (ary.Length >= 4)
                //{
                //    ViewBag.oncekiSayfa = Request.UrlReferrer.ToString().Split('/')[4];
                //}
            }
            
            return View(viewData);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Duzenle(Domain domain,int id)
        {
            if (ModelState.IsValid)
            {
                domain.DomainAdi = Temizle(domain.DomainAdi);
                Domain entity = Db.Domains.SingleOrDefault(x=>x.DomainID==domain.DomainID);

                entity.DomainAdi = domain.DomainAdi;
                entity.Tarih = domain.Tarih;
                entity.UzatmaTarihi = domain.UzatmaTarihi;
                entity.RefDomainKayitliFirmaId = domain.RefDomainKayitliFirmaId;
                entity.RefHostingID = domain.RefHostingID;
                entity.RefDomainFirmaID = domain.RefDomainFirmaID;
                entity.IpAdres = domain.IpAdres;
                entity.RefDomainKategori = domain.RefDomainKategori;
                entity.DomainDurum = domain.DomainDurum;
                entity.Kontrol = domain.Kontrol;
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
                    return RedirectToAction("Domainler");
                }
               
            }
            else
            {
                var viewData = Db.Domains.FirstOrDefault(a => a.DomainID == id);
                ViewBag.DomainKayitliFirma = Db.DomainKayitliFirmas.OrderBy(x => x.DomainKayitliFirmaAdi);
                ViewBag.HostingDetay = Db.Hostings.GetirHosting();
                ViewBag.DomainKategorileri = Db.DomainKategoris.GetirDomainKategorileri();
                TempData["Error"] = "Domain daha önce eklenmiş!";
                return View();
            }
        }
        public ActionResult DomainSorgula(string domain)
        {
            List<string> info = WhoisController.Whois.lookup(domain, WhoisController.Whois.RecordType.domain, "whois.internic.net");

            ViewBag.Mesaj = info;
            return View();
            
        }
        //Eski yeri MailSablonlariController
        public ActionResult DomainUzatmaMailiGonder(int domainID, string ucret)
        {
            JsonCevap jsn = new JsonCevap();
            try
            {


                Domain domainduzenle = Db.Domains.GetirDomain(domainID);

                string mesaj = Db.MailSablonus.Find(1).MailSablonu1;

                mesaj = mesaj.Replace("{FirmaAdi}", domainduzenle.Firma.FirmaAdi);
                mesaj = mesaj.Replace("{MusteriAdiSoyadi}", domainduzenle.Firma.YetkiliAdi + " " + domainduzenle.Firma.YetkiliSoyAdi);
                mesaj = mesaj.Replace("{AlanAdi}", domainduzenle.DomainAdi);
                mesaj = mesaj.Replace("{UzatmaTarihi}", domainduzenle.UzatmaTarihi.ToString());
                mesaj = mesaj.Replace("{FirmaAdi}", domainduzenle.Firma.FirmaAdi);
                mesaj = mesaj.Replace("{MusteriNo}", domainduzenle.Firma.FirmaID.ToString());
                mesaj = mesaj.Replace("{Ucret}", ucret);

                Fonksiyonlar.MailGonder("info@karayeltasarim.com,satis@karayeltasarim.com", domainduzenle.DomainAdi + " Domain Süresi Dolum Bildirimi", mesaj);
                jsn.Basarilimi = true;
            }
            catch
            {
                jsn.Basarilimi = false;
                
            }
            return Json(jsn,JsonRequestBehavior.AllowGet);
        }

        public ActionResult DomainUzat(int domainID)
        {
            Domain domainduzenle = Db.Domains.GetirDomain(domainID);

            DateTime yeniTarih = new DateTime(domainduzenle.UzatmaTarihi.AddYears(1).Year, domainduzenle.UzatmaTarihi.Month, domainduzenle.UzatmaTarihi.Day);

            domainduzenle.UzatmaTarihi = yeniTarih;
            Db.SaveChanges();

            TempData[SUCESS] = domainduzenle.DomainAdi + " Domain Uzatıldı !!!";


            return Redirect(Request.UrlReferrer.AbsoluteUri); // Nereden gelindiyse oraya geri gönder

        }
        //Eski yeri WebController,eski adi UzatmaSuresiGelenler Uzatılacak Domainler
        public ActionResult UzatilacakDomainler()
        {
            var viewData = Partial.GetirUzatmasiGelenler();

            return View(viewData);
        }
        public ActionResult SilinenDomainler(int? page,string domain)
        {
          
            return SilinenDomainler(page,domain,null);
        }
        //Ara
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SilinenDomainler(int?page,string domain,FormCollection frm)
        {
            int domainBaslangic = ((page ?? 1) - 1) * PagerCount;
            var viewData = (from q in Db.Domains
                            where string.IsNullOrEmpty(domain) ? true : q.DomainAdi.Contains(domain)
                            orderby q.SilmeTarihi descending
                            select q).Skip(domainBaslangic).Take(PagerCount);

            int totalCount = Db.Domains.Where(dp => dp.DomainAdi != null ? (dp.DomainAdi.Contains(domain)) : (1 == 1) && dp.DomainDurum == false)
                             .Count();
            ViewBag.Firmalar = Db.Firmas.GetirFirmalar("");
            ViewBag.DomainKategorileri = Db.DomainKategoris.GetirDomainKategorileri();

            PaginatedList pager = new PaginatedList((page ?? 1), PagerCount, totalCount);
            //sayfalama
            ViewData["queryData"] = domain;
            ViewBag.Sayfalama = pager;
            return View(viewData);
        }
        // Eski yeri WebController
        public ActionResult DomainSil(int id)
        {
            Domain domainduzenle = Db.Domains.Find(id);

            domainduzenle.SilmeTarihi = DateTime.Now;
            domainduzenle.DomainDurum = false;
            Db.SaveChanges();

            TempData[SUCESS] = domainduzenle.DomainAdi + " Domain Silindi !!!";


            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }
        #region domain kategori
        public ActionResult DomainKategorileri()
        {           
            var viewData = Db.DomainKategoris.GetirDomainKategorileri();

            return View(viewData);
        }
        [HttpPost]
        public ActionResult DomainKategorileri(string kategoriAdi)
        {
            kategoriAdi = Temizle(kategoriAdi);

            if (!String.IsNullOrEmpty(kategoriAdi))
            {
                if (Db.DomainKategoris.DomainKategoriEklimi(kategoriAdi))
                {
                    TempData[ERROR] = "Domain Kategorisi Daha Önce Eklenmiş!! ";
                }
                else
                {
                    DomainKategori DomainKategoriekle = new DomainKategori()
                    {
                        DomainKategoriAdi = kategoriAdi
                    };
                    Db.DomainKategoris.Add(DomainKategoriekle);
                    Db.SaveChanges();

                    TempData[SUCESS] = "Domain Kategori Eklendi!! ";
                }
            }
            else
            {
                TempData[ERROR] = "Domain Kategori İsmini Yazmadınız!! ";
            }



            return RedirectToAction("DomainKategorileri");
        }
        public ActionResult DomainKategoriSil(int id)
        {


            if (Db.Domains.KategorideDomainVarmi(id))
            {
                TempData[ERROR] = "Önce kategoriye ekli olan domainleri silin.";
            }
            else
            {
                DomainKategori domanainKategoriDetay = Db.DomainKategoris.GetirDomainKategori(id);
                Db.DomainKategoris.Remove(domanainKategoriDetay);
                Db.SaveChanges();
                TempData[SUCESS] = "Kategori silindi.";

            }

            return RedirectToAction("DomainKategorileri");
        }
        #endregion domain kategori
        #region Domain Notlari
        public ActionResult DomainNotlari()
        {            
            return View();
        }
        public JsonResult DomainNoteAra(string domainAdi,string basTarih, string bitisTarih)
        {
            DateTime tBas = DateTime.Parse(basTarih);
            DateTime tBit = DateTime.Parse(bitisTarih).AddHours(23).AddMinutes(59);
            var list = (from dn in Db.DomainNots
                        join d in Db.Domains on dn.RefDomainId equals d.DomainID
                        join f in Db.Firmas on d.RefDomainFirmaID equals f.FirmaID
                        join u in Db.AspNetUsers on dn.RefNotKullaniciId equals u.Id
                        orderby dn.DomainNotTarih descending
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
                      );
            list = list.Where(x => (!string.IsNullOrEmpty(domainAdi) ? x.DomainAdi.Contains(domainAdi) :true)
                && x.Tarih>=tBas && x.Tarih<=tBit);
            return Json(list,JsonRequestBehavior.AllowGet);
        }
        public ActionResult DomainNoteEkle(int?id)
        {
            DomainNot model = new DomainNot();
            if (id.HasValue)
            {
                model = Db.DomainNots.Include(x=>x.Domain).SingleOrDefault(x=>x.DomainNotId==id);
            }
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult DomainNoteEkle(DomainNot model, FormCollection frm)
        {
            bool isAjax = Request["isAjax"] != null;
            if (!isAjax)
            {
                DomainNot entity = new DomainNot();
                if (model.DomainNotId > 0)
                {
                    entity = Db.DomainNots.SingleOrDefault(x => x.DomainNotId == model.DomainNotId);
                }
                entity.DomainNotNot = model.DomainNotNot;
                entity.DomainNotTarih = DateTime.Now;
                entity.RefDomainId = model.RefDomainId;
                if (model.DomainNotId > 0)
                {
                    Db.Entry(entity).State = EntityState.Modified;
                }
                else
                {
                    Db.DomainNots.Add(entity);
                }
                //todo: login ekranı eklenince kaldırılacak
                entity.RefNotKullaniciId = User.Identity.GetUserId() ?? "f5f53da2-c311-44c9-af6a-b15ca29aee57";
                Db.SaveChanges();
                TempData[SUCESS] = "Domain Kaydedildi";
                return RedirectToAction("DomainNotlari");
            }else //if (Request.HttpMethod == "GET") 
            {
                JsonCevap jsn = new JsonCevap();
                jsn.Basarilimi = true;
                DomainNot entity = new DomainNot();

                entity.DomainNotNot = model.DomainNotNot;
                entity.DomainNotTarih = DateTime.Now;
                entity.RefDomainId =model.RefDomainId;
                Db.DomainNots.Add(entity);
               
                //todo: login ekranı eklenince kaldırılacak
                entity.RefNotKullaniciId = User.Identity.GetUserId() ?? "f5f53da2-c311-44c9-af6a-b15ca29aee57";
                Db.SaveChanges();

                return Json(jsn, JsonRequestBehavior.AllowGet);
            }            
        }
        #endregion
        public JsonResult ColumnNoteEkle(int pk,string value)
        {
            JsonCevap jsn = new JsonCevap();
            Domain domain = Db.Domains.SingleOrDefault(x => x.DomainID == pk);
            domain.Note = value;
            Db.SaveChanges();
            return Json(jsn, JsonRequestBehavior.AllowGet);
        }        
        private void SetViewBagEkle()
        {
            ViewBag.DomainKayitliFirmalar = Db.DomainKayitliFirmas.OrderBy(x => x.DomainKayitliFirmaAdi);
            ViewBag.Hostingler = Db.Hostings.OrderBy(x => x.HostingAdi);
            ViewBag.DomainKategorileri = Db.DomainKategoris.OrderBy(x => x.DomainKategoriAdi);
        }
        public static string Temizle(string Kelime)
        {
            if (!String.IsNullOrEmpty(Kelime))
            {
                while (Kelime.Contains("  "))
                    Kelime = Kelime.Replace("  ", " ");
                Kelime = Kelime.Trim();
            }
            return Kelime;
        }
    }
}