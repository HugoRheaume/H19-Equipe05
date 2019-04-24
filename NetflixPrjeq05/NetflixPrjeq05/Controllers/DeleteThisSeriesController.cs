using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NetflixPrjeq05.Models;

namespace NetflixPrjeq05.Controllers
{
    public class DeleteThisSeriesController : Controller
    {
        private H19_Projet_eq05Entities db = new H19_Projet_eq05Entities();

        // GET: DeleteThisSeries
        public ActionResult Index()
        {
            return View(db.Serie.ToList());
        }

        // GET: DeleteThisSeries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Serie serie = db.Serie.Find(id);
            if (serie == null)
            {
                return HttpNotFound();
            }
            return View(serie);
        }

        // GET: DeleteThisSeries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeleteThisSeries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nom")] Serie serie)
        {
            if (ModelState.IsValid)
            {
                db.Serie.Add(serie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(serie);
        }

        // GET: DeleteThisSeries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Serie serie = db.Serie.Find(id);
            if (serie == null)
            {
                return HttpNotFound();
            }
            return View(serie);
        }

        // POST: DeleteThisSeries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nom")] Serie serie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(serie);
        }

        // GET: DeleteThisSeries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Serie serie = db.Serie.Find(id);
            if (serie == null)
            {
                return HttpNotFound();
            }
            return View(serie);
        }

        // POST: DeleteThisSeries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Serie serie = db.Serie.Find(id);
            db.Serie.Remove(serie);
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
