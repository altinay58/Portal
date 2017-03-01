using Microsoft.AspNet.Identity;
using Portal.Models;
using Portal.Models.IslerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class YonetimselController : BaseController
    {
        public YonetimselController()
        {
            GuncelMenu = "Yonetimsel";
        }
        [Authorize(Roles ="Yonetim")]
        public ActionResult WebLog(int?p)
        {
            int baslangic = (p ?? 1 - 1) * PagerCount;
            var data = Db.WebLogs.OrderByDescending(x => x.WebLogID).Skip(baslangic).Take(PagerCount).ToList();
            int totalCount = Db.WebLogs.Count();
            PaginatedList pager = new PaginatedList((p ?? 1), PagerCount, totalCount);

            //ViewData["queryData"] = "";
            ViewBag.Sayfalama = pager;
            return View(data);
        }
        public ActionResult MesaiCizelgesi()
        {
            ViewBag.kullanicilar = Db.AspNetUsers.Where(x => x.LockoutEnabled == false).ToList();
            string userId = User.Identity.GetUserId() ?? "f5f53da2-c311-44c9-af6a-b15ca29aee57";
            ViewBag.guncelKullanici = Db.AspNetUsers.Where(x => x.Id == userId).
                                      Select(x => new Kullanici { Id = x.Id, AdSoyad = x.Isim + " " + x.SoyIsim }).FirstOrDefault();
            return View();
        }
    }
}