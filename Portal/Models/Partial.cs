using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Portal.Models.IslerModels;

namespace Portal.Models
{
    public static class Partial
    {

        //#region Domain Sorguları

        //public static IEnumerable<Domain> SQLDomainler()
        //{
        //    using (var dbc = new PortalEntities())
        //    {
        //        return dbc.Domains.ToList();
        //    }
        //}

        //public static IEnumerable<Domain> tumDomainler = SQLDomainler();

        //public static IEnumerable<Domain> Domainler()
        //{
        //    return tumDomainler.Where(a => a.DomainDurum == true);
        //}

        //public static IEnumerable<Domain> SilinenDomainler()
        //{
        //    return tumDomainler.Where(a => a.DomainDurum == false);
        //}


        //public static string DomainAdi(int domainID)
        //{
        //    if(tumDomainler.FirstOrDefault(a=>a.DomainID == domainID) != null)
        //    {
        //        return tumDomainler.FirstOrDefault(a => a.DomainID == domainID).DomainAdi;
        //    }
        //    else
        //    {
        //        return "Domain Bulunamadı";
        //    }
        //}

        //#endregion Domain Sorguları


            public static string Ayar(string adi)
        {
            using (var db = new PortalEntities())
            {
                return db.Ayars.FirstOrDefault(a => a.AyarAdi == adi).AyarDeger;
            }
        }

        public static IEnumerable<Domain> GetirUzatmasiGelenler()
        {
            using (var dbc = new PortalEntities())
            {
                DateTime uTarih = DateTime.Now.AddMonths(2);
                return (from a in dbc.Domains
                       where a.DomainDurum == true && a.UzatmaTarihi <= uTarih
                       orderby a.UzatmaTarihi ascending
                       select a).ToList();
            }
        }

        public static string IsinAdi(int isID)
        {
            string Isim = string.Empty;
            using (var dbc = new PortalEntities())
            {
                if (dbc.islers.FirstOrDefault(a=>a.islerID == isID) == null)
                {
                    return "Kullanıcı Yok.";
                }
                Isim = dbc.islers.FirstOrDefault(a => a.islerID == isID).islerAdi.ToString();
            }
            return Isim;
        }

        public static string DomainAdiGetirIsId(int isID)
        {
            using (var dbc = new PortalEntities())
            {
                isler isimiz = dbc.islers.FirstOrDefault(a => a.islerID == isID);
                if (isimiz == null)
                {
                    return "İş Bulunamadı. Hata : D001";
                }
                return Partial.DomainAdi(isimiz.islerRefDomainID??0);
            }
        }

        public static int DomainIdGetirIsId(int isID)
        {
            using (var dbc = new PortalEntities())
            {
                isler isimiz = dbc.islers.FirstOrDefault(a => a.islerID == isID);
                if (isimiz == null)
                {
                    return 0;
                }
                else
                {
                    return isimiz.islerRefDomainID ?? 0;
                }
            }
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

        public static BildirimlerView KullaniciBildirim(string KullaniciID)
        {
            BildirimlerView bildirimler = new BildirimlerView();

            if (!string.IsNullOrEmpty(KullaniciID))
                using (var dbc = new PortalEntities())
                {
                    DateTime tarih = DateTime.Now;
                    IEnumerable<isler> Yeniislerim = dbc.islers.GetirIsler(false, false, KullaniciID, null).ToList();
                    IEnumerable<isler> KontrolBekleyenIsler = dbc.islers.GetirIsler(true, false, KullaniciID, null).ToList();
                    IEnumerable<Randevu> Randevular = dbc.Randevus.Where(m => m.RandevuYetkiliKisiID == KullaniciID && m.RandevuTarihi.Value.Year + m.RandevuTarihi.Value.Month + m.RandevuTarihi.Value.Day == tarih.Year + tarih.Month + tarih.Day).ToList();
                    bildirimler.BildirimSayisi = Yeniislerim.ToList().Count + KontrolBekleyenIsler.ToList().Count + Randevular.ToList().Count;
                    bildirimler.yeniIsler = Yeniislerim;
                    bildirimler.kontrolBekleyenIsler = KontrolBekleyenIsler;
                    bildirimler.Randevular = Randevular;
                }

            return bildirimler;
        }
        //public static List<IslerListesi>KontrolBekleyenIsler(string kullaniciAdSoyad)
        //{
        //    var list = Database.DbHelper.GetDb.IslerListesis.
        //        Where(x => x.IsiVerenKisi.Contains(kullaniciAdSoyad) && x.IsinDurumu!="Biten").ToList();
        //    return list;
        //}
        public static List<IslerListesi> YeniIsler(string kullaniciAdSoyad)
        {
            var list = Database.DbHelper.GetDb.IslerListesis.
                Where(x => x.IsiYapacakKisi.Contains(kullaniciAdSoyad) && (x.IsinDurumu == "Yapilacak" || x.IsinDurumu == "YapilacakDeadline")).ToList();
            return list;
        }
        public static List<IslerListesi> KontrolEdilecekIsler(string kullaniciAdSoyad)
        {
            var list = Database.DbHelper.GetDb.IslerListesis.
                Where(x => x.IsiVerenKisi.Contains(kullaniciAdSoyad) && x.IsinDurumu == "KontrolBekleyen").ToList();
            return list;
        }
        public static IEnumerable<isler> CevaplananIslerOkunmayanlar(string KullaniciID)
        {

            using (var dbc = new PortalEntities())
            {
                List<int> yorumlananIsIdleri = dbc.YorumDurums.Where(a => a.RefUserID == KullaniciID).Select(a => a.RefIsID).ToList();

                return dbc.islers.Where(a => yorumlananIsIdleri.Contains(a.islerID)).ToList();

            }
        }
        public static IEnumerable<TeklifDetay> TeklifDetaylariGoruntule(int teklifDetayRefTeklifID)
        {
            using (var dbc = new PortalEntities())
            {
                return dbc.TeklifDetays.Where(m => m.teklifDetayRefTeklifID == teklifDetayRefTeklifID).OrderByDescending(m => m.teklifDetayTarih).ToList();
            }

        }

        public static IEnumerable<Teklif> TeklifleriGoruntule(int teklifRefArayanID)
        {
            List<Teklif> items;
            using (var dbc = new PortalEntities())
            {
                items = dbc.Teklifs.Where(m => m.teklifRefArayanID == teklifRefArayanID).OrderByDescending(m => m.teklifTarih).ToList();

                return items;
            }

        }

        public static string DomainAdi(int id)
        {
            using (var dbc = new PortalEntities())
            {
                return dbc.Domains.FirstOrDefault(a => a.DomainID == id).DomainAdi;
            }

        }

        public static string FirmaAdi(int id)
        {
            using (var dbc = new PortalEntities())
            {
                return dbc.Firmas.FirstOrDefault(a => a.FirmaID == id).FirmaAdi;
            }

        }

        public static string HostingAdi(int id)
        {
            using (var dbc = new PortalEntities())
            {
                return dbc.Hostings.FirstOrDefault(a => a.HostingID == id).HostingAdi;
            }

        }

        public static string KayitliFirmaAdi(int id)
        {
       
            var firma = Database.DbHelper.GetDb.KayitliFirmas.FirstOrDefault(a => a.KayitliFirmaID == id);
            if (firma != null)
            {
                return firma.KayitliFirmaAdi;
            }else
            {
                return "";
            }           

        }

        public static string DomainKategoriAdi(int id)
        {
            using (var dbc = new PortalEntities())
            {
                return dbc.DomainKategoris.FirstOrDefault(a => a.DomainKategoriID == id).DomainKategoriAdi;
            }

        }


        public static IEnumerable<Firma> FirmalariGoruntule()
        {
            List<Firma> items;
            using (var dbc = new PortalEntities())
            {
                items = dbc.Firmas.Where(a => a.FirmaSilindi == false).ToList();

                return items;
            }

        }
        public static IEnumerable<Arayanlar> ArayanlariGoruntule()
        {
            List<Arayanlar> items;
            using (var dbc = new PortalEntities())
            {
                items = dbc.Arayanlars.Where(m => m.arayanFirmaKayitDurum == false).ToList();

                return items;
            }

        }

        public static IEnumerable<Domain> DomainleriGoruntule()
        {
            List<Domain> items;
            using (var dbc = new PortalEntities())
            {
                items = dbc.Domains.ToList();

                return items;
            }

        }


        public static IEnumerable<Makale> MakaleleriGoruntule()
        {
            List<Makale> items;
            using (var dbc = new PortalEntities())
            {
                items = dbc.Makales.ToList();

                return items;
            }

        }
        public static IEnumerable<MakaleKategori> MakaleleKategorileriGoruntule()
        {
            List<MakaleKategori> items;
            using (var dbc = new PortalEntities())
            {
                items = dbc.MakaleKategoris.ToList();

                return items;
            }

        }





        public static IEnumerable<Istekler> IstekleriGoruntule(DateTime? tarih)
        {
            List<Istekler> items;

            using (var dbc = new PortalEntities())
            {
                if (string.IsNullOrEmpty(tarih.ToString()))
                {
                    items = dbc.Isteklers.Where(m => m.istekTarih.Value.ToShortDateString() == tarih.Value.ToShortDateString()).ToList();
                }
                else
                {
                    items = dbc.Isteklers.ToList();

                }

                return items;
            }

        }
        //public static BildirimlerView KullaniciBildirim(string KullaniciID)
        //{
        //    BildirimlerView bildirimler = new BildirimlerView();

        //    if (!string.IsNullOrEmpty(KullaniciID))
        //        using (var dbc = new KarayelEntities())
        //        {
        //            DateTime tarih = DateTime.Now;
        //            IEnumerable<isler> Yeniislerim = dbc.islers.GetirIsler(false, false, KullaniciID, null).ToList();
        //            IEnumerable<isler> KontrolBekleyenIsler = dbc.islers.GetirIsler(true, false, KullaniciID, null).ToList();
        //            IEnumerable<Randevu> Randevular = dbc.Randevus.Where(m => m.RandevuYetkiliKisiID == KullaniciID && m.RandevuTarihi.Value.Year + m.RandevuTarihi.Value.Month + m.RandevuTarihi.Value.Day == tarih.Year + tarih.Month + tarih.Day).ToList();
        //            bildirimler.BildirimSayisi = Yeniislerim.ToList().Count + KontrolBekleyenIsler.ToList().Count + Randevular.ToList().Count;
        //            bildirimler.yeniIsler = Yeniislerim;
        //            bildirimler.kontrolBekleyenIsler = KontrolBekleyenIsler;
        //            bildirimler.Randevular = Randevular;
        //        }

        //    return bildirimler;
        //}

        //public static IEnumerable<isler> CevaplananIslerOkunmayanlar(string KullaniciID)
        //{


        //    using (var dbc = new KarayelEntities())
        //    {
        //        List<int> yorumlananIsIdleri = dbc.YorumDurums.Where(a => a.RefUserID == KullaniciID).Select(a => a.RefIsID).ToList();

        //        return dbc.islers.Where(a => yorumlananIsIdleri.Contains(a.islerID)).ToList();

        //    }
        //}
        public static int YapilacaklarSayisi()
        {            
            var userId = HttpContext.Current.User.Identity.GetUserId();
            int count = Database.DbHelper.GetDb.ToDoes.Where(x => x.KulId == userId && x.Durum==(int)TodoDurum.Beklemede).Count();
            return count;
         }
        public static int BuguneAitIsPlanSayisi()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var count = Database.DbHelper.GetDb.IsPlanis.Where(x => System.Data.Entity.DbFunctions.TruncateTime(x.Tarih) == DateTime.Today 
            && x.EtiketIsPlaniDurum!=(int)EtiketIsPlaniDurum.Tamamlandi && x.RefSorumluKisiId==userId).Count();
            return count;
        }
        public static string KulllaniciIsmi()
        {
            string name = "";
            object guncelKullanici = HttpContext.Current.Session["user"];
            if (guncelKullanici != null)
            {
                name = ((AspNetUser)guncelKullanici).Isim;
            }else
            {
                var userId = HttpContext.Current.User.Identity.GetUserId();
                AspNetUser user = Database.DbHelper.GetDb.AspNetUsers.SingleOrDefault(x => x.Id == userId);
                guncelKullanici = user;
                name = user.Isim;
            }
            return name;
        }
        public static Kullanici GuncelKullanici()
        {
            string userId = HttpContext.Current.User.Identity.GetUserId() ?? "f5f53da2-c311-44c9-af6a-b15ca29aee57";
            var guncelKullanici = Database.DbHelper.GetDb.AspNetUsers.Where(x => x.Id == userId).
                                    Select(x => new Kullanici { Id = x.Id, AdSoyad = x.Isim + " " + x.SoyIsim }).FirstOrDefault();
            return guncelKullanici;
        }
    }
}