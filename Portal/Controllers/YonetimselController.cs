using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Portal.Filters;
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
        
        public JsonResult GetMesaiCizelgesi(string kullaniciId, int ay, int  yil)
        {          
            JsonCevap jsn = new JsonCevap();
            var data = Db.MesaiCizelgesis.Where(x => x.KullaniciId == kullaniciId && x.Tarih.Month == ay && x.Tarih.Year == yil).ToList();
            var list = JsonConvert.SerializeObject(data,Formatting.None,new JsonSerializerSettings(){ ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            jsn.Data = list;
            return Json(jsn, JsonRequestBehavior.AllowGet);
        }
        public JsonResult MesaiCizelgeDegistir(int? pk, string value,string ccolumn)
        {
            JsonCevap jsn = new JsonCevap();
            //Sati item = Db.Satis.SingleOrDefault(x => x.SatisID == pk);
            //item.musteriSatis = value;
            //Db.SaveChanges();
            MesaiCizelgesi entity = new Models.MesaiCizelgesi();
            if (pk.HasValue)
            {
                entity = Db.MesaiCizelgesis.SingleOrDefault(x => x.Id == pk.Value);
            }
            else
            {
                Db.MesaiCizelgesis.Add(entity);
            }
            //var propertyInfo = entity.GetType().GetProperty(ccolumn, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            //propertyInfo.SetValue(entity, value, null);
            entity.GetType().GetProperty(ccolumn).SetValue(entity, value);
            Db.SaveChanges();
            return Json(jsn, JsonRequestBehavior.AllowGet);
        }
    }
}