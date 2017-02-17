using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
namespace Portal.Controllers
{
    public class ArgeBolumuController : BaseController
    {
        public ArgeBolumuController()
        {
            ViewBag.guncelMenu="ArgeBolumu";
        }

        // GET: ArgeBolumu
        public ActionResult SanalPost()
        {
            var sanalpos = Db.SanalPos.Include(s => s.Firma);
            return View(sanalpos.ToList());
        }
    }
}