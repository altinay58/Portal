using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;
using System.Threading.Tasks;

namespace Portal.Controllers
{
    public class DomainController : BaseController
    {
        const int  sayfada_gosterilecek_domain_sayisi = 10;
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
            if (Database.Db.Domains.DomainEklimi(domain.DomainAdi))
            {
                SetViewBagEkle();
                TempData[ERROR] = "Domain daha önce eklenmiş!";
                return View(domain);
            }

            if (ModelState.IsValid)
            {
                domain.DomainDurum = true;
                domain.UzatmaTarihi = DateTime.Now.AddYears(-1);
                Database.Db.Domains.Add(domain);
                Database.Db.SaveChanges();
                SetViewBagEkle();
                TempData[SUCESS] = "Domain Kaydedildi";
                return View(domain);
            }
            
            return View();
        }
        public ActionResult Domainler(int? page)
        {
           
            int domainBaslangic = ((page ?? 1) - 1) * PagerCount;
            var viewData = Database.Db.Domains.GetirDomainler(PagerCount, domainBaslangic);
            int totalCount = Database.Db.Domains.GetirDomainler().Count();
            ViewBag.Firmalar = Database.Db.Firmas.GetirFirmalar("");
            ViewBag.DomainKategorileri = Database.Db.DomainKategoris.GetirDomainKategorileri();
            
            PaginatedList pager = new PaginatedList((page ?? 1), PagerCount, totalCount);
            //sayfalama
            ViewBag.Sayfalama = pager;
            return View(viewData);

        }
        public ActionResult Duzenle(int?id)
        {
            Domain d = new Domain();
            //d.Kontrol
            var viewData =  Database.Db.Domains.FirstOrDefault(a => a.DomainID == id);
            ViewBag.DomainKayitliFirma = Database.Db.DomainKayitliFirmas.OrderBy(x=>x.DomainKayitliFirmaAdi);
            ViewBag.HostingDetay = Database.Db.Hostings.GetirHosting();
            ViewBag.DomainKategorileri = Database.Db.DomainKategoris.GetirDomainKategorileri();
            return View(viewData);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Duzenle(Domain domain,int id)
        {
            if (ModelState.IsValid)
            {
                domain.DomainAdi = Temizle(domain.DomainAdi);
                Domain entity = Database.Db.Domains.SingleOrDefault(x=>x.DomainID==domain.DomainID);

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
                Database.Db.SaveChanges();
                TempData[SUCESS] = "Kaydedildi";
                return RedirectToAction("Domainler");
            }
            else
            {
                var viewData = Database.Db.Domains.FirstOrDefault(a => a.DomainID == id);
                ViewBag.DomainKayitliFirma = Database.Db.DomainKayitliFirmas.OrderBy(x => x.DomainKayitliFirmaAdi);
                ViewBag.HostingDetay = Database.Db.Hostings.GetirHosting();
                ViewBag.DomainKategorileri = Database.Db.DomainKategoris.GetirDomainKategorileri();
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
        private void SetViewBagEkle()
        {
            ViewBag.DomainKayitliFirmalar = Database.Db.DomainKayitliFirmas.OrderBy(x => x.DomainKayitliFirmaAdi);
            ViewBag.Hostingler = Database.Db.Hostings.OrderBy(x => x.HostingAdi);
            ViewBag.DomainKategorileri = Database.Db.DomainKategoris.OrderBy(x => x.DomainKategoriAdi);
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