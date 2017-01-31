using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models.IslerModels
{
    public class DomainBilgi
    {
        public int Id { get; set; }
        public bool SatisOncelikli { get; set; }
        public bool GuncellemeSozlesmesiVarmi { get; set; }
        public bool OdemesiAlindi { get; set; }
        public int  DomainAksiyon { get; set; }
    }
}