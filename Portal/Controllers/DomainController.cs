using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            ViewBag.KayitliFirmalar = Database.Db.KayitliFirmas.OrderBy(x => x.KayitliFirmaAdi);
            ViewBag.Hostingler = Database.Db.Hostings.OrderBy(x=>x.HostingAdi);
            return View();
        }
    }
}