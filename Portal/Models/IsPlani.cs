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
    
    public partial class IsPlani
    {
        public int Id { get; set; }
        public string Aciklama { get; set; }
        public string RefSorumluKisiId { get; set; }
        public Nullable<int> EtiketIsPlaniTipi { get; set; }
        public Nullable<int> RefIsId { get; set; }
        public Nullable<int> RefFirmaId { get; set; }
        public Nullable<int> RefSatisFirsatiId { get; set; }
        public Nullable<int> EtiketIsPlaniDurum { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Firma Firma { get; set; }
        public virtual SatisFirsati SatisFirsati { get; set; }
    }
}
