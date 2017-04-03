using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class SatisDashboardController : BaseController
    {
        public SatisDashboardController()
        {
            GuncelMenu = "Satis Bolumu";
        }
        // GET: SatisDashboard
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}