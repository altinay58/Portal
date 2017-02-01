using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;
namespace Portal.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            string sql = @" select distinct d.* from Domain d inner join isler i on(d.DomainID=i.islerRefDomainID)
 where DomainID in
 (
 select top 10 DomainID from
 (
 select  top 100  islerRefDomainID as DomainID
 ,row_number() over (partition by islerRefDomainID order by islerTarih desc) as seqnum
 from isler
 order by islerTarih desc 
 )t
 where t.seqnum=1
 ) ";
            var list = Db.Database.SqlQuery<Domain>(sql).ToList();
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
    }
}