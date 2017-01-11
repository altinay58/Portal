using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;
namespace Portal.Controllers
{
    public class DomainController : BaseController
    {
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
                TempData["Error"] = "Domain daha önce eklenmiş!";
                return View(domain);
            }

            if (ModelState.IsValid)
            {
                domain.DomainDurum = true;
                domain.UzatmaTarihi = DateTime.Now.AddYears(-1);
                Database.Db.Domains.Add(domain);
                Database.Db.SaveChanges();
                SetViewBagEkle();
                TempData["Success"] = "Domain Kaydedildi";
                return View(domain);
            }
            
            return View();
        }
        private void SetViewBagEkle()
        {
            ViewBag.DomainKayitliFirmalar = Database.Db.DomainKayitliFirmas.OrderBy(x => x.DomainKayitliFirmaAdi);
            ViewBag.Hostingler = Database.Db.Hostings.OrderBy(x => x.HostingAdi);
            ViewBag.DomainKategorileri = Database.Db.DomainKategoris.OrderBy(x => x.DomainKategoriAdi);
        }
    }
}