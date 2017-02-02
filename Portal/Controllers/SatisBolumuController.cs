using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;
using Microsoft.AspNet.Identity;

namespace Portal.Controllers
{
    public class SatisBolumuController : BaseController
    {
        // GET: SatisBolumu
        public ActionResult Index()
        {
            return View();
        }
        #region Notlar
        public ActionResult Notlar()
        {
            return View(Db.SatisNotus.OrderByDescending(x=>x.SatisNotlariTarih).ToList());
        }
        public ActionResult NotKaydet(int?id)
        {
            SatisNotu model = new SatisNotu();
            if (id.HasValue)
            {
                model = Db.SatisNotus.SingleOrDefault(x=>x.SatisNotlariID==id);
            }
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult NotKaydet(SatisNotu model)
        {
            SatisNotu entity = new SatisNotu();
            if (model.SatisNotlariID>0)
            {
                entity = Db.SatisNotus.SingleOrDefault(x => x.SatisNotlariID == model.SatisNotlariID);
            }
            entity.SatisNotu1 = model.SatisNotu1;
            entity.SatisNotlariTarih = DateTime.Now;
            //todo: DEFAULT_USER_ID login ekranından sonra kaldırılacak
            entity.RefUserID = User.Identity.GetUserId()??DEFAULT_USER_ID;
            if (model.SatisNotlariID ==0)
            {
                Db.SatisNotus.Add(entity);
            }
            Db.SaveChanges();
            TempData[SUCESS] = "Kaydedildi";
            return RedirectToAction("Notlar") ;
        }
        public ActionResult Sil(int id)
        {
            var satisSoru = Db.SatisNotus.SingleOrDefault(x=>x.SatisNotlariID==id);
            Db.SatisNotus.Remove(satisSoru);
            TempData[SUCESS] = "Kayıt Silindi";
            Db.SaveChanges();
            return RedirectToAction("Notlar");
        }
        #endregion Notlar
    }
}