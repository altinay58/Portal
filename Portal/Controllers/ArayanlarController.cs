using Portal.Models;
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
            Arayanlar arayanlar = new Arayanlar();
            isler isler = new isler();
            ViewBag.arayanGrupID = new SelectList(Db.ArayanGrups, "arayanGrupID", "arayanGrupAdi");
            ViewBag.arayanRefKonumID = new SelectList(Db.Konums, "KonumID", "Konum1");
            ViewBag.arayanDomainKategoriID = new SelectList(Db.DomainKategoris, "DomainKategoriID", "DomainKategoriAdi");
            return View(FrmView.Data.WithArayanlar(arayanlar).WithIsler(isler));
        }
    }
}