using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class JsonCevap
    {
        public bool Basarilimi { get; set; }
        public object Data { get; set; }
        public int ToplamSayi { get; set; }
    }
}