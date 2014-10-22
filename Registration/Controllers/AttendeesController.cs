using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Registration.Models;
using SendGrid;

namespace Registration.Controllers
{
    public class AttendeesController : Controller
    {
        private RegistrationContext db = new RegistrationContext();

        // GET: Attendees
        public ActionResult Index()
        {
            return View(db.Attendee.ToList());
        }

        // GET: Attendees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendee attendee = db.Attendee.Find(id);
            if (attendee == null)
            {
                return HttpNotFound();
            }
            return View(attendee);
        }

        // GET: Attendees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Attendees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,NumberOfAttendee,AttendBanquet,Comment")] Attendee attendee)
        {
            if (ModelState.IsValid)
            {
                attendee.LastUpdated = DateTime.Now;
                db.Attendee.Add(attendee);
                db.SaveChanges();
                SendEmail(attendee);
                return RedirectToAction("Index");
            }

            return View(attendee);
        }

        // GET: Attendees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendee attendee = db.Attendee.Find(id);
            if (attendee == null)
            {
                return HttpNotFound();
            }
            return View(attendee);
        }

        // POST: Attendees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,NumberOfAttendee,AttendBanquet,Comment")] Attendee attendee)
        {
            if (ModelState.IsValid)
            {
                attendee.LastUpdated = DateTime.Now;
                db.Entry(attendee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(attendee);
        }

        // GET: Attendees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendee attendee = db.Attendee.Find(id);
            if (attendee == null)
            {
                return HttpNotFound();
            }
            return View(attendee);
        }

        // POST: Attendees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Attendee attendee = db.Attendee.Find(id);
            db.Attendee.Remove(attendee);
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

        private void SendEmail(Attendee attendee)
        {
            var message = new SendGridMessage();
            message.From = new MailAddress("nateadallas@gmail.com");
            var recipients = new List<string>
                             {
                                 @"nateadallas@gmail.com",
                                 attendee.Email
                             };
            message.AddTo(recipients);
            message.Subject = "台工會2014年會活動報名";
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("<p>Dear {0}:</p>", attendee.Name));
            sb.Append("<br/>");
            sb.Append(string.Format("<p>Thank you for registering NATEA Dallas Chapter 2014 Annual Conference</p>"));
            sb.Append("<p>感謝您報名台工會- 2014年年會活動報名</p>");
            sb.Append("<br/>");
            sb.Append("<p>You have provided the head count as following:(您報名資料如下)</p>");
            sb.Append("<br/>");
            sb.Append(string.Format("<p>How many people? {0}</p>", attendee.NumberOfAttendee));
            sb.Append(string.Format("<p>Would you like to join the banquet? {0}</p>", attendee.AttendBanquet ? "Yes" : "No"));
            sb.Append(string.Format("<p>Special note: {0}</p>", attendee.Comment));
            sb.Append("<br/>");
            sb.Append("<p>===================Event Detail(活動內容)=================</p>");
            sb.Append("<br/>");
            sb.Append("<p>達拉斯台工會敬上</p>");
            message.Html = sb.ToString();

            var credentials = getNetworkCredential();
            var transportWeb = new Web(credentials);
            transportWeb.Deliver(message);
        }

        private NetworkCredential getNetworkCredential()
        {
            var username = "azure_51facc643ce1eb59de02b1b49e9c5b1d@azure.com";
            var pwd = "87IhmdmaaBZ6a9A";
            return new NetworkCredential(username, pwd);
        }
    }
}
