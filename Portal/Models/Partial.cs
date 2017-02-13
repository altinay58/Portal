using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public static class Partial
    {
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
    }
}