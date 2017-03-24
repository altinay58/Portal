using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class IsPlaniController : BaseController
    {
        // GET: IsPlani
        public IsPlaniController()
        {
            GuncelMenu = "Satis Bolumu";
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}