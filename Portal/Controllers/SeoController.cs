using Portal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{ 
    public class SeoController : BaseController
    {
        public SeoController()
        {
            ViewBag.guncelMenu = "ArgeBolumu";
        }
        // GET: Seo
        public ActionResult List()
        {
            var seos = Db.Seos.ToList();
            return View(seos);
        }
        public ActionResult SeoKelimeriniGetir(int? id)
        {
            var kelimeler = Db.Seos.Where(m => m.SeoRefDomainID == id).ToList();
            return View("List", kelimeler);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seo seo = Db.Seos.Find(id);
            if (seo == null)
            {
                return HttpNotFound();
            }
            return View(seo);
        }
        public ActionResult Create()
        {
            ViewBag.SeoRefDomainID = new SelectList(Db.Domains.OrderBy(m => m.DomainAdi), "DomainID", "DomainAdi");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Seo seo)
        {
            if (ModelState.IsValid)
            {
                seo.SeoGoogleSearchAdres = seo.SeoGoogleSearchAdres.Trim();
                Db.Seos.Add(seo);
                Db.SaveChanges();
                return RedirectToAction("List");
            }

            ViewBag.SeoRefDomainID = new SelectList(Db.Domains, "DomainID", "DomainAdi", seo.SeoRefDomainID);
            return View(seo);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seo seo = Db.Seos.Find(id);
            if (seo == null)
            {
                return HttpNotFound();
            }
            ViewBag.SeoRefDomainID = new SelectList(Db.Domains, "DomainID", "DomainAdi", seo.SeoRefDomainID);
            return View(seo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Seo seo)
        {
            if (ModelState.IsValid)
            {
                seo.SeoGoogleSearchAdres = seo.SeoGoogleSearchAdres.Trim();
                Db.Entry(seo).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("List");
            }
            ViewBag.SeoRefDomainID = new SelectList(Db.Domains, "DomainID", "DomainAdi", seo.SeoRefDomainID);
            return View(seo);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seo seo = Db.Seos.Find(id);
            if (seo == null)
            {
                return HttpNotFound();
            }
            return View(seo);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Seo seo = Db.Seos.Find(id);
            Db.Seos.Remove(seo);
            Db.SaveChanges();
            return RedirectToAction("List");
        }
        public ActionResult GetirGoogleSiralamasi(string SeoGoogleSearchAdres, string urlString, string kelime, string ID)
        {

            int SeoID = int.Parse(ID);

            Uri url = new Uri(urlString);
            string googleRaw = "https://www." + SeoGoogleSearchAdres + "/search?num=39&q={0}&btnG=Search";
            string tara = string.Format(googleRaw, HttpUtility.UrlEncode(kelime));
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(tara);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII))
                {
                    string html = reader.ReadToEnd();

                    Seo seom = Db.Seos.Find(SeoID);
                    seom.SeoGoogleSiralamasi = (SiralamaBul(html, url));
                    seom.SeoGoogleSiralamaTarihi = DateTime.Now;
                    
                    Db.SaveChanges();
                    return Content(seom.SeoGoogleSiralamasi.ToString());
                }
            }
        }
        private static int SiralamaBul(string html, Uri url)
        {
            string arama = @"<div class=\""g\"">\s*(.+?)\s*</div></div></div>";//Google Search'un aranan kelimeleri listelerken kullandığı etiketler burası google'a gore güncellenebilir.
            MatchCollection eslesmeler = Regex.Matches(html, arama);
            for (int i = 0; i < eslesmeler.Count; i++)
            {
                string eslesme = eslesmeler[i].Groups[0].Value;

                eslesme = Regex.Replace(eslesme, "<.*?>", string.Empty);

                if (eslesme.Contains(url.Host))
                {  // kelime eğer google sıralaması içinde varsa o kelimenin sıralamasını döndürüyor. Yoksa sıralama değeri sıfır dönüyor.
                    return i + 1;

                }
            }
            return 0;
        }
    }
}