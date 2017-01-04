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


    }
}