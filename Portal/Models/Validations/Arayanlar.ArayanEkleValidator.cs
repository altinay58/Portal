using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public partial class Arayanlar
    {
        public class ArayanEkleValidator : AbstractValidator<Portal.Models.ArayanlarModels.ArayanModel>
        {
            public ArayanEkleValidator()
            {
                //EĞER this.takvimeEkle PARAMETRESİ true ise validasyon kontrolleri yapılmaktadır.
                RuleFor(r1 => r1.takvimUserId).NotEmpty().WithMessage("Takvim Kullanıcısı seçilmedi!").When(w1 => w1.takvimeEkle);
                RuleFor(r1 => r1.takvimBaslik).NotEmpty().WithMessage("Takvim başlık boş geçilemez!").When(w1 => w1.takvimeEkle);
                //RuleFor(r1 => r1.takvimLokasyon).NotEmpty().WithMessage("Lokasyon boş geçilemez!").When(w1 => w1.takvimeEkle);

                RuleFor(r1 => r1.takvimBasTar).NotEmpty().WithMessage("Takvim etkinlik başlangıç tarihi boş geçilemez!").When(w1 => w1.takvimeEkle);
                RuleFor(r1 => r1.takvimBasSaat).NotEmpty().WithMessage("Takvim etkinlik başlangıç saati boş geçilemez!").When(w1 => w1.takvimeEkle);

                RuleFor(r1 => r1.takvimBitTar).NotEmpty().WithMessage("Takvim etkinlik bitiş tarihi boş geçilemez!").When(w1 => w1.takvimeEkle && w1.takvimTumGunMu );
                RuleFor(r1 => r1.takvimBitSaat).NotEmpty().WithMessage("Takvim etkinlik bitiş saati boş geçilemez!").When(w1 => w1.takvimeEkle && w1.takvimTumGunMu );

                RuleFor(r1 => r1).Must(BasBitTarKontrol).WithMessage(w1 => w1.MesajTarihKontrol).When(w1 => w1.takvimeEkle);
            }

            private bool BasBitTarKontrol(Portal.Models.ArayanlarModels.ArayanModel m)
            {
                string basTar = (m.takvimBasTar + " " + m.takvimBasSaat);
                string bitTar = (m.takvimBitTar + " " + m.takvimBitSaat);

                bool basTarTarihMi = basTar.IsDateTime();
                bool bitTarTarihMi = bitTar.IsDateTime();

                if (!basTarTarihMi)
                    m.MesajTarihKontrol += "Başlangıç tarih ve saati geçerli formatta değil!";

                if (!bitTarTarihMi && !m.takvimTumGunMu)
                    m.MesajTarihKontrol += "Bitiş tarih ve saati geçerli formatta değil!";

                if(basTarTarihMi && bitTarTarihMi && !m.takvimTumGunMu)
                {
                    if (basTar.ConvertToDateTime() > bitTar.ConvertToDateTime())
                        m.MesajTarihKontrol += "Başlangıç tarihi, bitiş tarihinden büyük olamaz.";
                }

                return m.MesajTarihKontrol.IsNullOrEmpty(); //mesaj yoksa hata yoktur.
            }
        }
    }
}