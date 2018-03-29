using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class EtkinlikSilViewModel
    {
        [Required]
        [Display(Name = "Etkinlik Id")]
        public string etkinlikId { get; set; }

        [Required]
        [Display(Name = "Kullanıcı")]
        public string userId { get; set; }
    }
}