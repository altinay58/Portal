using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;
using Microsoft.AspNet.Identity;

namespace Portal.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult Index()
        {
            string sql = @"select d.*
 from
 (
 select  i.islerRefDomainID as DomainID,i.islerTarih,
 (select count(i2.islerRefDomainID) from isler i2 where i2.islerRefDomainID=i.islerRefDomainID) cnt,
 row_number() over (partition by i.islerRefDomainID order by i.islerTarih desc) as seqnum
 from isler i inner join Domain d on (i.islerRefDomainID=d.DomainID)

 ) r,Domain d
 where seqnum=1 and 
 d.DomainID=r.DomainID
  order by islerTarih desc";
            var list = Db.Database.SqlQuery<Domain>(sql).ToList();

            string userid = User.Identity.GetUserId();

            List<int> idler = Db.IsiYapacakKisis.Where(a => a.RefIsiYapacakKisiUserID == userid).Select(x => x.RefIsID).ToList();

            var yapacagiisler = Db.islers.Where(a => idler.Contains(a.islerID) && a.islerisinTamamlanmaDurumu != true).OrderBy(a => a.islerTarih).ToList();

            var verdigiisler = Db.islers.Where(a => a.islerisiVerenKisi == userid && a.islerisinTamamlanmaDurumu != true).OrderBy(a => a.islerSira).ToList();

            //IEnumerable<isler> yeniIslerim = Partial.KullaniciBildirim(userid).yeniIsler;
            //IEnumerable<isler> kontrolBekleyenIsler = Partial.KullaniciBildirim(userid).kontrolBekleyenIsler;
            //IEnumerable<isler> okunmayanIsler = Partial.CevaplananIslerOkunmayanlar(userid);
            //IEnumerable<Randevu> randevular = Partial.KullaniciBildirim(iduserid.Randevular;
            //int bildirimSayisi = Partial.KullaniciBildirim(userid).BildirimSayisi;
            return View(list);
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
        public JsonResult TodoList(int?page)
        {
            JsonCevap jsn = new JsonCevap();
            int baslangis = ((page ?? 1) - 1) * PagerCount;
            var userid = User.Identity.GetUserId();
            var list= Db.ToDoes.Where(x => x.KulId == userid ).OrderByDescending(x => x.Tarih);
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
        #endregion todo
    }
}