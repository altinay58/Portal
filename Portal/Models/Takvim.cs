//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Portal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Takvim
    {
        public int TakvimId { get; set; }
        public string RefUserId { get; set; }
        public System.DateTime TakvimBasTar { get; set; }
        public System.DateTime TakvimBitTar { get; set; }
        public string TakvimBaslik { get; set; }
        public string TakvimIcerik { get; set; }
        public System.DateTime KayitTarihi { get; set; }
        public string RefKayitKul { get; set; }
        public Nullable<System.DateTime> DuzeltmeTarihi { get; set; }
        public string RefDuzeltmeKul { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
