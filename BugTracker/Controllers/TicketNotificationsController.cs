﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Action_Filters;
using BugTracker.Models;

namespace BugTracker.Controllers
{
    [Authorize]
    [RequireHttps]
    public class TicketNotificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketNotifications
        [AdminAuthorization]
        public ActionResult Index()
        {
            var ticketNotifications = db.TicketNotifications.Include(t => t.Ticket);
            return View(ticketNotifications.ToList());
        }

        // GET: TicketNotifications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Oops5", "Home", null);
            }
            TicketNotification ticketNotification = db.TicketNotifications.Find(id);
            if (ticketNotification == null)
            {
                return RedirectToAction("Oops5", "Home", null);
            }
            return View(ticketNotification);
        }

        // GET: TicketNotifications/Create
        public ActionResult Create()
        {
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title");
            return View();
        }

        // POST: TicketNotifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TicketId,Body,SenderId,RecieverId")] TicketNotification ticketNotification)
        {
            if (ModelState.IsValid)
            {
                db.TicketNotifications.Add(ticketNotification);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketNotification.TicketId);
            return View(ticketNotification);
        }

        // GET: TicketNotifications/Edit/5
        [AdminAuthorization]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Oops5", "Home", null);
            }
            TicketNotification ticketNotification = db.TicketNotifications.Find(id);
            if (ticketNotification == null)
            {
                return RedirectToAction("Oops5", "Home", null);
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketNotification.TicketId);
            return View(ticketNotification);
        }

        // POST: TicketNotifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,Body,SenderId,RecieverId")] TicketNotification ticketNotification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketNotification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketNotification.TicketId);
            return View(ticketNotification);
        }

        // GET: TicketNotifications/Delete/5
        [AdminAuthorization]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Oops5", "Home", null);
            }
            TicketNotification ticketNotification = db.TicketNotifications.Find(id);
            if (ticketNotification == null)
            {
                return RedirectToAction("Oops5", "Home", null);
            }
            return View(ticketNotification);
        }

        // POST: TicketNotifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketNotification ticketNotification = db.TicketNotifications.Find(id);
            db.TicketNotifications.Remove(ticketNotification);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
