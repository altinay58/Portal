using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Portal.Models
{
    public class TakvimApi
    {
        private string UserId { get; set; }
        public TakvimAyar KullaniciTakvimAyar { get; set; }

        public TakvimApi(string userId, PortalEntities Db)
        {
            this.UserId = userId;

            if (string.IsNullOrEmpty(this.UserId))
                throw new Exception("User Id boş geçilemez.");
             
            this.KullaniciTakvimAyar = Db.TakvimAyars.AsNoTracking().FirstOrDefault(fo1 => fo1.RefUserId == this.UserId);
            if (this.KullaniciTakvimAyar == null)
                throw new Exception("Kullanıcıya ait takvim ayarı bulunamadı!<br/>Lütfen takvim için ayarlarınızı tanımlatınız. Tablo: TakvimAyar");
        }

        private UserCredential OturumAc()
        {
            //OTURUM BİLGİSİ OLUŞTURUYORUZ.
            UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                                                                                        new ClientSecrets
                                                                                        {
                                                                                            ClientId = this.KullaniciTakvimAyar.ApiKimlikId,
                                                                                            ClientSecret = this.KullaniciTakvimAyar.ApiGuvenlikAnahtari
                                                                                        },
                                                                                        new[] { CalendarService.Scope.Calendar}, //Yazma ve Okuma yetkisi veriyor.
                                                                                        this.KullaniciTakvimAyar.ApiKullaniciId, //Her oturum için farklı kullanıcı adı belirtilmeli yoksa en son oturum açılan kullanıcının takvimine giriş yapar.
                                                                                        CancellationToken.None
                                                                                    ).Result;

            return credential;
        }
        private CalendarService CreateCalenderService(UserCredential credential)
        {
            //CALENDER API Yİ KULLANMAK İÇİN SERVİS OLUŞTURALIM
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "GoogleCalenderApi",
            });

            return service;
        }

        public Event EtkinlikKaydet(TakvimEtkinlik etkinlik)
        {
            //Oturum açılıp, servis oluşturuluyor.
            CalendarService service = this.CreateCalenderService(this.OturumAc());

            //KATILIMCILARI EKLİYORUZ.
            List<EventAttendee> katilimcilar = null;
            if (etkinlik.Katilimcilar != null)
            {
                katilimcilar = new List<EventAttendee>();
                foreach (var eMail in etkinlik.Katilimcilar)
                    katilimcilar.Add(new EventAttendee() { Email = eMail });
            }

            DateTime? startEventDateTime = null;
            DateTime? endEventDateTime = null;

            if (!etkinlik.TumGunMu)
            {
                startEventDateTime = etkinlik.BasTar;
                endEventDateTime = etkinlik.BitTar;
            }

            Event event1 = null;
            if (!etkinlik.Id.IsNullOrEmpty())
            {
                event1 = service.Events.Get(KullaniciTakvimAyar.ApiTakvimId, etkinlik.Id).Execute();
                if (event1 == null)
                    throw new Exception("Seçilen etkinlik bulunamadı!");

                event1.Summary = etkinlik.Baslik;
                event1.Location = etkinlik.Lokasyon;
                event1.Description = etkinlik.Icerik;
                event1.Start = new EventDateTime()
                {
                    Date = etkinlik.TumGunMu ? etkinlik.BasTar.ToString("yyyy-MM-dd") : null,
                    DateTime = startEventDateTime,
                    TimeZone = "Europe/Istanbul"
                };
                event1.End = new EventDateTime()
                {
                    Date = etkinlik.TumGunMu ? etkinlik.BasTar.ToString("yyyy-MM-dd") : null,
                    DateTime = endEventDateTime,
                    TimeZone = "Europe/Istanbul",
                };
                event1.Attendees = katilimcilar;
                event1.Reminders = new Event.RemindersData()
                {
                    UseDefault = false,
                    Overrides = new EventReminder[] {
                            new EventReminder() { Method = "email", Minutes = 24 * 60 }, //1 gün önceden hatırlatma gönder(e-mail ile)
                            new EventReminder() { Method = "popup", Minutes = 30 }, //30 dakika öncesinden telefon ekranında bildirim göster.
                        }
                };

                event1.Updated = DateTime.Now;

                return service.Events.Update(event1, this.KullaniciTakvimAyar.ApiTakvimId, event1.Id).Execute();
            }
            else
            {
                //YENİ ETKİNLİK OLUŞTURMA İŞLEMLERİ
                event1 = new Event()
                {
                    Summary = etkinlik.Baslik,
                    Location = etkinlik.Lokasyon,
                    Description = etkinlik.Icerik,
                    Start = new EventDateTime()
                    {
                        Date = etkinlik.TumGunMu ? etkinlik.BasTar.ToString("yyyy-MM-dd") : null,
                        DateTime = startEventDateTime,
                        TimeZone = "Europe/Istanbul"
                    },
                    End = new EventDateTime()
                    {
                        Date = etkinlik.TumGunMu ? etkinlik.BasTar.ToString("yyyy-MM-dd") : null,
                        DateTime = endEventDateTime,
                        TimeZone = "Europe/Istanbul",
                    },
                    Attendees = katilimcilar,
                    Reminders = new Event.RemindersData()
                    {
                        UseDefault = false,
                        Overrides = new EventReminder[] {
                            new EventReminder() { Method = "email", Minutes = 24 * 60 }, //1 gün önceden hatırlatma gönder(e-mail ile)
                            new EventReminder() { Method = "popup", Minutes = 30 }, //30 dakika öncesinden telefon ekranında bildirim göster.
                        }
                    }
                };

                EventsResource.InsertRequest request = service.Events.Insert(event1, this.KullaniciTakvimAyar.ApiTakvimId);
                return request.Execute();
            }
        }
        public string EtkinlikSil(string etkinlikId)
        {
            //Oturum açılıp, servis oluşturuluyor.
            CalendarService service = this.CreateCalenderService(this.OturumAc());

            return service.Events.Delete(this.KullaniciTakvimAyar.ApiTakvimId, etkinlikId).Execute();
        }

        public Etkinlik Get(string etkinlikId)
        {
            //Oturum açılıp, servis oluşturuluyor.
            CalendarService service = this.CreateCalenderService(this.OturumAc());
            Event event1 = service.Events.Get(this.KullaniciTakvimAyar.ApiTakvimId, etkinlikId).Execute();

            return new Etkinlik
            {
                _id = event1.Id,
                title = event1.Summary,
                description = event1.Description,
                start = event1.Start.DateTime ?? Convert.ToDateTime(event1.Start.Date),
                end = event1.End.DateTime ?? Convert.ToDateTime(event1.End.Date),
                allDay = (event1.Start.DateTime == null)
            };
        }

        public List<Etkinlik> GetList(DateTime basTar,DateTime bitTar)
        {
            //Oturum açılıp, servis oluşturuluyor.
            CalendarService service = this.CreateCalenderService(this.OturumAc());

            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = basTar;
            request.TimeMax = bitTar;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            Events events = request.Execute();
            //foreach (var a in events.Items)
            //{
            //    Console.WriteLine(a.Description, a.Start.Date, a.Start.DateTime, a.End.Date, a.End.DateTime);
            //}

            return events.Items.Select(s1 => new Etkinlik
            {
                _id=s1.Id,
                title = s1.Summary,
                description = s1.Description,
                start = s1.Start.DateTime ?? Convert.ToDateTime(s1.Start.Date),
                end = s1.End.DateTime?? Convert.ToDateTime(s1.End.Date),
                allDay = (s1.Start.DateTime == null)
            }).ToList();
        }
    }

    public class TakvimEtkinlik
    {
        public string Id { get; set; }
        public DateTime BasTar { get; set; }
        public DateTime BitTar { get; set; }
        public bool TumGunMu { get; set; }

        public string Baslik { get; set; }
        public string Icerik { get; set; }
        public string Lokasyon { get; set; }

        /// <summary>
        /// E-Mail adresi yazılmalı
        /// </summary>
        public string[] Katilimcilar { get; set; }
    }

    [Serializable]
    public class Etkinlik
    {
        public string _id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime? start { get; set; }
        public DateTime? end { get; set; }
        public bool allDay { get; set; }
    }
} 