using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public enum IslerOncelikSira:byte
    {
        Birinci=1,Ikinci=2,Ucuncu=3
    }
    public enum IsinDurumu:byte
    {
        Yapilacak = 1,
        YapilacakDeadline,
        Yapiliyor,
        KontrolBekleyen,
        Biten
    }
    public enum DomainAksiyon : byte
    {
        BeklemeyeAl=1,YayinaAl,YayiniDurdur
    }
    public enum MesaiDurum
    {
        Mesai=1,HaftaSonu,ResmiTatil,Rapor,UcretliIzin,UcretsizIzin,YillikIzin
    }
    public enum TodoDurum:byte
    {
        Beklemede=0,Tamamlanmis=1
    }
    public enum EtiketIsPlaniDurum : byte
    {
        Atlandi=1, Eklendi=2, Tamamlandi=3
    }
}