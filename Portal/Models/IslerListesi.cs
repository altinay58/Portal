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
    
    public partial class IslerListesi
    {
        public int Id { get; set; }
        public string IsAdi { get; set; }
        public string Firma { get; set; }
        public string Domain { get; set; }
        public string IsiVerenKisi { get; set; }
        public string IsiYapacakKisi { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
        public string IsinDurumu { get; set; }
        public Nullable<int> DomainId { get; set; }
        public string Bolge { get; set; }
        public Nullable<int> KonumId { get; set; }
    }
}
