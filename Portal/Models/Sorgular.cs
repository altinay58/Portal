using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace Portal.Models
{
    public static class Sorgular
    {
        public static IEnumerable<isler> GetirIsler(this IEnumerable<isler> kaynakTablo, bool? kontrolBekleyenIsler, bool? onaylananIsler, string kullaniciID, int? RefBolgeID)
        {

            return (from d in kaynakTablo
                    where (kontrolBekleyenIsler == true ? d.islerinisinOnayDurumu == true : d.islerinisinOnayDurumu == false)
                    &&    (onaylananIsler == true ? d.islerinisinOnayDurumu == true : d.islerinisinOnayDurumu == false)
                    &&    (kullaniciID == null ? true : d.islerisiVerenKisi == kullaniciID)
                    select d).ToList();


        }
        public static bool DomainEklimi(this IEnumerable<Domain> kaynakTablo, string domainAdi)
        {
            if (kaynakTablo.FirstOrDefault(q => q.DomainAdi == domainAdi) == null)
                return false;
            else
                return true;
        }
        public static IEnumerable<Domain> GetirSilinenDomainler(this IEnumerable<Domain> kaynakTablo, int sayfadaGosterilecekDomainSayisi, int baslangic)
        {
            return (from q in kaynakTablo
                    where q.DomainDurum == false
                    orderby q.SilmeTarihi descending
                    select q).Skip(baslangic).Take(sayfadaGosterilecekDomainSayisi);
        }
        public static IEnumerable<Domain> GetirSilinenDomainler(this IEnumerable<Domain> kaynakTablo)
        {
            return from q in kaynakTablo
                   where q.DomainDurum == false
                   orderby q.DomainID descending
                   select q;
        }
        public static IEnumerable<Firma> GetirFirmalar(this IEnumerable<Firma> kaynakTablo, string durum)
        {
            if (durum == "personel")
            {
                return from q in kaynakTablo
                       where q.Personel == true && q.FirmaSilindi == false
                       orderby q.FirmaAdi ascending
                       select q;
            }
            if (durum == "kasa")
            {
                return from q in kaynakTablo
                       where q.Kasa == true && q.FirmaSilindi == false
                       orderby q.FirmaAdi ascending
                       select q;
            }
            else if (durum == "araci")
            {
                return from q in kaynakTablo
                       where q.Araci == true && q.FirmaSilindi == false
                       orderby q.FirmaAdi ascending
                       select q;
            }
            else if (durum == "musteri")
            {
                return from q in kaynakTablo
                       where q.Musteri == true && q.FirmaSilindi == false
                       orderby q.FirmaAdi ascending
                       select q;
            }
            else
            {
                return from q in kaynakTablo
                       where q.FirmaSilindi == false
                       orderby q.FirmaAdi ascending
                       select q;
            }

        }
        public static IEnumerable<DomainKategori> GetirDomainKategorileri(this IEnumerable<DomainKategori> kaynakTablo)
        {
            return from d in kaynakTablo
                   orderby d.DomainKategoriAdi ascending
                   select d;
        }
        public static IEnumerable<Domain> GetirDomainler(this IEnumerable<Domain> kaynakTablo, int sayfadaGosterilecekDomainSayisi, int baslangic)
        {
            return (from q in kaynakTablo
                    where q.DomainDurum == true
                    orderby q.DomainID descending
                    select q).Skip(baslangic).Take(sayfadaGosterilecekDomainSayisi);
        }
        public static IEnumerable<Domain> GetirDomainler(this IEnumerable<Domain> kaynakTablo)
        {
            return from q in kaynakTablo
                   where q.DomainDurum == true
                   orderby q.DomainID descending
                   select q;
        }
        public static IEnumerable<Hosting> GetirHosting(this IEnumerable<Hosting> kaynakTablo)
        {
            return from d in kaynakTablo
                   orderby d.HostingAdi ascending
                   select d;
        }
        public static Domain GetirDomain(this IEnumerable<Domain> kaynakTablo, int id)
        {
            return kaynakTablo.FirstOrDefault(q => q.DomainID == id && q.DomainDurum == true);
        }
        public static IEnumerable<Domain> GetirUzatmasiGelenler(this IEnumerable<Domain> kaynakTablo)
        {

            return from a in kaynakTablo
                   where a.DomainDurum == true && a.UzatmaTarihi <= DateTime.Now.AddMonths(2)
                   orderby a.UzatmaTarihi ascending
                   select a;


        }
        public static bool KategorideDomainVarmi(this IEnumerable<Domain> kaynakTablo, int kategoriID)
        {
            if (kaynakTablo.FirstOrDefault(q => q.RefDomainKategori == kategoriID) == null)
                return false;
            else
                return true;
        }
        public static DomainKategori GetirDomainKategori(this IEnumerable<DomainKategori> kaynakTablo, int id)
        {
            return kaynakTablo.FirstOrDefault(q => q.DomainKategoriID == id);
        }
        public static bool DomainKategoriEklimi(this IEnumerable<DomainKategori> kaynakTablo, string domainKateogriAdi)
        {
            if (kaynakTablo.FirstOrDefault(q => q.DomainKategoriAdi == domainKateogriAdi) == null)
                return false;
            else
                return true;
        }
        public static long DomainToplamZaman(int domainId)
        {
            using (PortalEntities Db=new PortalEntities())
            {
                var q = Db.Database
                                    .SqlQuery<long?>(@"select sum(z.GecenZamanSaniye) 
                     from ZamanIs z left outer join isler i on(z.RefIsId=i.islerID)
                  where i.islerRefDomainID=@p0", domainId);
                return q.SingleOrDefault() ?? 0;
            }
                
        }
        public static IEnumerable<Firma> GetirFirmalar(this IEnumerable<Firma> kaynakTablo, int sayfadaGosterilecekDomainSayisi, int baslangic, string durum)
        {
            if (durum == "personel")
            {
                return (from q in kaynakTablo
                        where q.Personel == true && q.FirmaSilindi == false
                        orderby q.FirmaID descending
                        select q).Skip(baslangic).Take(sayfadaGosterilecekDomainSayisi);
            }
            if (durum == "kasa")
            {
                return (from q in kaynakTablo
                        where q.Kasa == true && q.FirmaSilindi == false
                        orderby q.FirmaID descending
                        select q).Skip(baslangic).Take(sayfadaGosterilecekDomainSayisi);
            }
            else if (durum == "araci")
            {
                return (from q in kaynakTablo
                        where q.Araci == true && q.FirmaSilindi == false
                        orderby q.FirmaID descending
                        select q).Skip(baslangic).Take(sayfadaGosterilecekDomainSayisi);
            }
            else if (durum == "musteri")
            {
                return (from q in kaynakTablo
                        where q.Musteri == true && q.FirmaSilindi == false
                        orderby q.FirmaID descending
                        select q).Skip(baslangic).Take(sayfadaGosterilecekDomainSayisi);
            }
            else
            {
                return (from q in kaynakTablo
                        where q.FirmaSilindi == false
                        orderby q.FirmaID descending
                        select q).Skip(baslangic).Take(sayfadaGosterilecekDomainSayisi);
            }
        }
        public static IEnumerable<Konum> GetirKonumlar(this IEnumerable<Konum> kaynakTablo)
        {
            return from a in kaynakTablo
                   orderby a.Konum1 ascending
                   select a;
        }
        public static IEnumerable<Arayanlar> GetirArayanGecmisAramalar(this IEnumerable<Arayanlar> kaynakTablo, string FirmaAdi)
        {

            return (from d in kaynakTablo
                    where d.arayanFirmaAdi.Replace(" ", "").ToUpper() == FirmaAdi.ToString()
                    orderby d.arayanKayitTarih descending
                    select d).ToList();
        }
    }
}