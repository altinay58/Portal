using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class Cevap
    {
        public bool Durum { get; set; }
        public bool Durum2 { get; set; }
        public List<string> Mesaj { get; set; }
        public string Html { get; set; }
        public string Json { get; set; }

        public int Sayi1 { get; set; }
        public int Sayi2 { get; set; }
        public int Sayi3 { get; set; }

        public bool SoruMu { get; set; }
        public int SoruNo { get; set; }

        public bool Yonlendir { get; set; }

        private string adres { get; set; }
        public string Adres
        {
            get { return this.adres; }
            set { this.adres = (value + "").Replace("~", ""); }
        }

        public bool MesajVarMi
        {
            get
            {
                return (this.Mesaj != null && this.Mesaj.Count > 0);
            }
        }
        public string HtmlMesaj
        {
            get
            {
                string s = "";
                if (this.MesajVarMi)
                    foreach (var m in this.Mesaj)
                        s += m + "<br/>";

                return s;
            }
        }

        public object Veri { get; set; }
        public bool VeriVarMi { get { return this.Veri != null ? true : false; } }

        public Cevap()
        {
            this.Durum = false;
            this.Mesaj = new List<string>();

            this.SoruMu = true;
            this.SoruNo = 1;
        }
        public Cevap(bool durum)
        {
            this.Durum = durum;
            this.Mesaj = new List<string>();

            this.SoruMu = true;
            this.SoruNo = 1;
        }
    }

}