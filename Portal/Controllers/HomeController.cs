using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;
using Microsoft.AspNet.Identity;
using Portal.Models.IslerModels;
using System.Data.Entity.SqlServer;

namespace Portal.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult Index()
        {
          
            return View();
        }
        public JsonResult ListIsAra(int? page, string basTarih, string bitisTarih, string isAdi,
            string firma,string domain,string isiKontrolEden,string isiYapacakKisi,string isinDurumu,int? isId,int? bolgeId)
        {
            int baslangic = ((page??1) - 1) * PagerCount;
            JsonCevap jsn = new JsonCevap();
            var userId = User.Identity.GetUserId();
            var guncelKullanici = Db.AspNetUsers.SingleOrDefault(x => x.Id == userId);
            var query = Db.IslerListesis.Where(x => (!string.IsNullOrEmpty(isAdi) ? x.IsAdi.Contains(isAdi) : true));
            query = query.Where(x => 
                    (!string.IsNullOrEmpty(firma) ? x.Firma.Contains(firma) : true)
                 && (!string.IsNullOrEmpty(domain) ? x.Domain.Contains(domain) : true)
                 && (!string.IsNullOrEmpty(isinDurumu) ? x.IsinDurumu.Contains(isinDurumu) : true)
                 && (isId.HasValue ? SqlFunctions.StringConvert((double)x.Id).Contains(isId.ToString()) : true) 
                 );
          
            //if (!User.IsInRole("Muhasebe"))
            //{
            //    string adSoyad = guncelKullanici.Isim + " " + guncelKullanici.SoyIsim;
            //    query = query.Where(x => x.IsiVerenKisi.Contains(adSoyad) || x.IsiYapacakKisi.Contains(adSoyad));              
            //}
            //else
            //{
            //    query = query.Where(x => (!string.IsNullOrEmpty(isiKontrolEden) ? x.IsiVerenKisi.Contains(isiKontrolEden) : true)
            //     && (!string.IsNullOrEmpty(isiYapacakKisi) ? x.IsiYapacakKisi.Contains(isiYapacakKisi) : true));
            //}
            query = query.Where(x => (!string.IsNullOrEmpty(isiKontrolEden) ? x.IsiVerenKisi.Contains(isiKontrolEden) : true)
               && (!string.IsNullOrEmpty(isiYapacakKisi) ? x.IsiYapacakKisi.Contains(isiYapacakKisi) : true)
               && (bolgeId.HasValue ? x.KonumId==bolgeId : true)
               );
            if (!string.IsNullOrEmpty(basTarih) && !string.IsNullOrEmpty(bitisTarih))
            {
                DateTime tBas = DateTime.Parse(basTarih);
                DateTime tBit = DateTime.Parse(bitisTarih).AddHours(23).AddMinutes(59);
                jsn.ToplamSayi = query.Where(x => x.Tarih >= tBas && x.Tarih <= tBit).Count();
                query = query.Where(x => x.Tarih >= tBas && x.Tarih <= tBit).OrderByDescending(x => x.Tarih).Skip(baslangic).Take(PagerCount);
                jsn.Data = query.ToList();
            }
            else
            {
                jsn.ToplamSayi = query.Count();
                query = query.OrderByDescending(x => x.Tarih).Skip(baslangic).Take(20);
                jsn.Data = query.ToList();
            }
            return Json(jsn, JsonRequestBehavior.AllowGet);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        #region todo
        public JsonResult TodoList(int?page,int durum)
        {
            JsonCevap jsn = new JsonCevap();
            int baslangis = ((page ?? 1) - 1) * PagerCount;
            var userid = User.Identity.GetUserId();
            var list= Db.ToDoes.Where(x => x.KulId == userid && x.Durum==durum).OrderByDescending(x => x.Tarih);
            jsn.ToplamSayi = list.Count();
            jsn.Data = list.Skip(baslangis).Take(PagerCount).ToList();
            return Json(jsn, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EkleTodo(string aciklama)
        {
            
            JsonCevap jsn = new JsonCevap();
            if (!string.IsNullOrEmpty(aciklama)){
                ToDo ent = new ToDo();
                ent.Aciklama = aciklama;
                ent.Durum = 0;
                ent.KulId = User.Identity.GetUserId();
                ent.Tarih = DateTime.Now;
                Db.ToDoes.Add(ent);
                Db.SaveChanges();
                jsn.Basarilimi = true;
                jsn.Data = ent;
            }
            else
            {
                jsn.Basarilimi = false;
            }
        
            return Json(jsn, JsonRequestBehavior.AllowGet);
        }
        public JsonResult TodoDurumDegistir(int id,int yeniDurum)
        {
            JsonCevap jsn = new JsonCevap();
            ToDo ent = Db.ToDoes.SingleOrDefault(x => x.Id == id);          
            ent.Durum = yeniDurum;
            Db.SaveChanges();
            jsn.Basarilimi = true;
            return Json(jsn, JsonRequestBehavior.AllowGet);
        }
        public  JsonResult YapilacaklarSayisi()
        {
            var userId = User.Identity.GetUserId();
            int count = Database.DbHelper.GetDb.ToDoes.Where(x => x.KulId == userId && x.Durum == (int)TodoDurum.Beklemede).Count();
            return Json(count,JsonRequestBehavior.AllowGet);
        }
        #endregion todo
        public JsonResult FirmaKisiler(int firmaId)
        {
            var kisiler = Db.FirmaKisis.AsNoTracking().Where(x => x.FirmaId == firmaId).ToList()
                .Select(x=>new { Id=x.Id,AdSoyad=$"{x.Ad} {x.Soyad}"});
            return Json(kisiler, JsonRequestBehavior.AllowGet);
        }
    }
}