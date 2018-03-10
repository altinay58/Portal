using System;
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


        public static void SifreGonder(string mail, string sifirlamaKodu)
        {
            string Yazi = "";
            Yazi = Yazi + "Musteri Giris Bilgileriniz asagidadir...<br />";
            Yazi = Yazi + "Kullanici Adi : " + mail + "<br />";
            Yazi = Yazi + "Şifre : " + sifirlamaKodu + " (Giris yaptiktan sonra acilan menuden <strong>Şifre Değiştir</strong> secenegini kullanarak degistirebilirsiniz.)<br />";
            Yazi = Yazi + "Saygilarimizla...<br />";
            Yazi = Yazi + "<br />";

            MailGonder(mail, "Yeni Yönetici Şifreniz", Yazi);

        }

        public static string SifreSifirlamaMailiGonder(string mail, string sifirlamaKodu, string userID)
        {

            string siteadresi = "http://portal.karayeltasarim.com";
            string baslik = "Şifre Sıfırlama Maili";
            string Yazi = "Site : " + siteadresi + "<br /><br />";
            Yazi = Yazi + "Parola sıfırlama maili göndermediyseniz bu maili dikkate almayın.<br /><br /><br />";
            Yazi = Yazi + "Maili siz gönderdiyseniz!!!<br />";
            Yazi = Yazi + "Parolanızı sıfırlamak için aşağıdaki linke tıklayın.<br /><br />";
            Yazi = Yazi + "Yeni Parolanız Mail Adresinize Gönderilecektir!!!<br />";
            Yazi = Yazi + "<a href=\"" + siteadresi + "/kullanici/parola/sifirla/" + sifirlamaKodu + "/" + userID + "\" target=\"_blank\">Parola Sıfırlama</a><br />";
            Yazi = Yazi + "Saygilarimizla...<br />";
            Yazi = Yazi + "<br />";

            return Fonksiyonlar.MailGonder(mail, baslik, Yazi);
        }

        public static string SuresiDolanDomainleriMailGonder()
        {

            using (var db = new PortalEntities())
            {

                DateTime birgun = DateTime.Now.AddDays(1).Date;
                DateTime ongun = DateTime.Now.AddDays(10).Date;
                DateTime yirmigun = DateTime.Now.AddDays(20).Date;
                DateTime otuzgun = DateTime.Now.AddDays(30).Date;
                DateTime kirkbesgun = DateTime.Now.AddDays(45).Date;

                IEnumerable<Domain> domainler = db.Domains.Where(a => a.DomainDurum == true && (a.UzatmaTarihi == birgun || a.UzatmaTarihi == ongun || a.UzatmaTarihi == yirmigun || a.UzatmaTarihi == otuzgun || a.UzatmaTarihi == kirkbesgun));

                foreach (Domain domain in domainler)
                {
               
            string mesaj = "" +
            "<p> Sayın Yetkili,</p><p> Aşağıdaki hizmetlerinizin süresi dolmak üzeredir.</p>" +
            "<table border = \"1\" cellpadding = \"5\" cellspacing = \"0\" style = \"width:800px;\" width = \"800\">" +
            "<tbody>" +
            "<tr><td style = \"width:180px;\">Sipariş Tarihi</td><td style = \"width:620px;\">"+ domain.UzatmaTarihi.Date +"</td></tr>" +
            "<tr><td style = \"width:180px;\">Müşteri</td><td style = \"width:620px;\">"+ domain.Firma.FirmaAdi +"</td></tr>" +
            "<tr><td style = \"width:180px;\">Sipariş Bakiyesi</td><td style = \"width:620px;\">" + domain.DomainUrun.DomainUrunFiyati * 1.18 +"TL</td></tr>" +
            "</tbody>" +
            "</table>" +
            "<p> &nbsp;</p>" +
            "<table border = \"1\" cellpadding = \"5\" cellspacing = \"0\" style = \"width:800px;\" width = \"800\">" +
            "<tbody>" +
            "<tr><td>Hizmet</td><td>Fiyat</td><td>Servis Dönemi</td><td>+KDV 18.00 %</td><td>Toplam</td></tr>" +
            "<tr><td>1 yıl için "+ domain.DomainAdi +" barındırma hizmeti</td><td> "+ domain.DomainUrun.DomainUrunFiyati + "TL</td><td><p> 1 yıl </p></td><td><p> " + ((domain.DomainUrun.DomainUrunFiyati * 1.18) - domain.DomainUrun.DomainUrunFiyati) + "TL </p></td><td>" + domain.DomainUrun.DomainUrunFiyati * 1.18 + "TL</td></tr>" +
            "</tbody>" +
            "</table>" +
             "<p>Toplam Tutar: " + domain.DomainUrun.DomainUrunFiyati * 1.18 + "TL </p>" +
             "<p>Ödemelerinizi aşağıda belirtilen banka hesap numaralarına Havale / Eft ile yapabilirsiniz. Ödemeniz alındıktan sonra faturanız adresinize gönderilecektir. Faturanızın kesilmesini istediğiniz firmanızın ünvan ve adres bilgilerinde değişiklik olması durumunda, ödemenizi yapmadan önce <a href = \"mailto:muhasebe@karayeltasarim.com\"> muhasebe@karayeltasarim.com </a> güncel bilgilerinizi göndermenizi rica ederiz.</p>" +
             "<p><strong> Karayel Arge Tasarım Bil.Rek.Öz.Eğ.İnş.Taş.San.Ve Tic.Ltd.Şti.</strong><br />" +
             "Ziraat Bankası <br />" +
             "<strong> Şube:</strong>Özgürlük Bulvarı / Antalya Şubesi <br />" +
             "<strong> Hesap Numarası:</strong> 2620 - 72735807 - 5001 <br />" +
             "<strong> Iban Numarası:</strong> TR 4100 0100 2620 7273 5807 5001 </p>" +
            "<p><br />" +
            "<strong> Karayel Arge Tasarım Ltd.Şti.</strong><br />" +
            "<br />" +
            "<strong> Adres :</strong>Pınarbaşı Mah. H & uuml; rriyet Cad. Akdeniz & Uuml; niversitesi Teknokent Arge 1 Binası No:3 / B Kat: 1 / 5A & ndash; 07070 Konyaaltı / ANTALYA <br />" +
            "<strong> Telefon :</strong> &nbsp; +90(242) 344 10 20 <br />" +
            "<br />" +
            "<strong> Adres :</strong> &nbsp; Hobyar Mah. Hocahanı Sk. Bahtiyar Han No: 14 Kat: 7 / 116 & nbsp;/ Fatih / İSTANBUL <br />" +
            "<strong> Telefon :</strong> &nbsp; +90(212) 512 52 52 <br />" +
            "<strong> Bilgi ve Destek :</strong> &nbsp;<a href = \"mailto:info@karayeltasarim.com\"> info@karayeltasarim.com </a><br />" +
            "<strong> Web:</strong> &nbsp;<a href = \"http://www.karayeltasarim.com\"> www.karayeltasarim.com </a></p>";

                    Fonksiyonlar.MailGonder("info@karayeltasarim.com", "Hizmet Süresi Dolum Bildirimi -- Son "+ DomainUzatmaSuresineKalanGun(domain.UzatmaTarihi) + " gün", mesaj);
                }
            }

            //string mesaj = "<table>";

            //using (var db = new PortalEntities())
            //{
            //    IEnumerable<Domain> domainler = db.Domains.GetirUzatmasiGelenler();

            //    foreach (Domain domain in domainler)
            //    {
            //        mesaj = mesaj + "<tr><td>" + domain.Firma.FirmaAdi + "</td><td>" + domain.DomainAdi + "</td><td>" + Fonksiyonlar.DomainUzatmaSuresineKalanGun(domain.UzatmaTarihi) + "</td></tr>";
            //    }
            //}

            //mesaj = mesaj + "</table>";


            

            return "Gonderildi";
        }

        public static bool DomainMailGonderilsinMi()
        {

            if (DateTime.Now.Hour < 8 || DateTime.Now.Hour > 19)
            {
                return false;
            }

            using (var db = new PortalEntities())
            {
                Ayar mailTarih = db.Ayars.FirstOrDefault(a => a.AyarAdi == "DomainMailGonderilmeTarihi");

                if (mailTarih != null)
                {
                    try
                    {
                        DateTime MailGonderimTarihi = Convert.ToDateTime(mailTarih.AyarDeger);

                        if (MailGonderimTarihi.Date < DateTime.Now.Date)
                        {
                            mailTarih.AyarDeger = DateTime.Now.ToShortDateString();
                            db.SaveChanges();
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                    catch
                    {
                        mailTarih.AyarDeger = DateTime.Now.ToShortDateString();
                        db.SaveChanges();
                        return true;
                    }

                }
                else
                {
                    Ayar yeniayar = new Ayar()
                    {
                        AyarDeger = DateTime.Now.ToShortDateString(),
                        AyarAdi = "DomainMailGonderilmeTarihi"
                    };
                    db.Ayars.Add(yeniayar);
                    db.SaveChanges();
                    return true;
                }
            }

        }

        public static int DomainUzatmaSuresineKalanGun(DateTime uzatmaTarihi)
        {

            TimeSpan fark = uzatmaTarihi - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            return Convert.ToInt32(fark.TotalDays);

        }
        public static string MailGonder(string gidecekMailAdresleri, string baslik, string mesaj)
        {

            // KarayelEntities db = new KarayelEntities();

            //db.Database.ExecuteSqlCommand("insert into Mail.dbo.MailGonder values ('info@antalyawebtasarim.com','Awt@1234',1,'smtp.live.com',587,'" + gidecekMailAdresleri + "','" + baslik + "','" + mesaj + "')");

            string MailAdresi = Partial.Ayar("MailAdresi");

            string MailSifresi = Partial.Ayar("MailSifresi");
            bool MailSsl = true;
            string SMTPAdresi = Partial.Ayar("SMTPAdresi");
            string Port = Partial.Ayar("Port");
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
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(MailAdresi, MailSifresi);
                
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
                        sayfalama = sayfalama + aktifLi + "<a class=\"" + aCSS + " " + activeCSS + "\" href=\"" + link + "/" + (bulunduguSayfa).ToString() + ">" + bulunduguSayfa + "</a>" + liBitis;
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
        public static string TelefonDuzelt(string TelefonNo)
        {
            if (!string.IsNullOrEmpty(TelefonNo))
            {
                Regex rgx = new Regex("[^0-9]");
                TelefonNo = rgx.Replace(TelefonNo, "");

                if (TelefonNo.Length > 0 && TelefonNo.Substring(0, 1) != "0")
                {
                    TelefonNo = "0" + TelefonNo;
                }
                if(TelefonNo.Length > 10)
                {
                    TelefonNo = TelefonNo.Substring(TelefonNo.Length - 11);
                    TelefonNo = TelefonNo.Substring(0, 4) + " " + TelefonNo.Substring(4, 3) + " " + TelefonNo.Substring(7, 2) + " " + TelefonNo.Substring(9, 2);
                }
            }

            return TelefonNo;
        }
        public static string DomainAdiGetir(int DomainID)
        {
            string domainAdi = Database.DbHelper.GetDb.Domains.Find(DomainID).DomainAdi;  

            return domainAdi;
        }
        public static string MailGonderOdeme(string gidecekMailAdresleri, string baslik, string mesaj)
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
        public static IEnumerable<CariHareket> GetirCariHareketler(int firmaID)
        {
            var Db = Database.DbHelper.GetDb;
            var kategoriler = from a in Db.CariHarekets
                              where a.RefFirmaID == firmaID
                              orderby a.ChTarihi descending
                              select a;
            return kategoriler;
        }
        public static IEnumerable<Domain> GetirFirmaDomainleri(int firmaID)
        {
            var Db = Database.DbHelper.GetDb;
            IEnumerable<Domain> domainler = Db.Domains.ToList();
            IEnumerable<Domain> yeniDomainler = Db.Domains.ToList();
            domainler = domainler.Where(a => a.RefDomainFirmaID == firmaID && a.DomainDurum == true).ToList();
            domainler = domainler.OrderByDescending(m => m.IpAdres).ToList();
            return domainler;
        }
        public static string KullaniciAdi(string userNameOrEmailorId)
        {
            var db = Database.DbHelper.GetDb;
            var kullanici = db.AspNetUsers.FirstOrDefault(a => a.UserName == userNameOrEmailorId || a.Id == userNameOrEmailorId);
            if (kullanici != null)
            {            
                string kullaniciIsmi = kullanici.Isim + " " + kullanici.SoyIsim;
                return kullaniciIsmi;
            }
            else
            {
                return "Kullanıcı Adı Bulunamadı";
            }
        }
        public static string BolgeAdiGetir(int? KonumID)
        {
            string bolgeAdi = string.Empty;
            using (var dbc = new PortalEntities())
            {
                bolgeAdi = dbc.Konums.Find(KonumID).Konum1.ToString();

            }

            return bolgeAdi;
        }

        public static string FirmaAdiGetir(int FirmaID)
        {
            string firmaAdi = string.Empty;
            using (var dbc = new PortalEntities())
            {
                firmaAdi = dbc.Firmas.Find(FirmaID).FirmaAdi.ToString();

            }

            return firmaAdi;
        }
        public static string FirmaSahipAdiGetir(int? FirmaID)
        {
            string firmaSahibiAdi = string.Empty;
            using (var dbc = new PortalEntities())
            {
                firmaSahibiAdi = dbc.Firmas.Find(FirmaID).FirmaKisis.FirstOrDefault().Ad.ToString();
                firmaSahibiAdi = firmaSahibiAdi + "/" + dbc.Firmas.Find(FirmaID).FirmaKisis.FirstOrDefault().Ad;
            }

            return firmaSahibiAdi;
        }
        public static string ArayanFirmaAdiGetir(int? arayanID)
        {
            string Isim = string.Empty;
            using (var dbc = new PortalEntities())
            {
                Isim = dbc.Arayanlars.Find(arayanID).arayanFirmaAdi.ToString();

            }

            return Isim;
        }
        public static string ArayanKisiAdiGetir(int? arayanID)
        {
            string Isim = string.Empty;
            using (var dbc = new PortalEntities())
            {
                Isim = dbc.Arayanlars.Find(arayanID).arayanAdi.ToString();
                Isim = Isim + "/" + dbc.Arayanlars.Find(arayanID).arayanSoyadi;

            }

            return Isim;
        }
        public static string KisiAdiGetir(string KisiID)
        {
            string Isim = string.Empty;
            using (var dbc = new PortalEntities())
            {
                if (dbc.AspNetUsers.Find(KisiID) == null)
                {
                    return "Kullanıcı Yok.";
                }
                Isim = dbc.AspNetUsers.Find(KisiID).Isim.ToString();

            }

            return Isim;
        }
    }

}