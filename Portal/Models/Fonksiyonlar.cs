using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public partial class Fonksiyonlar
    {
        public static int DomainUzatmaSuresineKalanGun(DateTime uzatmaTarihi)
        {

            TimeSpan fark = uzatmaTarihi - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            return Convert.ToInt32(fark.TotalDays);

        }
    }
}