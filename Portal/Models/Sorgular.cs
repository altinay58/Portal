using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}