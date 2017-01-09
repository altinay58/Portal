using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public abstract class BaseController : Controller
    {

   
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            Database.Db = new Models.PortalEntities();
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            //Database.Db.Dispose();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Database.Db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}