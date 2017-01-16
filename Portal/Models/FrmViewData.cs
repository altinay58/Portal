using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class FrmViewData
    {
        public Arayanlar Arayanlar { get; set; }
        public isler Isler { get; set; }
        public FrmViewData WithArayanlar(Arayanlar entity)
        {
            this.Arayanlar = entity;
            return this;
        }
        public FrmViewData WithIsler(isler entity)
        {
            this.Isler = entity;
            return this;
        }
    }
    public class FrmView
    {
        public static FrmViewData Data { get { return new FrmViewData(); } }
    }
}