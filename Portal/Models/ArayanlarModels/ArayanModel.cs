using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models.ArayanlarModels
{
    public class ArayanModel
    {
        //public int arayanID { get; set; }
        public string arayanAdi { get; set; }
        public string arayanSoyadi { get; set; }
        public string arayanFirmaAdi { get; set; }
        public string arayanFirmaSahibiOzelligi { get; set; }
        public string arayanTelefonNo { get; set; }
        public string arayanCepTelNo { get; set; }
        public string arayanMailAdresi { get; set; }
        public string arayanWebAdresi { get; set; }
        public string arayanSehir { get; set; }
        public string arayanilce { get; set; }
        public string arayanAdres { get; set; }
        public Nullable<int> arayanRefKonumID { get; set; }
        public Nullable<int> AramaYontemiId { get; set; }
        public string arayanKonu { get; set; }
        public string arayanNot { get; set; }
        public string arayanBegendigiWebSiteleri { get; set; }
        public Nullable<System.DateTime> arayanKayitTarih { get; set; }
        public Nullable<int> arayanDomainKategoriID { get; set; }
        public string arayanTelefonaBakanKisiID { get; set; }
        public Nullable<int> arayanSektorID { get; set; }
        public Nullable<int> kisiid { get; set; }
        public Nullable<int> arayanGrupID { get; set; }
        public Nullable<bool> arayanilkArama { get; set; }
        public Nullable<bool> arayanFirmaKayitDurum { get; set; }
        public Nullable<bool> arayanKayitliMusterimi { get; set; }
        public int arayanKayitliRefFirmaID { get; set; }
        public string ArayanAraciAdi { get; set; }
        public Nullable<int> arayanMailSablonuId { get; set; }
        //iş için
        public string islerisiYapacakKisi { get; set; }
        public string gelisYonetemi { get; set; }
        public string kontrolEdecekKisi { get; set; }
        public bool BitirmeTarihiVarmi { get; set; }
        public DateTime? bitirmeTarihi { get; set; }
        public int? domainId { get; set; }
        public string isAdi { get; set; }
        public string isAciklama { get; set; }
        //
        public bool araciVarmi { get; set; }
        public string SaatDakika { get; set; }
        public bool isEkle { get; set; }
       

        public bool? Kayitlimi { get; set; }
        public bool TanitimMailiGonder { get; set; }
        public string MailBaslik { get; set; }
       
    }
}