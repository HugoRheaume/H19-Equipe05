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
    public class ContenusController : Controller
    {
        private Entities db = new Entities();

        // GET: Contenus
        public ActionResult Index(int paysId)
        {
            ViewBag.Region = new List<string>() { "Ca", "Fr", "Eu" };
            var queryContenu = from C in db.Contenu
                               join CR in db.OffrePays on C.ContenuId equals CR.ContenuId
                               where CR.PaysId == paysId
                               orderby C.Date_de_sortie descending
                               select C;

            List<Contenu> colContenu = queryContenu.ToList();
            return View(colContenu);

        }

        public ActionResult Contenu()
        {
            int paysId = 2;
            List<Pays> pay6s = db.Pays.ToList();
            ViewBag.Region = new SelectList(db.Pays.ToList(), "PaysId", "Nom");
            //ViewBag.Region = new List<string>() { "Ca", "Fr", "Eu" };
            //var queryContenu = from C in db.Contenu
            //                   join CR in db.ContenuPays on C.ContenuId equals CR.ContenuId
            //                   where CR.PaysId == paysId
            //                   orderby C.Date_de_sortie descending
            //                   select C;

            List<Contenu> colContenu = db.Contenu.ToList();//queryContenu.ToList();

            //foreach (var contenu in colContenu)
            //{
            //    var queryLangue = from C in db.Contenu
            //                      join CL in db.Contenu_Langue on C.contenuId equals CL.contenuId
            //                      join L in db.Langue on CL.langueId equals L.langueId
            //                      where C.contenuId = contenu.contenuId
            //                      select L;
            //}


            //List<Contenu> colContenu = db.Contenu.ToList().OrderByDescending(c => c.Date_de_sortie);


            return View(colContenu); //colContenu

        }

        // GET: Contenus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contenu contenu = db.Contenu.Find(id);
            if (contenu == null)
            {
                return HttpNotFound();
            }
            return View(contenu);
        }

        // GET: Contenus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contenus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContenuId,Description,Affiche,Cote_moyenne,Nombre_de_Cote,Status,Budget,Titre_Original,Date_de_sortie,Duree")] Contenu contenu)
        {
            if (ModelState.IsValid)
            {
                db.Contenu.Add(contenu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contenu);
        }

        // GET: Contenus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contenu contenu = db.Contenu.Find(id);
            if (contenu == null)
            {
                return HttpNotFound();
            }
            return View(contenu);
        }

        // POST: Contenus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContenuId,Description,Affiche,Cote_moyenne,Nombre_de_Cote,Status,Budget,Titre_Original,Date_de_sortie,Duree")] Contenu contenu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contenu);
        }

        // GET: Contenus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contenu contenu = db.Contenu.Find(id);
            if (contenu == null)
            {
                return HttpNotFound();
            }
            return View(contenu);
        }

        // POST: Contenus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contenu contenu = db.Contenu.Find(id);
            db.Contenu.Remove(contenu);
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
