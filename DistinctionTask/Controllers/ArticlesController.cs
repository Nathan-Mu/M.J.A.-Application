using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DistinctionTask.Models;
using Microsoft.AspNet.Identity;

namespace DistinctionTask.Controllers
{
    public class ArticlesController : Controller
    {
        private Entities db = new Entities();

        [Authorize(Roles =("Admin"))]
        // GET: Articles
        public ActionResult Index()
        {
            var articles = db.Articles.Include(a => a.AspNetUser);
            return View(articles.ToList());
        }

        [Authorize]
        public ActionResult IndexIndividual()
        {
            var articles = db.Articles.Include(a => a.AspNetUser);
            return View(articles.ToList());
        }

        [Authorize(Roles = ("Admin"))]
        // GET: Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }


        [Authorize]
        public ActionResult DetailsIndividual(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        [Authorize]
        // GET: Articles/Create
        public ActionResult Create()
        {
            ViewBag.JournalistId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        [Authorize]
        public ActionResult CreateIndividual()
        {
            ViewBag.JournalistId = new SelectList(db.AspNetUsers, "Id", "Email");
            Article article = new Models.Article();
            string currentUserId = User.Identity.GetUserId();
            article.JournalistId = currentUserId;
            article.PubDate = DateTime.Now;
            return View(article);
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArticleId,Title,PubDate,Text,JournalistId")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JournalistId = new SelectList(db.AspNetUsers, "Id", "Email", article.JournalistId);
            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateIndividual([Bind(Include = "ArticleId,Title,PubDate,Text,JournalistId")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("IndexIndividual");
            }

            ViewBag.JournalistId = new SelectList(db.AspNetUsers, "Id", "Email", article.JournalistId);
            return View(article);
        }

        [Authorize]
        // GET: Articles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.JournalistId = new SelectList(db.AspNetUsers, "Id", "Email", article.JournalistId);
            return View(article);
        }

        [Authorize]
        public ActionResult EditIndividual(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.JournalistId = new SelectList(db.AspNetUsers, "Id", "Email", article.JournalistId);
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArticleId,Title,PubDate,Text,JournalistId")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JournalistId = new SelectList(db.AspNetUsers, "Id", "Email", article.JournalistId);
            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditIndividual([Bind(Include = "ArticleId,Title,PubDate,Text,JournalistId")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexIndividual");
            }
            ViewBag.JournalistId = new SelectList(db.AspNetUsers, "Id", "Email", article.JournalistId);
            return View(article);
        }

        [Authorize]
        // GET: Articles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        [Authorize]
        public ActionResult DeleteIndividual(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("DeleteIndividual")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteIndividualConfirmed(int id)
        {
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("IndexIndividual");
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
