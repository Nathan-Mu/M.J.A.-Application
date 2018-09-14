using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Threading.Tasks;
using DistinctionTask.Models;
using System.IO;
using Microsoft.AspNet.Identity;

namespace DistinctionTask.Controllers
{
    public class JournalistsController : Controller
    {
        private Entities db = new Entities();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Send(EmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                var message = new MailMessage();
                //message.To.Add(new MailAddress("recipient@gmail.com"));  // replace with valid value 
                foreach (String s in model.ToEmails.Split(';'))
                {
                    if (s != "")
                    {
                        message.To.Add(new MailAddress(s));
                    }
                }
                // replace with valid value 
                //message.From = new MailAddress("sender@outlook.com");  // replace with valid value
                message.From = new MailAddress("dzha152@student.monash.edu");  // replace with valid value
                message.Subject = model.Subject;
                message.Body = model.Message;
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "dzha152@student.monash.edu",  // replace with valid value
                        Password = "password"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    //smtp.Host = "smtp-mail.outlook.com";
                    smtp.Host = "smtp.monash.edu.au";
                    //smtp.Port = 587;
                    //smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Sent");
                }
            }
            return View(model);
        }

        public ActionResult Sent()
        {
            return View();
        }
        [Authorize(Roles = ("Admin"))]
        public ActionResult Send(String emailLine)
        {
            EmailFormModel email = new EmailFormModel();
            email.ToEmails = emailLine;
            return View(email);
        }

        [Authorize(Roles = ("Admin"))]
        // GET: Journalists
        public ActionResult Index()
        {
            return View(db.AspNetUsers.ToList());
        }

        [Authorize(Roles = ("Admin"))]
        [HttpPost]
        public ActionResult Index(String[] emails)
        {
            if (emails != null)
            {
                String emailLine = "";
                if (emails.Length != 0)
                {
                    foreach (String s in emails)
                    {
                        emailLine += s + ";";
                    }
                    //emailLine.Substring(0, emailLine.Length - 1);
                }
                return RedirectToAction("Send", new { emailLine = emailLine });
            }
            return View();
        }

        [Authorize]
        public ActionResult IndexIndividual()
        {
            return View(db.AspNetUsers.ToList());
        }

        [Authorize(Roles = ("Admin"))]
        // GET: Journalists/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        [Authorize]
        // GET: Journalists/Details/5
        public ActionResult DetailsIndividual(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // GET: Journalists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Journalists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,DateOfBirth,Address,FirstName,LastName")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUsers.Add(aspNetUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspNetUser);
        }

        // GET: Journalists/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // GET: Journalists/Edit/5
        public ActionResult EditIndividual(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: Journalists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,DateOfBirth,Address,FirstName,LastName")] AspNetUser aspNetUser, HttpPostedFileBase postedFile)
        {
            if (ModelState.IsValid)
            {
                if (postedFile != null)
                {
                    if (!postedFile.FileName.Substring(postedFile.FileName.Length - 5).Equals(".jpg"))
                    {
                        string path = Server.MapPath("~/Uploads/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        postedFile.SaveAs(path + Path.GetFileName(aspNetUser.Id) + ".jpg");
                        db.Entry(aspNetUser).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Only jpg type file";
                    }

                }
                else
                {
                    db.Entry(aspNetUser).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(aspNetUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditIndividual([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,DateOfBirth,Address,FirstName,LastName")] AspNetUser aspNetUser, HttpPostedFileBase postedFile)
        {
            if (ModelState.IsValid)
            {
                if (postedFile != null)
                {
                    if (!postedFile.FileName.Substring(postedFile.FileName.Length - 5).Equals(".jpg"))
                    {
                        string path = Server.MapPath("~/Uploads/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        postedFile.SaveAs(path + Path.GetFileName(User.Identity.GetUserId()) + ".jpg");
                        db.Entry(aspNetUser).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("IndexIndividual");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Only jpg type file";
                    }

                }
                else
                {
                    db.Entry(aspNetUser).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("IndexIndividual");
                }
            }

            return View(aspNetUser);


        }

        // GET: Journalists/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: Journalists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            db.AspNetUsers.Remove(aspNetUser);
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
