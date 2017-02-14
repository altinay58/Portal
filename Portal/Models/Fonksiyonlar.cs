﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Portal.Models;
using System.Collections;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Mail;

namespace Portal.Models
{
    public partial class Fonksiyonlar
    {
        public static int DomainUzatmaSuresineKalanGun(DateTime uzatmaTarihi)
        {

            TimeSpan fark = uzatmaTarihi - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            return Convert.ToInt32(fark.TotalDays);

        }
        public static string MailGonder(string gidecekMailAdresleri, string baslik, string mesaj)
        {

            // KarayelEntities db = new KarayelEntities();

            //db.Database.ExecuteSqlCommand("insert into Mail.dbo.MailGonder values ('info@antalyawebtasarim.com','Awt@1234',1,'smtp.live.com',587,'" + gidecekMailAdresleri + "','" + baslik + "','" + mesaj + "')");

            string MailAdresi = "satis@karayeltasarim.com";

            string MailSifresi = "Kwt112107";
            bool MailSsl = true;
            string SMTPAdresi = "smtp.live.com";
            string Port = "587";
            string[] MailAdresleri = gidecekMailAdresleri.Split(',');

            string ilkMailAdresi = MailAdresleri[0];

            if (String.IsNullOrEmpty(ilkMailAdresi))
            {
                ilkMailAdresi = MailAdresi;
            }

            MailMessage message = new MailMessage();
            message.From = new MailAddress(MailAdresi);
            try
            {

                foreach (string mailim in MailAdresleri)
                {
                    if (String.IsNullOrEmpty(mailim))
                    {
                        continue;
                    }
                    message.To.Add(mailim); // <-- this one
                }

                message.Subject = baslik;
                //message.CC.Add("info@antalyacarhire.com");
                message.Body = mesaj;

                message.IsBodyHtml = true;

                SmtpClient client = new SmtpClient(SMTPAdresi, Convert.ToInt32(Port));
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(MailAdresi, MailSifresi);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = MailSsl;

                client.Send(message);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            if (message != null)
            {
                message.Dispose();
            }
            return "";
        }
        public static string KarakterDuzenle(string metin)
        {
            string Duzenlenmis = metin;

            if (String.IsNullOrEmpty(Duzenlenmis))
                return "";

            Duzenlenmis = Duzenlenmis.Replace("&#304;", "I");
            Duzenlenmis = Duzenlenmis.Replace("&#305;", "i");
            Duzenlenmis = Duzenlenmis.Replace("&#214;", "Ö");
            Duzenlenmis = Duzenlenmis.Replace("&#246;", "ö");
            Duzenlenmis = Duzenlenmis.Replace("&Ouml;", "Ö");
            Duzenlenmis = Duzenlenmis.Replace("&ouml;", "ö");
            Duzenlenmis = Duzenlenmis.Replace("&#220;", "Ü");
            Duzenlenmis = Duzenlenmis.Replace("&#252;", "ü");
            Duzenlenmis = Duzenlenmis.Replace("&Uuml;", "Ü");
            Duzenlenmis = Duzenlenmis.Replace("&uuml;", "ü");
            Duzenlenmis = Duzenlenmis.Replace("&#199;", "Ç");
            Duzenlenmis = Duzenlenmis.Replace("&#231;", "ç");
            Duzenlenmis = Duzenlenmis.Replace("&Ccedil;", "Ç");
            Duzenlenmis = Duzenlenmis.Replace("&ccedil;", "ç");
            Duzenlenmis = Duzenlenmis.Replace("&#286;", "G");
            Duzenlenmis = Duzenlenmis.Replace("&#287;", "g");
            Duzenlenmis = Duzenlenmis.Replace("&#350;", "S");
            Duzenlenmis = Duzenlenmis.Replace("&#351;", "s");
            Duzenlenmis = Duzenlenmis.Replace("&hellip;", "...");
            Duzenlenmis = Duzenlenmis.Replace("&rsquo;", "'");
            Duzenlenmis = Duzenlenmis.Replace("&ldquo;", "“");
            Duzenlenmis = Duzenlenmis.Replace("&rdquo;", "”");
            Duzenlenmis = Duzenlenmis.Replace("&#39;", "'");


            return Duzenlenmis;
        }
        public static int FirmaBolgeIDGetir(int FirmaID)
        {
            int? firmaBolgeID;
            using (var dbc = new PortalEntities())
            {
                firmaBolgeID = dbc.Firmas.Find(FirmaID).RefKonumID;

            }

            return firmaBolgeID ?? 0;
        }

        public static int KonumIDGetir(int arayanID)
        {
            int? arayanRefKonumID;
            using (var dbc = new PortalEntities())
            {
                arayanRefKonumID = dbc.Arayanlars.Find(arayanID).arayanRefKonumID;

            }

            return arayanRefKonumID ?? 0;
        }
        public static MvcHtmlString Sayfala(string link, int bulunduguSayfa, int ogeAdedi, int sayfadaGosterilecekOgesayisi, bool lilimi, string liCSS, string aCSS, string activeCSS,
       bool limiAktif, bool tumSayfalariGoster, int sayfadanOnceKacSayfaGosterilsin)
        {
            string sayfalama = "";
            string liBaslangic = "";
            string liBitis = "";
            string aktifLi = "";

            // Sayfasayısını buluyoruz Eğer sayfa sayısında kalan olursa yukarı yuvarlayarak artan elemanların son
            // sayfada çıkmasını sağlıyoruz

            double toplamSayfa = (double)ogeAdedi / sayfadaGosterilecekOgesayisi;
            toplamSayfa = Math.Ceiling(toplamSayfa);


            // Sayfalama sistemi li li ise lileri ekliyoruz
            if (lilimi)
            {
                liBaslangic = "<li class=\"" + liCSS + "\">";
                liBitis = "</li>";
                aktifLi = "<li class=\"" + liCSS + " " + activeCSS + "\">";
            }


            // ilk sayfa kodu
            sayfalama = sayfalama + liBaslangic + "<a class=\"" + aCSS + "\" href=\"" + link + "\">İlk</a>" + liBitis;

            // Bulunduğu sayfa 1 den büyükde önceki sayfa linkini ekliyoruz
            if (bulunduguSayfa > 1)
            {
                sayfalama = sayfalama + liBaslangic + "<a class=\"" + aCSS + "\" href=\"" + link + "/" + (bulunduguSayfa - 1).ToString() + "\">Önceki</a>" + liBitis;

            }

            // Tüm sayfaları göster true ise tüm sayfaların listelenmesi için aşağıdaki kodu yazdık.
            if (tumSayfalariGoster)
            {
                if (toplamSayfa > 1)
                {
                    if (bulunduguSayfa == 0)
                    {
                        sayfalama = sayfalama + aktifLi + "<a class=\"" + aCSS + " " + activeCSS + "\" href=\"" + link + "\">1</a>" + liBitis;
                    }
                    else
                    {
                        sayfalama = sayfalama + liBaslangic + "<a class=\"" + aCSS + "\" href=\"" + link + "\">" + 1 + "</a>" + liBitis;
                    }
                }


                for (int i = 2; i <= toplamSayfa; i++)
                {
                    if (bulunduguSayfa == i)
                    {
                        sayfalama = sayfalama + aktifLi + "<a class=\"" + aCSS + " " + activeCSS + "\" href=\"" + link + "/" + (bulunduguSayfa).ToString() + "\">" + bulunduguSayfa + "</a>" + liBitis;
                    }
                    else
                    {
                        sayfalama = sayfalama + liBaslangic + "<a class=\"" + aCSS + "\" href=\"" + link + "/" + (i).ToString() + "\">" + i + "</a>" + liBitis;
                    }
                }
            }
            else // Tüm sayfalar gösterilmeyecekse aşağıdaki kodu yazdık.
            {
                // Sayfamızdan verilen değerden önceki sayfaları getiriyoruz.
                for (int i = bulunduguSayfa - sayfadaGosterilecekOgesayisi; i < bulunduguSayfa; i++)
                {
                    if (i == 1)
                    {
                        sayfalama = sayfalama + liBaslangic + "<a class=\"" + aCSS + "\" href=\"" + link + "\">" + 1 + "</a>" + liBitis;
                    }
                    else if (i > 0)
                    {
                        sayfalama = sayfalama + liBaslangic + "<a class=\"" + aCSS + "\" href=\"" + link + "/" + (i).ToString() + "\">" + i + "</a>" + liBitis;
                    }
                }

                // Bulunduğu sayfa 0 dan büyükse yazdırıyoruz.
                if (bulunduguSayfa > 0)
                {
                    if (bulunduguSayfa == 1)
                    {
                        sayfalama = sayfalama + aktifLi + "<a class=\"" + aCSS + " " + activeCSS + "\" href=\"" + link + "\">" + 1 + "</a>" + liBitis;
                    }
                    else if (bulunduguSayfa > 0)
                    {
                        sayfalama = sayfalama + aktifLi + "<a class=\"" + aCSS + " " + activeCSS + "\" href=\"" + link + "/" + (bulunduguSayfa).ToString() + "\">" + bulunduguSayfa + "</a>" + liBitis;
                    }
                }

                // sayfadan sonraki belirtilen sayfasayıkı kadar sayfa ekliyoruz
                for (int i = bulunduguSayfa + 1; i < bulunduguSayfa + sayfadaGosterilecekOgesayisi + 1; i++)
                {
                    if (i <= toplamSayfa)
                    {
                        sayfalama = sayfalama + liBaslangic + "<a class=\"" + aCSS + "\" href=\"" + link + "/" + (i).ToString() + "\">" + i + "</a>" + liBitis;
                    }
                }


            }


            // Bulunduğu sayfa son sayfadan küçükse Sonraki linkini ekliyoruz
            if (toplamSayfa > bulunduguSayfa)
            {
                sayfalama = sayfalama + liBaslangic + "<a class=\"" + aCSS + "\" href=\"" + link + "/" + (bulunduguSayfa + 1).ToString() + "\">Sonraki</a>" + liBitis;
            }

            // Son sayfa kodu
            sayfalama = sayfalama + liBaslangic + "<a class=\"" + aCSS + "\" href=\"" + link + "/" + (toplamSayfa).ToString() + "\">Son</a>" + liBitis;


            return MvcHtmlString.Create(sayfalama);

        }
        public static string TelefonDuzelt(string telefonNo)
        {
            //Regex rgx = new Regex("[^0-9]");
            //TelefonNo = rgx.Replace(TelefonNo, "");

            //if (TelefonNo.Length == 7)
            //{
            //    TelefonNo = "0242" + TelefonNo;
            //}

            //if (TelefonNo.Substring(0, 1) != "0")
            //{
            //    TelefonNo = "0" + TelefonNo;
            //}
            //TelefonNo = TelefonNo.Substring(TelefonNo.Length - 11);
            //TelefonNo = "(" + TelefonNo.Substring(0, 4) + ")-" + TelefonNo.Substring(4, 3) + "-" + TelefonNo.Substring(7, 2) + "-" + TelefonNo.Substring(9, 2);
            telefonNo=Regex.Replace(telefonNo, @"[^\d]", "");
            return telefonNo;
        }
    }

}