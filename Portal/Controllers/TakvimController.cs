using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;

namespace Portal.Controllers
{
    /// <summary>
    /// Google Calendar Api referans ekleme: Install-Package Google.Apis.Calendar.v3
    /// Api Key Oluşturma URL: https://console.developers.google.com/start/api?id=calendar
    /// </summary>
    public class TakvimController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.arayanDomainKategoriID = new SelectList(Db.DomainKategoris, "DomainKategoriID", "DomainKategoriAdi");
            ViewBag.Kullanicilar = (
                                        from u in Db.AspNetUsers
                                        join ta in Db.TakvimAyars on u.Id equals ta.RefUserId
                                        select u
                                   ).ToList();


            //TakvimApi takvimApi = new TakvimApi(User.Identity.GetUserId(), Db);


            ViewBag.Takvim = null;
            return View();
        }

        [HttpPost]
        public JsonResult Index(EtkinlikKaydetViewModel model)
        {
            Cevap c = new Cevap();

            //string mesaj = "";

            if (ModelState.IsValid)
            {
                bool tumGunMu = model.tumGunMu ?? false;
                DateTime basTar = Convert.ToDateTime(model.takvimBasTar + " " + model.takvimBasSaat);

                //tüm gün ise başlangıç gününü bitiş günü olarak ayarlıyoruz yoksa girilen bitiş tarihi ayarlıyoruz.
                DateTime bitTar = tumGunMu ? basTar : Convert.ToDateTime(model.takvimBitTar + " " + model.takvimBitSaat);
                
                //Api ile bağlantı kuruyoruz.
                TakvimApi takvimApi = new TakvimApi(model.userId, Db);
                var etkinlik = takvimApi.EtkinlikKaydet(new TakvimEtkinlik
                {
                    Id = model.etkinlikId,
                    Baslik = model.takvimBaslik,
                    Icerik = model.takvimIcerik,
                    BasTar = basTar,
                    BitTar = bitTar,
                    TumGunMu = tumGunMu,
                    
                    Katilimcilar = model.katilimcilar,
                    Lokasyon = model.lokasyon
                });

                c.Veri = JsonConvert.SerializeObject(new Etkinlik
                {
                    _id = etkinlik.Id,
                    title = etkinlik.Summary,
                    allDay = !etkinlik.Start.Date.IsNullOrEmpty(),
                    start = etkinlik.Start.DateTime ?? Convert.ToDateTime(etkinlik.Start.Date),
                    end = etkinlik.End.DateTime ?? Convert.ToDateTime(etkinlik.End.Date)
                });

                c.Durum = true;
                c.Durum2 = model.etkinlikId.IsNullOrEmpty(); //Yeni kayıt olup olmadığının bilgisi dönülüyor. true=yeni kayıt, false=düzeltilen kayıt
                c.Mesaj.Add("Etkinlik kaydedilmiştir.");
            }
            else
            {
                foreach (var modelState in ModelState.Values)
                    foreach (var modelError in modelState.Errors)
                        c.Mesaj.Add(modelError.ErrorMessage);

                //TempData[ERROR] = mesaj;
            }

            return Json(c, JsonRequestBehavior.AllowGet);
        }
         
        [HttpPost]
        public JsonResult EtkinlikGetir(string userId, string takvimBasTar,string takvimBitTar)
        {
            TakvimApi takvimApi = new TakvimApi(userId, Db);
            List<Etkinlik> list = takvimApi.GetList(Convert.ToDateTime(takvimBasTar), Convert.ToDateTime(takvimBitTar).AddDays(1));

            JsonCevap c = new JsonCevap();
            c.Basarilimi = true;
            c.Data = JsonConvert.SerializeObject(list);

            return Json(c, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EtkinlikSil(EtkinlikSilViewModel m)
        {
            #region DEĞİŞKEN TANIMLARI
            Cevap c = new Cevap();

            #endregion
            #region MODEL KONTROLÜ

            c.KontrolEt(ModelState);

            #endregion
            #region SON
            if (c.MesajVarMi)
                return Json(c, JsonRequestBehavior.AllowGet);
            #endregion
            #region İŞLEMLER
            try
            {
                TakvimApi takvimApi = new TakvimApi(m.userId, Db);
                takvimApi.EtkinlikSil(m.etkinlikId);

                c.Durum = true;
                c.Mesaj.Ekle("Etkinlik silinmiştir.");
            }
            catch(Exception ex)
            {
                c.Durum = true;
                c.Mesaj.Ekle(ex);
            }
            #endregion
            return Json(c, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EtkinlikGetirTek(string userId, string etkinlikId)
        {
            #region DEĞİŞKEN TANIMLARI
            Cevap c = new Cevap();

            #endregion
            #region MODEL KONTROLÜ

            c.KontrolEt(ModelState);

            #endregion
            #region SON
            if (c.MesajVarMi)
                return Json(c, JsonRequestBehavior.AllowGet);
            #endregion
            #region İŞLEMLER
            try
            {
                TakvimApi takvimApi = new TakvimApi(userId, Db);
                Etkinlik list = takvimApi.Get(etkinlikId);
                
                c.Durum = true;
                c.Veri = JsonConvert.SerializeObject(list);
            }
            catch (Exception ex)
            {
                c.Durum = true;
                c.Mesaj.Ekle(ex);
            }
            #endregion

            return Json(c, JsonRequestBehavior.AllowGet);
        }

    }
}