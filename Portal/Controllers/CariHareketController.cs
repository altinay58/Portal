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
    }
}