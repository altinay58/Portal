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
    
    public partial class SatisNotu
    {
        public int SatisNotlariID { get; set; }
        public string RefUserID { get; set; }
        public string SatisNotu1 { get; set; }
        public System.DateTime SatisNotlariTarih { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
