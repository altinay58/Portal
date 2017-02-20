﻿using Portal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class MailSablonlariController : BaseController
    {
        public MailSablonlariController()
        {
            ViewBag.guncelMenu = "Mailler";
        }
        // GET: MailSablonlari
        public ActionResult List()
        {
            return View(Db.MailSablonus.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MailSablonu mailSablonu = Db.MailSablonus.Find(id);
            if (mailSablonu == null)
            {
                return HttpNotFound();
            }
            return View(mailSablonu);
        }
        public ActionResult Create()
        {
            return View();
        }

        // POST: MailSablonus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MailSablonuID,MailSablonuAdi,MailSablonu1")] MailSablonu mailSablonu)
        {
            if (ModelState.IsValid)
            {
                Db.MailSablonus.Add(mailSablonu);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mailSablonu);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MailSablonu mailSablonu = Db.MailSablonus.Find(id);
            if (mailSablonu == null)
            {
                return HttpNotFound();
            }
            return View(mailSablonu);
        }

        // POST: MailSablonus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MailSablonuID,MailSablonuAdi,MailSablonu1")] MailSablonu mailSablonu)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(mailSablonu).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mailSablonu);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MailSablonu mailSablonu = Db.MailSablonus.Find(id);
            if (mailSablonu == null)
            {
                return HttpNotFound();
            }
            return View(mailSablonu);
        }

        // POST: MailSablonus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MailSablonu mailSablonu = Db.MailSablonus.Find(id);
            Db.MailSablonus.Remove(mailSablonu);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}