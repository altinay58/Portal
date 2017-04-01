using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class SatisRouteView
    {
        public IEnumerable<Arayanlar> arayanlar { get; set; }
        public IEnumerable<Randevu> randevular { get; set; }
        public IEnumerable<Firma> firmalar { get; set; }
    }
}