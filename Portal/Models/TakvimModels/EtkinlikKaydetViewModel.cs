using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class EtkinlikKaydetViewModel
    {
        [Display(Name = "Etkinlik Id")]
        public string etkinlikId { get; set; }

        [Required]
        [Display(Name = "Kullanıcı")]
        public string userId { get; set; }
        
        [Required]
        [Display(Name = "Başlık")]
        public string takvimBaslik { get; set; }

        //[Required]
        [Display(Name = "İçerik")]
        public string takvimIcerik { get; set; }

        [Required]
        [Display(Name = "Etkinlik Başlangıç Tarihi")]
        public string takvimBasTar { get; set; }
        
        [Required]
        [Display(Name = "Etkinlik Başlangıç Saati")]
        public string takvimBasSaat { get; set; }

        [Required]
        [Display(Name = "Etkinlik Bitişi Tarihi")]
        public string takvimBitTar { get; set; }

        [Required]
        [Display(Name = "Etkinlik Bitişi Saati")]
        public string takvimBitSaat { get; set; }
        
        [Display(Name = "Tüm Gün")]
        public bool? tumGunMu { get; set; }

        [Display(Name = "Katılımcılar")]
        public string[] katilimcilar { get; set; }
        
        [Display(Name = "Lokasyon")]
        public string lokasyon { get; set; }
    }
}