using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public static class Sorgular
    {
        public static IEnumerable<isler> GetirIsler(this IEnumerable<isler> kaynakTablo, bool kontrolBekleyenIsler, bool onaylananIsler, string kullaniciID, int? RefBolgeID)
        {
            //kontrolbekleyen işler
            if (kontrolBekleyenIsler == true && onaylananIsler == false)
            {
                return (from d in kaynakTablo
                        where d.islerinisinOnayDurumu != true && d.islerisinTamamlanmaDurumu == true && d.islerisiVerenKisi == kullaniciID
                        orderby d.islerTarih descending
                        select d).ToList();
            }
            //yeni işler
            else if (onaylananIsler == false && kontrolBekleyenIsler == false)
            {
                List<int> idler;

                using (var db = new PortalEntities())
                {
                    idler = db.IsiYapacakKisis.Where(a => a.RefIsiYapacakKisiUserID == kullaniciID).Select(x => x.RefIsID).ToList();
                }


                return from d in kaynakTablo
                       where d.islerinisinOnayDurumu != true && d.islerisinTamamlanmaDurumu == false && idler.Contains(d.islerID)
                       orderby d.islerTarih descending
                       select d;
            }

            else if (onaylananIsler == true && kontrolBekleyenIsler == true)
            {
                return from d in kaynakTablo
                       where d.islerinisinOnayDurumu == true && d.islerisinTamamlanmaDurumu == true && d.islerisiVerenKisi == kullaniciID
                       orderby d.islerTarih descending
                       select d;
            }
            else
            {
                return from d in kaynakTablo
                       where d.islerisiVerenKisi == kullaniciID
                       orderby d.islerTarih descending
                       select d;
            }


        }


    }
}