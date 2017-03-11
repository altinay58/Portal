﻿using Microsoft.AspNet.Identity;
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
        #region mesai
        public ActionResult MesaiCizelgesi()
        {
            ViewBag.kullanicilar = Db.AspNetUsers.Where(x => x.LockoutEnabled == false).ToList();
            string userId = User.Identity.GetUserId() ?? "f5f53da2-c311-44c9-af6a-b15ca29aee57";
            ViewBag.guncelKullanici = Db.AspNetUsers.Where(x => x.Id == userId).
                                      Select(x => new Kullanici { Id = x.Id, AdSoyad = x.Isim + " " + x.SoyIsim }).FirstOrDefault();
            return View();
        }
        [JsonNetFilter]
        public JsonResult GetMesaiCizelgesi(string kullaniciId, int ay, int  yil)
        {          
            JsonCevap jsn = new JsonCevap();
            var data = Db.MesaiCizelgesis.Where(x => x.KullaniciId == kullaniciId && x.Tarih.Month == ay && x.Tarih.Year == yil).ToList();
            var list = JsonConvert.SerializeObject(data,Formatting.None,new JsonSerializerSettings(){ ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            jsn.Data = list;
            return Json(jsn, JsonRequestBehavior.AllowGet);
        }
        [JsonNetFilter]
        public JsonResult MesaiCizelgeDegistir(int? pk, string value,string ccolumn,string jsnObj)
        {
            JsonCevap jsn = new JsonCevap();
            //Sati item = Db.Satis.SingleOrDefault(x => x.SatisID == pk);
            //item.musteriSatis = value;
            //Db.SaveChanges();
            jsnObj = jsnObj.Replace("Empty", "");
            MesaiCizelgesi entity = new Models.MesaiCizelgesi();
            if (pk.HasValue)
            {
                entity = Db.MesaiCizelgesis.SingleOrDefault(x => x.Id == pk.Value);
            }
            else
            {
                var mdf = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Local
                };
              
                if (!string.IsNullOrEmpty(jsnObj))
                {
                    entity = JsonConvert.DeserializeObject<MesaiCizelgesi>(jsnObj,mdf);
                }
                if (entity.Id > 0)
                {
                    entity = Db.MesaiCizelgesis.SingleOrDefault(x => x.Id == entity.Id);
                }
                else
                {
                    Db.MesaiCizelgesis.Add(entity);
                }
               
            }
            //var propertyInfo = entity.GetType().GetProperty(ccolumn, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            //propertyInfo.SetValue(entity, value, null);
            if(ccolumn == "MesaiSuresi")
            {
                entity.MesaiSuresi = Convert.ToDecimal(value);
            }else if (ccolumn == "Durum")
            {
                entity.Durum = Convert.ToInt32(value);
            }
            else
            {
                var pr = entity.GetType().GetProperty(ccolumn);        
                object val_c = Convert.ChangeType(value, pr.PropertyType);
                pr.SetValue(entity, val_c);
              

            }
          
            //entity.GetType().GetProperty(ccolumn).SetValue(entity, value);
            Db.SaveChanges();
            jsn.Data = entity;
            return Json(jsn, JsonRequestBehavior.AllowGet);
        }
        public JsonResult HesaplaMesai(string kullanici,int yil,int ay)
        {
            var dataAylik = (from m in Db.MesaiCizelgesis
                        where m.Durum==(int)MesaiDurum.Mesai && m.MesaiSuresi!=null &&
                        m.KullaniciId==kullanici && m.Tarih.Month==ay && m.Tarih.Year==yil
                        select m
                        ).ToList().Sum(x=>Convert.ToDecimal(x.MesaiSuresi));
            var dataYillik = (from m in Db.MesaiCizelgesis
                             where m.Durum == (int)MesaiDurum.Mesai && m.MesaiSuresi!=null &&
                             m.KullaniciId == kullanici && m.Tarih.Year==yil
                              select m
                        ).ToList().Sum(x => Convert.ToDecimal(x.MesaiSuresi));
            var data = new { aylikToplam = dataAylik, yillikToplam = dataYillik };
            
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion mesai
        public ActionResult Takvim()
        {
            return View();
        }
    }
}