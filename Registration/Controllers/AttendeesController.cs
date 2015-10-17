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
			Attendee attendee = new Attendee();
			attendee.MealTypes =
				new List<Meal> 
                { 
                    new Meal
					{
						Id=1,
						Name="Regular meal 一般餐點",
						Type = MealType.Regular,
						Descriptions = 
						new List<string>
						{
							"Crowne Holiday Salad",
							"Sliced Roasted Turkey Breast with Homemade Turkey Gravy",
							"Southwestern stuffing",
							"Fresh Seasonal Vegetable",
                            "Fresh Rolls and Butter",
							"Pecan Pie"
						}
					},
					new Meal
					{
						Id = 2,
						Name="Vegetarian 素食",
						Type = MealType.Vegetarian,
						Descriptions =
						new List<string>
						{
							"Crowne Holiday Salad",
							"Linguine Primavera, Topped with Julienne Vegetables, Mushrooms and Tomato",
							"Fresh Seasonal Vegetable",
                            "Fresh Rolls and Butter",
							"Pecan Pie"
						}
					}
				};
			attendee.MealType = attendee.MealTypes.FirstOrDefault(m => m.Type == MealType.Regular).Name;
			return View(attendee);
		}

		// POST: Attendees/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Id,Name,Email,NumberOfAttendee,AttendBanquet,MealType,Comment")] Attendee attendee)
		{
			if (ModelState.IsValid)
			{
				attendee.LastUpdated = DateTime.Now;
				attendee.MealType = attendee.AttendBanquet ? attendee.MealType : string.Empty;
				db.Attendee.Add(attendee);
				db.SaveChanges();
				SendEmail(attendee);
				return RedirectToAction("RegistrationCompleted");
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


		public ActionResult RegistrationCompleted()
		{
			ViewBag.Message = "Thanks for your registration.";
			return View();
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
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("nateatxdallas@gmail.com", "piicsntxdpsbaagb")
            };

            //var message = new SendGridMessage();
            //message.From = new MailAddress("nateadallas@gmail.com");
            //var recipients = new List<string>
            //                 {
            //                     @"nateadallas@gmail.com",
            //                     attendee.Email
            //                 };
            //message.AddTo(recipients);
            //message.Subject = "台工會2015年會活動報名";
			StringBuilder sb = new StringBuilder();
			sb.Append(string.Format("<p>Dear {0}:</p>", attendee.Name));
			sb.Append("<br/>");
			sb.Append(string.Format("<p>Thank you for registering NATEA Dallas Chapter 2015 Annual Conference</p>"));
			sb.Append("<p>感謝您報名台工會- 2015年年會活動報名</p>");
			sb.Append("<br/>");
			sb.Append("<p>You have provided the head count as following:(您報名資料如下)</p>");
			sb.Append("<br/>");
			sb.Append(string.Format("<p>How many people? {0}</p>", attendee.NumberOfAttendee));
			sb.Append(string.Format("<p>Would you like to join the banquet? {0}</p>", attendee.AttendBanquet ? "Yes" : "No"));
            if (attendee.AttendBanquet)
            {
                sb.Append(string.Format("<p>Meal selection: {0}</p>", attendee.MealType));
            }
			sb.Append(string.Format("<p>Special note: {0}</p>", attendee.Comment));
			sb.Append("<br/>");
			sb.Append("<p>================Event Detail(活動內容)=================</p>");
            sb.Append("<p>NATEA-Dallas 2014 Annual Conference</p>");
            sb.Append("<p>達拉斯台工會2014年年會</p>");
            sb.Append("<p>主題:「台灣經濟與科技展望暨達拉斯台工會獎學金十年回顧」</p>");
            sb.Append("<p>Keynote speakers: 范良信教授與范李春美教授夫婦</p>");
            sb.Append("<p>講題:「台灣在經濟急速全球化下的科技努力」(Taiwan’s Technological Efforts in Rapidly Globalizing World Economy) </p>");
            sb.Append("<p>Date&Time 時間: 11/7/2015(Saturday) from 2:00pm to 9:30pm</p>");
            sb.Append("<p>Location 地點: Crowne Plaza Hotel and Resorts.  14315 Midway Road, Addison, TX 75001</p>");
            sb.Append("<p>Addmission: 免費入場.  晚宴費用: 會員$25, 非會員: $30, 學生: $15</p>");
			sb.Append("<br/>");
			sb.Append("<p>達拉斯台工會敬上</p>");

            using (var msg = new MailMessage("nateatxdallas@gmail.com", attendee.Email)
            {
                IsBodyHtml = true,
                Subject = "台工會2015年會活動報名",
                Body = sb.ToString()
            })
            {
                smtp.Send(msg);
            }

            //message.Html = sb.ToString();

            //var credentials = getNetworkCredential();
            //var transportWeb = new Web(credentials);
            //transportWeb.Deliver(message);
		}

		private NetworkCredential getNetworkCredential()
		{
			var username = "azure_51facc643ce1eb59de02b1b49e9c5b1d@azure.com";
			var pwd = "87IhmdmaaBZ6a9A";
			return new NetworkCredential(username, pwd);
		}
	}
}
