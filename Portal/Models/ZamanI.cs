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
    
    public partial class ZamanI
    {
        public int ZamanIsId { get; set; }
        public int RefIsId { get; set; }
        public System.DateTime ZamanIsBasTarih { get; set; }
        public Nullable<long> GecenZamanSaniye { get; set; }
    
        public virtual isler isler { get; set; }
    }
}
