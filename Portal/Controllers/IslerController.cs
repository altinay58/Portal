using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;
using Microsoft.AspNet.Identity;

namespace Portal.Controllers
{
    public class IslerController : Controller
    {
        private PortalEntities db = new PortalEntities();
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
        public ActionResult IcerikFormu()
        {
            return View();
        }
        public ActionResult FirmalariGetir(string firmaAdi)
        {


            var firmalar = new List<object>();

            foreach (Firma firmalarim in db.Firmas.Where(a => a.FirmaAdi.Contains(firmaAdi) || a.YetkiliCepTelefon.Contains(firmaAdi) || a.YetkiliTelefon.Contains(firmaAdi)))
            {
                firmalar.Add(new { value = firmalarim.FirmaID, label = firmalarim.FirmaAdi });
            }

            var diziArayanlar = db.Arayanlars.Where(a => a.arayanKayitliMusterimi == false && a.arayanFirmaAdi.Contains(firmaAdi) || a.arayanCepTelNo.Contains(firmaAdi) || a.arayanTelefonNo.Contains(firmaAdi)).GroupBy(g => g.arayanFirmaAdi).Select(o => new { ArayanAdi = o.Key, Arayan = o.OrderBy(c => c.arayanFirmaAdi).ToList() }).ToList();

            foreach (var aramaYapan in diziArayanlar)
            {
                Arayanlar arayanFirma = db.Arayanlars.FirstOrDefault(a => a.arayanFirmaAdi == aramaYapan.ArayanAdi);
                if (arayanFirma != null)
                {
                    firmalar.Add(new { value = "kayitliDegil" + arayanFirma.arayanID, label = arayanFirma.arayanFirmaAdi + " - Kayıtlı Değil" });
                }
            }

            return Json(firmalar, JsonRequestBehavior.AllowGet);
        }
    }
}