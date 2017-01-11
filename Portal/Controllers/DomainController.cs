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
        const int  sayfada_gosterilecek_domain_sayisi = 50;
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
        public ActionResult Domainler(int? sayfaNo)
        {
            ViewBag.SayfaAdi = "Domainler";
            int SayfaNo = sayfaNo ?? 0;

            int domainBaslangic = 0;
            if (SayfaNo > 1)
            {
                domainBaslangic = (SayfaNo - 1) * sayfada_gosterilecek_domain_sayisi;
            }

            var viewData = Database.Db.Domains.GetirDomainler(sayfada_gosterilecek_domain_sayisi, domainBaslangic);

            //sayfalama
            if (SayfaNo == 0)
            {
                SayfaNo = 1;
            }

            string sayfalama = "";
            //sayfa numarası 0 dan büyükse yani birinci sayfada değilse ilk sayfaya link verdik.
            if (SayfaNo > 6)
            {
                sayfalama = sayfalama + " <a  class=\"page\" href=\"/domain/liste\"> İlk </a> ";
            }


            int sayi = Database.Db.Domains.GetirDomainler().Count();
            double sayfa = (double)sayi / sayfada_gosterilecek_domain_sayisi;
            sayfa = Math.Ceiling(sayfa);

            // sayfa ilk sayfa değilse sayfadan önceki 5 sayfaya link verdik 
            // sayfamız 5 ise 1-2-3-4 nolu sayfalar linklenecek sonra 5 gelecek

            for (int i = SayfaNo - 5; i < SayfaNo; i++)
            {
                if (i == 1)
                {
                    sayfalama = sayfalama + " <a  class=\"page\" href=\"/domain/liste/\"> 1 </a> ";
                }
                else if (i > 0)
                {
                    sayfalama = sayfalama + " <a  class=\"page\" href=\"/domain/liste/" + i + "\">" + i + "</a> ";
                }
            }

            if (SayfaNo > 0)
            {
                sayfalama = sayfalama + " " + SayfaNo + " ";
            }
            // sayfadan sonraki 5 sayfa
            for (int i = SayfaNo + 1; i < SayfaNo + 6; i++)
            {
                if (i <= sayfa)
                {
                    sayfalama = sayfalama + " <a  class=\"page\" href=\"/domain/liste/" + i + "\">" + i + "</a> ";
                }
            }

            //sayfa numarası 0 dan büyükse yani birinci sayfada değilse ilk sayfaya link verdik.
            if (SayfaNo < sayfa - 5)
            {
                sayfalama = sayfalama + " <a  class=\"page\" href=\"/domain/liste/" + (sayfa).ToString() + "\"> Son </a> ";
            }

            ViewBag.Firmalar = Database.Db.Firmas.GetirFirmalar("");
            ViewBag.DomainKategorileri = Database.Db.DomainKategoris.GetirDomainKategorileri();
            ViewBag.Sayfalama = sayfalama;
            //sayfalama

            return View(viewData);

        }
        private void SetViewBagEkle()
        {
            ViewBag.DomainKayitliFirmalar = Database.Db.DomainKayitliFirmas.OrderBy(x => x.DomainKayitliFirmaAdi);
            ViewBag.Hostingler = Database.Db.Hostings.OrderBy(x => x.HostingAdi);
            ViewBag.DomainKategorileri = Database.Db.DomainKategoris.OrderBy(x => x.DomainKategoriAdi);
        }
    }
}